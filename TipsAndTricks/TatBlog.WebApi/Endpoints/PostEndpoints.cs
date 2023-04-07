using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging.Configuration;
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
    public static class PostEndpoints
    {
        public static WebApplication MapPostEndpoints(
            this WebApplication app)
        {
            var routeGroupBuilder = app.MapGroup("/api/posts");

            //routeGroupBuilder.MapGet("/", GetPosts)
            //    .WithName("GetPosts")
            //    .Produces<ApiResponse<PaginationResult<PostQuery>>>();

            routeGroupBuilder.MapGet("/{id:int}", GetPostDetails)
                .WithName("GetPostsByPostId")
                .Produces<ApiResponse<PostItem>>()
                .Produces(401);

            routeGroupBuilder.MapGet(
                "/{slug:regex(^[a-z0-9_-]+$)}/posts",
                GetPostsBySlug)
                .WithName("GetPostsByPostSlug")
                .Produces<ApiResponse<PaginationResult<PostDto>>>();

            routeGroupBuilder.MapPost("/", AddPost)
                .WithName("AddNewPost")
                .Accepts<PostEditModel>("multipart/form-data")
                .Produces(401)
                .Produces<ApiResponse<PostItem>>();

            routeGroupBuilder.MapPost("/{id:int}/avatar", SetPostPicture)
               .WithName("SetPostPicture")
               .Accepts<IFormFile>("multipart/form-data")
               .Produces<ApiResponse<string>>()
               .Produces(401);

            //routeGroupBuilder.MapPut("/{id:int}", UpdatePost)
            //   .WithName("UpdateAnPost")
            //   .Produces(401)
            //   .Produces<ApiResponse<string>>();

            routeGroupBuilder.MapDelete("/{id:int}", DeletePost)
               .WithName("DeleteAnPost")
               .Produces(401)
               .Produces<ApiResponse<string>>();

            routeGroupBuilder.MapGet("/get-posts-filter", GetFilteredPosts)
               .WithName("GetFilteredPosts")
               .Produces<ApiResponse<PostDto>>();

            routeGroupBuilder.MapGet("/get-filter", GetFilter)
              .WithName("GetFilter")
              .Produces < ApiResponse<PostFilterModel>>();
            return app;
        }

        //private static async Task<IResult> GetPosts(
        //    [AsParameters] PostFilterModel model,
        //    IPostRepository postRepository,
        //      IMapper mapper)
        //{
        //    var postQuery = mapper.Map<PostQuery>(model);
        //    var postsList = await postRepository.GetPagedPostAsync(
        //      postQuery, model,
        //      posts => posts.ProjectToType<PostDto>());

        //    var paginationResult = new PaginationResult<PostDto>(postsList);

        //    return Results.Ok(ApiResponse.Success(paginationResult));
        //}

        private static async Task<IResult> GetPostDetails(
            int id,
            IPostRepository postRepository,
            IMapper mapper)
        {
            var post = await postRepository.GetCachedPostByIdAsync(id);
            return post == null
                ? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound,$"khong tim thaybai viet co ma so {id}"))
                : Results.Ok(ApiResponse.Success(mapper.Map<PostItem>(post)));
        }

        private static async Task<IResult> GetPostsByPostId(
            int id,
            [AsParameters] PagingModel pagingModel,
            IPostRepository postRepository)
        {
            var postQuery = new PostQuery()
            {
                Id = id,
                PublishedOnly = true,
            };
            var postsList = await postRepository.GetPagedPostAsync(
                postQuery, pagingModel,
                posts => posts.ProjectToType<PostDto>());

            var paginationResult = new PaginationResult<PostDto>(postsList);
            return Results.Ok(ApiResponse.Success(paginationResult));
        }

        private static async Task<IResult> GetPostsBySlug(
            [FromRoute] string slug,
            [AsParameters] PagingModel pagingModel,
            IPostRepository postRepository)
        {
            var postQuery = new PostQuery()
            {
                PostSlug = slug,
                PublishedOnly = true,
            };
            var postsList = await postRepository.GetPagedPostAsync(
                postQuery, pagingModel,
                posts => posts.ProjectToType<PostDto>());
            var paginationResult = new PaginationResult<PostDto>(postsList);
            return Results.Ok(ApiResponse.Success(paginationResult));
        }



        private static async Task<IResult> AddPost(
            HttpContext context,
           // PostEditModel model,
           // IValidator<PostEditModel> validator,
            IPostRepository postRepository,
            IBlogRepository blogRepository,
            IMapper mapper,
            IMediaManager mediaManager)
        {
            var model = await PostEditModel.BindAsync(context);
            var slug = model.Title.GenerateSlug();
            if (await postRepository.IsPostSlugExistedAsync(model.Id, slug))
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict,$"Slug'{slug}' da duoc su dung cho bai viet khac"));
            }
            var post = model.Id > 0 ? await
                postRepository.GetPostByIdAsync(model.Id) : null;
            if (post == null)
            {
                post = new Post()
                {
                    PostedDate = DateTime.Now,
                };
            }

            post.Title = model.Title;
            post.AuthorId = model.AuthorId;
            post.CategoryId = model.CategoryId;
            post.ShortDesciption = model.ShortDesciption;
            post.Description = model.Description;
            post.Meta = model.Meta;
            post.Publisded = model.Published;
            post.ModifiedDate = DateTime.Now;
            post.UrlSlug = model.Title.GenerateSlug();

            if (model.ImageFile?.Length>0)
            {
                string hostname =
                    $"{context.Request.Scheme}://{context.Request.Host}{context.Request.PathBase}/",
                    uploadedPath = await
                    mediaManager.SaveFileAsync(model.ImageFile.OpenReadStream(),
                    model.ImageFile.FileName,
                    model.ImageFile.ContentType);
                if(!string.IsNullOrWhiteSpace(uploadedPath))
                {
                    post.ImageUrl = uploadedPath;
                }
                    
            }
            await blogRepository.CreateOrUpdatePostAsync(post,model.GetSelectedTags());
            return Results.Ok(ApiResponse.Success(
                mapper.Map<PostItem>(post), HttpStatusCode.Created));
            //var post = mapper.Map<Post>(model);
            //await postRepository.AddOrUpdateAsync(post);
            //return Results.CreatedAtRoute(
            //    "GetPostById", new { post.Id },
            //    mapper.Map<PostItem>(post));

        }

        private static async Task<IResult> SetPostPicture(
            int id, IFormFile imageFile,
            IPostRepository postRepository,
            IMediaManager mediaManager)
        {
            var imageUrl = await mediaManager.SaveFileAsync(
                imageFile.OpenReadStream(),
                imageFile.FileName, imageFile.ContentType);
            if (string.IsNullOrWhiteSpace(imageUrl))
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.BadRequest,"khong luu duoc tap tin"));
            }
            await postRepository.SetImageUrlAsync(id, imageUrl);
            return Results.Ok(ApiResponse.Success(imageUrl));
        }

        //private static async Task<IResult> UpdatePost(
        //    int id, PostEditModel model,
        //    IValidator<PostEditModel> validator,
        //    IPostRepository postRepository,
        //    IMapper mapper)
        //{
        //    var validationResult = await validator.ValidateAsync(model);
        //    if (!validationResult.IsValid)
        //    {
        //        return Results.Ok(ApiResponse.Fail(HttpStatusCode.BadRequest,
        //            validationResult));
        //    }

        //    if (await postRepository
        //        .IsPostSlugExistedAsync(id, model.UrlSlug))
        //    {
        //        return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict,
        //            $"Slug '{model.UrlSlug}' da duoc su dung"));
        //    }
        //    var post = mapper.Map<Post>(model);
        //    post.Id = id;

        //    return await postRepository.AddOrUpdateAsync(post)
        //        ? Results.Ok(ApiResponse.Success("post is update",HttpStatusCode.NoContent))
        //        : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "could not file post"));
        //}

        private static async Task<IResult> DeletePost(
            int id, IPostRepository postRepository)
        {
            return await postRepository.DeletePostAsync(id)
                ? Results.Ok(ApiResponse.Success("post is delete", HttpStatusCode.NoContent))
                : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "could not find post with id"));
        }

        private static async Task<IResult> GetFilter(
            IAuthorRepository authorRepository,
            IBlogRepository blogRepository,
            ICategoryRepository categoryRepository)
        {
            var model = new PostFilterModel()
            {
                AuthorList = (await authorRepository.GetAuthorsAsync())
                .Select(a => new SelectListItem()
                {
                    Text = a.FullName,
                    Value = a.Id.ToString()
                }),

                CategoryList = (await categoryRepository.GetCategoriesAsync())
                .Select(c => new SelectListItem()
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                })
            };
            return Results.Ok(ApiResponse.Success(model));
        }

        private static async Task<IResult> GetFilteredPosts(
            [AsParameters] PostFilterModel model,
            [AsParameters] PagingModel pagingModel,
            IBlogRepository blogRepository)
        {
            var postQuery = new PostQuery()
            {
                Keyword = model.Keyword,
                CategoryId = model.CategoryId,
                AuthorId = model.AuthorId,
                PostedYear = model.Year,
                PostedMonth = model.Month,
            };
            var postsList = await blogRepository.GetPagedPostsAsync(
                postQuery, pagingModel, posts => posts.ProjectToType<PostDto>());

            var paginationResult = new PaginationResult<PostDto>(postsList);
            return Results.Ok(ApiResponse.Success(paginationResult));
        }
    }
       
}
