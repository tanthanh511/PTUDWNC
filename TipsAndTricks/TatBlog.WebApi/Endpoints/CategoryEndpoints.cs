using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Configuration;
using TatBlog.Core.Collections;
using TatBlog.Core.Constants;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.WebApi.Extensions;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Endpoints
{
    public static class CategoryEndpoints
    {
        public static WebApplication MapCategoryEndpoints(
            this WebApplication app)
        {
            var routeGroupBuilder = app.MapGroup("/api/categories");

            routeGroupBuilder.MapGet("/", GetCategories)
                .WithName("GetCategories")
                .Produces<PaginationResult<CategoryItem>>();

            routeGroupBuilder.MapGet("/{id:int}", GetCategoryDetails)
                .WithName("GetCategoryById")
                .Produces<CategoryItem>()
                .Produces(404);

            routeGroupBuilder.MapGet(
                "/{slug:regex(^[a-z0-9_-]+$)}/posts",
                GetPostsByCategorySlug)
                .WithName("GetPostsByCategorySlug")
                .Produces<PaginationResult<PostDto>>();

            routeGroupBuilder.MapPost("/", AddCategory)
                .WithName("AddNewCategory")
                .Produces(201)
                .Produces(400)
                .Produces(409);
            //routeGroupBuilder.MapPost("/{id:int}/avatar", SetCategoryPicture)
            //   .WithName("SetCategoryPicture")
            //   .Accepts<IFormFile>("multipart/form-data")
            //   .Produces<string>()
            //   .Produces(400);
            routeGroupBuilder.MapPut("/{id:int}", UpdateCategory)
               .WithName("UpdateAnCategory")
               .Produces(204)
               .Produces(400)
               .Produces(409);
            routeGroupBuilder.MapDelete("/{id:int}", DeleteCategory)
               .WithName("DeleteAnCategory")
               .Produces(204)
               .Produces(404);
            return app;
        }

        private static async Task<IResult> GetCategories(
            [AsParameters] CategoryFilterModel model,
            ICategoryRepository categoryRepository)
        {
            var categoriesList = await categoryRepository
                .GetPagedCategoriesAsync(model, model.Name);
            var paginationResult = new PaginationResult<CategoryItem>(categoriesList);
            return Results.Ok(paginationResult);
        }

        private static async Task<IResult> GetCategoryDetails(
            int id,
            ICategoryRepository categoryRepository,
            IMapper mapper)
        {
            var category = await categoryRepository.GetCachedCategoryByIdAsync(id);
            return category == null
                ? Results.NotFound($"khong tim thay chu de co ma so {id}")
                : Results.Ok(mapper.Map<CategoryItem>(category));
        }

        private static async Task<IResult> GetPostsByCategoryId(
            int id,
            [AsParameters] PagingModel pagingModel,
            IBlogRepository blogRepository)
        {
            var postQuery = new PostQuery()
            {
                CategoryId = id,
                PublishedOnly = true,
            };
            var postsList = await blogRepository.GetPagedPostsAsync(
                postQuery, pagingModel,
                posts => posts.ProjectToType<PostDto>());

            var paginationResult = new PaginationResult<PostDto>(postsList);
            return Results.Ok(paginationResult);
        }

        private static async Task<IResult> GetPostsByCategorySlug(
            [FromRoute] string slug,
            [AsParameters] PagingModel pagingModel,
            IBlogRepository blogRepository)
        {
            var postQuery = new PostQuery()
            {
                CategorySlug = slug,
                PublishedOnly = true,
            };
            var postsList = await blogRepository.GetPagedPostsAsync(
                postQuery, pagingModel,
                posts => posts.ProjectToType<PostDto>());
            var paginationResult = new PaginationResult<PostDto>(postsList);
            return Results.Ok(paginationResult);
        }



        private static async Task<IResult> AddCategory(
            CategoryEditModel model,
            IValidator<CategoryEditModel> validator,
            ICategoryRepository categoryRepository,
            IMapper mapper)
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                return Results.BadRequest(
                    validationResult.Errors.ToResponse());
            }
            if (await categoryRepository.IsCategorySlugExistedAsync(0, model.UrlSlug))
            {
                return Results.Conflict($"Slug'{model.UrlSlug}' da duoc su dung");
            }
            var category = mapper.Map<Category>(model);
            await categoryRepository.AddOrUpdateAsync(category);
            return Results.CreatedAtRoute(
                "GetCategoryById", new { category.Id },
                mapper.Map<CategoryItem>(category));

        }

        //private static async Task<IResult> SetCategoryPicture(
        //    int id, IFormFile imageFile,
        //    IAuthorRepository authorRepository,
        //    IMediaManager mediaManager)
        //{
        //    var imageUrl = await mediaManager.SaveFileAsync(
        //        imageFile.OpenReadStream(),
        //        imageFile.FileName, imageFile.ContentType);
        //    if (string.IsNullOrWhiteSpace(imageUrl))
        //    {
        //        return Results.BadRequest("khong luu duoc tap tin");
        //    }
        //    await authorRepository.SetImageUrlAsync(id, imageUrl);
        //    return Results.Ok(imageUrl);
        //}

        private static async Task<IResult> UpdateCategory(
            int id, CategoryEditModel model,
            IValidator<CategoryEditModel> validator,
            ICategoryRepository categoryRepository,
            IMapper mapper)
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                return Results.BadRequest(
                    validationResult.Errors.ToResponse());
            }

            if (await categoryRepository
                .IsCategorySlugExistedAsync(id, model.UrlSlug))
            {
                return Results.Conflict(
                    $"Slug '{model.UrlSlug}' da duoc su dung");
            }
            var category = mapper.Map<Category>(model);
            category.Id = id;

            return await categoryRepository.AddOrUpdateAsync(category)
                ? Results.NoContent()
                : Results.NotFound();
        }

        private static async Task<IResult> DeleteCategory(
            int id, ICategoryRepository categoryRepository)
        {
            return await categoryRepository.DeleteCategoryAsync(id)
                ? Results.NoContent()
                : Results.NotFound($"could not find category with id={id}");
        }
    }
       
}
