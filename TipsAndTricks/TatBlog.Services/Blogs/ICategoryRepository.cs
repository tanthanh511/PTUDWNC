using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;

namespace TatBlog.Services.Blogs;

public interface ICategoryRepository
{
    Task<Category> GetCategoryBySlugAsync(
        string slug,
        CancellationToken cancellationToken = default);

    Task<Category> GetCachedCategoryBySlugAsync(
        string slug, CancellationToken cancellationToken = default);

    Task<Category> GetCategoryByIdAsync(int categoryId);

    Task<Category> GetCachedCategoryByIdAsync(int categoryId);

    Task<IList<CategoryItem>> GetCategoriesAsync(
        CancellationToken cancellationToken = default);

    Task<IPagedList<CategoryItem>> GetPagedCategoriesAsync(
        IPagingParams paginationParams,
        string name = null,
        CancellationToken cancellationToken = default);

    Task<IPagedList<CategoryItem>> GetPagedCategoriesAsync(
        int pageSize, int pageNumber,
        string name = null,
        CancellationToken cancellationToken = default);


    Task<IPagedList<T>> GetPagedCategoriesAsync<T>(
        Func<IQueryable<Category>, IQueryable<T>> mapper,
        IPagingParams pagingParams,
        string name = null,
        CancellationToken cancellationToken = default);

    Task<bool> AddOrUpdateAsync(
        Category category,
        CancellationToken cancellationToken = default);

    Task<bool> DeleteCategoryAsync(
        int categoryId,
        CancellationToken cancellationToken = default);

    Task<bool> IsCategorySlugExistedAsync(
        int categoryId, string slug,
        CancellationToken cancellationToken = default);

    //Task<bool> SetImageUrlAsync(
    //	int authorId, string imageUrl,
    //	CancellationToken cancellationToken = default);
}