using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using TatBlog.Core.Constants;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Services.Extensions;



namespace TatBlog.Services.Blogs
{
    public class PostRepository : IPostRepository
    {
        private readonly BlogDbContext _context;
        private readonly IMemoryCache _memoryCache;

        public PostRepository(BlogDbContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }

        public async Task<Post> GetPostBySlugAsync(
            string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>()
                .FirstOrDefaultAsync(a => a.UrlSlug == slug, cancellationToken);
        }

        public async Task<Post> GetCachedPostBySlugAsync(
            string slug, CancellationToken cancellationToken = default)
        {
            return await _memoryCache.GetOrCreateAsync(
                $"post.by-slug.{slug}",
                async (entry) =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                    return await GetPostBySlugAsync(slug, cancellationToken);
                });
        }

        public async Task<Post> GetPostByIdAsync(int postId)
        {
            return await _context.Set<Post>().FindAsync(postId);
        }

        public async Task<Post> GetCachedPostByIdAsync(int postId)
        {
            return await _memoryCache.GetOrCreateAsync(
                $"category.by-id.{postId}",
                async (entry) =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                    return await GetPostByIdAsync(postId);
                });
        }

        public async Task<IList<PostItem>> GetPostsAsync(
            CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>()
                .OrderBy(a => a.Title)
                .Select(a => new PostItem()
                {
                    Id = a.Id,
                    Title = a.Title,
                    UrlSlug = a.UrlSlug,
                    Description = a.Description,
                    ShortDesciption = a.ShortDesciption,
                    Meta = a.Meta,
                    ImageUrl = a.ImageUrl,
                    Viewcount = a.Viewcount,
                    CategoryName = a.Category.Name,
                    Publisded = a.Publisded
                   // PostCount = a.Tags.Count(p => p.)
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<IPagedList<PostItem>> GetPagedPostsAsync(
            IPagingParams pagingParams,
            string title = null,
            CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>()
                .AsNoTracking()
                .WhereIf(!string.IsNullOrWhiteSpace(title),
                    x => x.Title.Contains(title))
                .Select(a => new PostItem()
                {
                    Id = a.Id,
                    Title = a.Title,
                    UrlSlug = a.UrlSlug,
                    Description = a.Description,
                    ShortDesciption = a.ShortDesciption,
                    Meta = a.Meta,
                    ImageUrl = a.ImageUrl,
                    Viewcount = a.Viewcount,
                    CategoryName = a.Category.Name,
                    Publisded = a.Publisded
                })
                .ToPagedListAsync(pagingParams, cancellationToken);
        }
        public async Task<IPagedList<PostItem>> GetPagedPostsAsync(
            int pageSize = 10, int pageNumber = 1,
            string title = null,
            CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>()
                .AsNoTracking()
                .WhereIf(!string.IsNullOrWhiteSpace(title),
                    x => x.Title.Contains(title))
                .Select(a => new PostItem()
                {

                    Id = a.Id,
                    Title = a.Title,
                    UrlSlug = a.UrlSlug,
                    Description = a.Description,
                    ShortDesciption = a.ShortDesciption,
                    Meta = a.Meta,
                    ImageUrl = a.ImageUrl,
                    Viewcount = a.Viewcount,
                    CategoryName = a.Category.Name,
                    Publisded = a.Publisded
                })
                .ToPagedListAsync(pageSize, pageNumber, nameof(Post.Title), "DESC", cancellationToken);
        }

        public async Task<bool> AddOrUpdateAsync(
            Post post, CancellationToken cancellationToken = default)
        {
            post.Publisded= true;
            if (post.Id > 0)
            {
                _context.Posts.Update(post);
                _memoryCache.Remove($"category.by-id.{post.Id}");
            }
            else
            {
                _context.Posts.Add(post);
            }

            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<bool> DeletePostAsync(
            int postId, CancellationToken cancellationToken = default)
        {
            return await _context.Posts
                .Where(x => x.Id == postId)
                .ExecuteDeleteAsync(cancellationToken) > 0;
        }

        public async Task<bool> IsPostSlugExistedAsync(
            int postId,
            string slug,
            CancellationToken cancellationToken = default)
        {
            return await _context.Categories
                .AnyAsync(x => x.Id != postId && x.UrlSlug == slug, cancellationToken);
        }

        public async Task<IPagedList<T>> GetPagedCategoriesAsync<T>(
            Func<IQueryable<Post>,
            IQueryable<T>> mapper,
            IPagingParams pagingParams,
            string title = null,
            CancellationToken cancellationToken = default)
        {
            var postQuery = _context.Set<Post>().AsNoTracking();

            if (!string.IsNullOrEmpty(title))
            {
                postQuery = postQuery.Where(x => x.Title.Contains(title));
            }

            return await mapper(postQuery)
                .ToPagedListAsync(pagingParams, cancellationToken);
        }

        public Task<IPagedList<T>> GetPagedPostsAsync<T>(Func<IQueryable<Post>, IQueryable<T>> mapper, IPagingParams pagingParams, string name = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IPagedList<Post>> GetPagedPostsAsync(PostQuery condition, IPagingParams pagingParams, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<IPagedList<T>> GetPagedPostAsync<T>(PostQuery pq, IPagingParams pagingParams, Func<IQueryable<Post>, IQueryable<T>> mapper, CancellationToken cancellationToken = default)
        {
            var posts = FilterPost(pq);
            var mapperPosts = mapper(posts);
            return await mapperPosts
                .ToPagedListAsync(pagingParams, cancellationToken);
        }
        public IQueryable<Post> FilterPost(PostQuery condition)
        {
            //    var query = _context.Set<Post>()
            //        .Include(c => c.Category)
            //        .Include(t => t.Tags)
            //        .Include(a => a.Author);
            //    return query.WhereIf(condition.AuthorId > 0, p => p.AuthorId == condition.AuthorId)
            //        .WhereIf(!string.IsNullOrWhiteSpace(condition.AuthorSlug), p => p.Author.UrlSlug == condition.AuthorSlug)
            //        .WhereIf(condition.PostId > 0, p => p.Id == condition.PostId)
            //        .WhereIf(condition.CategoryId > 0, p => p.CategoryId == condition.CategoryId)
            //        .WhereIf(!string.IsNullOrWhiteSpace(condition.CategorySlug), p => p.Category.UrlSlug == condition.CategorySlug)
            //        .WhereIf(condition.PostedYear > 0, p => p.PostedDate.Year == condition.PostedYear)
            //        .WhereIf(condition.PostedMonth > 0, p => p.PostedDate.Month == condition.PostedMonth)
            //        .WhereIf(condition.TagId > 0, p => p.Tags.Any(x => x.Id == condition.TagId))
            //        .WhereIf(!string.IsNullOrWhiteSpace(condition.TagSlug), p => p.Tags.Any(x => x.UrlSlug == condition.TagSlug))
            //        .WhereIf(condition.PublishedOnly != null, p => p.Publisded == condition.PublishedOnly);

            IQueryable<Post> posts = _context.Set<Post>()
            .Include(x => x.Category)
            .Include(x => x.Author)
            .Include(x => x.Tags);

            if (condition.PublishedOnly)
            {
                posts = posts.Where(x => x.Publisded);
            }

            if (condition.NotPublisded)
            {
                posts = posts.Where(x => !x.Publisded);
            }

            if (condition.CategoryId > 0)
            {
                posts = posts.Where(x => x.CategoryId == condition.CategoryId);
            }

            if (!string.IsNullOrWhiteSpace(condition.CategorySlug))
            {
                posts = posts.Where(x => x.Category.UrlSlug == condition.CategorySlug);
            }

            if (condition.AuthorId > 0)
            {
                posts = posts.Where(x => x.AuthorId == condition.AuthorId);
            }

            if (!string.IsNullOrWhiteSpace(condition.AuthorSlug))
            {
                posts = posts.Where(x => x.Author.UrlSlug == condition.AuthorSlug);
            }

            if (!string.IsNullOrWhiteSpace(condition.TagSlug))
            {
                posts = posts.Where(x => x.Tags.Any(t => t.UrlSlug == condition.TagSlug));
            }

            if (!string.IsNullOrWhiteSpace(condition.Keyword))
            {
                posts = posts.Where(x => x.Title.Contains(condition.Keyword) ||
                                         x.ShortDesciption.Contains(condition.Keyword) ||
                                         x.Description.Contains(condition.Keyword) ||
                                         x.Category.Name.Contains(condition.Keyword) ||
                                         x.Tags.Any(t => t.Name.Contains(condition.Keyword)));
            }

            if (condition.PostedYear > 0)
            {
                posts = posts.Where(x => x.PostedDate.Year == condition.PostedYear);
            }

            if (condition.PostedMonth > 0)
            {
                posts = posts.Where(x => x.PostedDate.Month == condition.PostedMonth);
            }

            if (condition.PostedDay > 0)
            {
                posts = posts.Where(x => x.PostedDate.Day == condition.PostedDay);
            }

            //if (!string.IsNullOrWhiteSpace(condition.CategorySlug))
            //{
            //    posts = posts.Where(x => x.UrlSlug == condition.CategorySlug);
            //}
            if (!string.IsNullOrWhiteSpace(condition.PostSlug))
            {
                posts = posts.Where(x => x.UrlSlug == condition.PostSlug);
            }

            return posts;

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
