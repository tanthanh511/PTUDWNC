using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Services.Extensions;

namespace TatBlog.Services.Blogs
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly BlogDbContext _context;
        private readonly IMemoryCache _memoryCache;

        public CategoryRepository(BlogDbContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }

        public async Task<Category> GetCategoryBySlugAsync(
            string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Category>()
                .FirstOrDefaultAsync(a => a.UrlSlug == slug, cancellationToken);
        }

        public async Task<Category> GetCachedCategoryBySlugAsync(
            string slug, CancellationToken cancellationToken = default)
        {
            return await _memoryCache.GetOrCreateAsync(
                $"category.by-slug.{slug}",
                async (entry) =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                    return await GetCategoryBySlugAsync(slug, cancellationToken);
                });
        }

        public async Task<Category> GetCategoryByIdAsync(int categoryId)
        {
            return await _context.Set<Category>().FindAsync(categoryId);
        }

        public async Task<Category> GetCachedCategoryByIdAsync(int categoryId)
        {
            return await _memoryCache.GetOrCreateAsync(
                $"category.by-id.{categoryId}",
                async (entry) =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                    return await GetCategoryByIdAsync(categoryId);
                });
        }

        public async Task<IList<CategoryItem>> GetCategoriesAsync(
            CancellationToken cancellationToken = default)
        {
            return await _context.Set<Category>()
                .OrderBy(a => a.Name)
                .Select(a => new CategoryItem()
                {
                    Id = a.Id,
                    Name = a.Name,
                    UrlSlug = a.UrlSlug,
                    Description = a.Description,
                    ShowOnMenu = a.ShowOnMenu,
                    PostCount = a.Posts.Count(p => p.Publisded)
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<IPagedList<CategoryItem>> GetPagedCategoriesAsync(
            IPagingParams pagingParams,
            string name = null,
            CancellationToken cancellationToken = default)
        {
            return await _context.Set<Category>()
                .AsNoTracking()
                .WhereIf(!string.IsNullOrWhiteSpace(name),
                    x => x.Name.Contains(name))
                .Select(a => new CategoryItem()
                {
                    Id = a.Id,
                    Name = a.Name,
                    UrlSlug = a.UrlSlug,
                    Description = a.Description,
                    ShowOnMenu = a.ShowOnMenu,
                    PostCount = a.Posts.Count(p => p.Publisded)
                })
                .ToPagedListAsync(pagingParams, cancellationToken);
        }
        public async Task<IPagedList<CategoryItem>> GetPagedCategoriesAsync(
            int pageSize = 10, int pageNumber = 1,
            string name = null,
            CancellationToken cancellationToken = default)
        {
            return await _context.Set<Category>()
                .AsNoTracking()
                .WhereIf(!string.IsNullOrWhiteSpace(name),
                    x => x.Name.Contains(name))
                .Select(a => new CategoryItem()
                {
                    Id = a.Id,
                    Name = a.Name,
                    UrlSlug = a.UrlSlug,
                    Description = a.Description,
                    ShowOnMenu = a.ShowOnMenu,
                    PostCount = a.Posts.Count(p => p.Publisded)
                })
                .ToPagedListAsync(pageSize, pageNumber, nameof(Category.Name), "DESC", cancellationToken);
        }

        public async Task<bool> AddOrUpdateAsync(
            Category category, CancellationToken cancellationToken = default)
        {
            category.ShowOnMenu= true;
            if (category.Id > 0)
            {
                _context.Categories.Update(category);
                _memoryCache.Remove($"category.by-id.{category.Id}");
            }
            else
            {
                _context.Categories.Add(category);
            }

            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<bool> DeleteCategoryAsync(
            int categoryId, CancellationToken cancellationToken = default)
        {
            return await _context.Categories
                .Where(x => x.Id == categoryId)
                .ExecuteDeleteAsync(cancellationToken) > 0;
        }

        public async Task<bool> IsCategorySlugExistedAsync(
            int categoryId,
            string slug,
            CancellationToken cancellationToken = default)
        {
            return await _context.Categories
                .AnyAsync(x => x.Id != categoryId && x.UrlSlug == slug, cancellationToken);
        }

        public async Task<IPagedList<T>> GetPagedCategoriesAsync<T>(
            Func<IQueryable<Category>,
            IQueryable<T>> mapper,
            IPagingParams pagingParams,
            string name = null,
            CancellationToken cancellationToken = default)
        {
            var authorQuery = _context.Set<Category>().AsNoTracking();

            if (!string.IsNullOrEmpty(name))
            {
                authorQuery = authorQuery.Where(x => x.Name.Contains(name));
            }

            return await mapper(authorQuery)
                .ToPagedListAsync(pagingParams, cancellationToken);
        }

        //public async Task<bool> SetImageUrlAsync(
        //	int authorId, string imageUrl,
        //	CancellationToken cancellationToken = default)
        //{
        //	return await _context.Authors
        //		.Where(x => x.Id == authorId)
        //		.ExecuteUpdateAsync(x => 
        //			x.SetProperty(a => a.ImageUrl, a => imageUrl), 
        //			cancellationToken) > 0;
        //}
    }
}
