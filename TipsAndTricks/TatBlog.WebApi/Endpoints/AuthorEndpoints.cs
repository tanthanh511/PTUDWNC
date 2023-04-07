using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using TatBlog.Core.Collections;
using TatBlog.Core.Constants;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.WebApi.Extensions;
using TatBlog.WebApi.Filters;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Endpoints
{
    public static class AuthorEndpoints
    {
        public static WebApplication MapAuthorEndpoints(
            this WebApplication app)
        {
            var routeGroupBuilder = app.MapGroup("/api/authors");

            routeGroupBuilder.MapGet("/", GetAuthors)
                .WithName("GetAuthors")
                .Produces<ApiResponse<PaginationResult<AuthorItem>>>();

            routeGroupBuilder.MapGet("/{id:int}", GetAuthorDetails)
                .WithName("GetAuthorById")
                .Produces<ApiResponse<AuthorItem>>();
                //.Produces(404);


            routeGroupBuilder.MapGet("/best/{limit}", GetNBestPostsByAuthor)
                .WithName("GetNBestPostsByAuthor")
                .Produces<ApiResponse<PaginationResult<AuthorItem>>>()
                .Produces(401);

            routeGroupBuilder.MapGet(
                "/{slug:regex(^[a-z0-9_-]+$)}/posts",
                GetPostsByAuthorSlug)
                .WithName("GetPostsByAuthorSlug")
                .Produces<ApiResponse<PaginationResult<PostDto>>>();

            routeGroupBuilder.MapPost("/", AddAuthor)
                .AddEndpointFilter< ValidatorFilter<AuthorEditModel>>()
                .WithName("AddNewAuthor")
                //.Produces(201)
                .Produces(401)
                //.Produces(409);
                .Produces<ApiResponse<AuthorItem>>();

            routeGroupBuilder.MapPost("/{id:int}/avatar", SetAuthorPicture)
               .WithName("SetAuthorPicture")
               .Accepts<IFormFile>("multipart/form-data")
               .Produces<ApiResponse<string>>()
               .Produces(401);

            routeGroupBuilder.MapPut("/{id:int}", UpdateAuthor)
               .WithName("UpdateAnAuthor")
               .Produces(401)
               .Produces<ApiResponse<string>>();
               //.Produces(400)
               //.Produces(409);
            routeGroupBuilder.MapDelete("/{id:int}", DeleteAuthor)
               .WithName("DeleteAnAuthor")
               .Produces(401)
               .Produces<ApiResponse<string>>();
            return app;
        }

        private static async Task<IResult> GetAuthors(
            [AsParameters] AuthorFilterModel model,
            IAuthorRepository authorRepository)
        {
            var authorsList = await authorRepository
                .GetPagedAuthorsAsync(model, model.Name);
            var paginationResult = new PaginationResult<AuthorItem>(authorsList);
            return Results.Ok(ApiResponse.Success(paginationResult));
        }

        private static async Task<IResult> GetNBestPostsByAuthor(
            int limit,
         [AsParameters] AuthorFilterModel model,
         IAuthorRepository authorRepository)
        {
            var authorsList = await authorRepository
                .GetPagedBestAuthorsAsync(model, limit);
            var paginationResult = new PaginationResult<AuthorItem>(authorsList);
            return Results.Ok(ApiResponse.Success(paginationResult));
        }

        private static async Task<IResult> GetAuthorDetails(
            int id,
            IAuthorRepository authorRepository,
            IMapper mapper)
        {
            var author = await authorRepository.GetCachedAuthorByIdAsync(id);
            return author == null
                ? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound,$"khong tim thay tac gia co ma so {id}"))
                : Results.Ok(ApiResponse.Success(mapper.Map<AuthorItem>(author)));
        }

        private static async Task<IResult> GetPostsByAuthorId(
            int id,
            [AsParameters] PagingModel pagingModel,
            IBlogRepository blogRepository)
        {
            var postQuery = new PostQuery()
            {
                AuthorId = id,
                PublishedOnly = true,
            };
            var postsList = await blogRepository.GetPagedPostsAsync(
                postQuery, pagingModel, 
                posts => posts.ProjectToType<PostDto>());

            var paginationResult = new PaginationResult<PostDto>(postsList);
            return Results.Ok(ApiResponse.Success(paginationResult));
        }

        private static async Task<IResult> GetPostsByAuthorSlug(
            [FromRoute] string slug,
            [AsParameters] PagingModel pagingModel,
            IBlogRepository blogRepository)
        {
            var postQuery = new PostQuery()
            {
                AuthorSlug = slug,
                PublishedOnly = true,
            };
            var postsList = await blogRepository.GetPagedPostsAsync(
                postQuery, pagingModel,
                posts => posts.ProjectToType<PostDto>());
            var paginationResult = new PaginationResult<PostDto>(postsList);
            return Results.Ok(ApiResponse.Success(paginationResult));
        }



        private static async Task<IResult> AddAuthor(
            AuthorEditModel model,
            IValidator<AuthorEditModel> validator,
            IAuthorRepository authorRepository,
            IMapper mapper)
        {           
            if (await authorRepository.IsAuthorSlugExistedAsync(0, model.UrlSlug))
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict,$"Slug'{model.UrlSlug}' da duoc su dung"));
            }
            var author = mapper.Map<Author>(model);
            await authorRepository.AddOrUpdateAsync(author);
            return Results.Ok(ApiResponse.Success(mapper.Map<AuthorItem>(author), HttpStatusCode.Created));
                
        }

        private static async Task<IResult> SetAuthorPicture(
            int id , IFormFile imageFile,
            IAuthorRepository authorRepository,
            IMediaManager mediaManager)
        {
            var imageUrl = await mediaManager.SaveFileAsync(
                imageFile.OpenReadStream(),
                imageFile.FileName,
                imageFile.ContentType);
            if(string.IsNullOrWhiteSpace(imageUrl))
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.BadRequest,"khong luu duoc tap tin"));
            }
            await authorRepository.SetImageUrlAsync(id, imageUrl);
            return Results.Ok(ApiResponse.Success(imageUrl));
        }

        private static async Task<IResult> UpdateAuthor(
            int id, AuthorEditModel model,
            IValidator<AuthorEditModel> validator,
            IAuthorRepository authorRepository,
            IMapper mapper)
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.BadRequest, validationResult));
            }

            if (await authorRepository
                .IsAuthorSlugExistedAsync(id, model.UrlSlug))
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, $"Slug '{model.UrlSlug}' da duoc su dung"));
            }
            var author = mapper.Map<Author>(model);
            author.Id = id;

            return await authorRepository.AddOrUpdateAsync(author)
                ? Results.Ok(ApiResponse.Success("author is update", HttpStatusCode.NoContent))
                : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound,"Could not file author"));
        }

        private static async Task<IResult> DeleteAuthor(
            int id, IAuthorRepository authorRepository)
        {
            return await authorRepository.DeleteAuthorAsync(id)
                ? Results.Ok(ApiResponse.Success("author is deleted",HttpStatusCode.NoContent))
                : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "could not find author"));
        }
    }


    // t còn một cái chưa viết á    
}
