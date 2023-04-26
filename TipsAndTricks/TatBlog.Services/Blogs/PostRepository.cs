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

        

        //  giu
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

        // giu
        public async Task<bool> DeletePostAsync(
            int postId, CancellationToken cancellationToken = default)
        {
            return await _context.Posts
                .Where(x => x.Id == postId)
                .ExecuteDeleteAsync(cancellationToken) > 0;
        }


        // giu
        public async Task<bool> IsPostSlugExistedAsync(
            int postId,
            string slug,
            CancellationToken cancellationToken = default)
        {
            return await _context.Posts
                .AnyAsync(x => x.Id != postId && x.UrlSlug == slug, cancellationToken);
        }

        //public async Task<IPagedList<T>> GetPagedCategoriesAsync<T>(
        //    Func<IQueryable<Post>,
        //    IQueryable<T>> mapper,
        //    IPagingParams pagingParams,
        //    string title = null,
        //    CancellationToken cancellationToken = default)
        //{
        //    var postQuery = _context.Set<Post>().AsNoTracking();

        //    if (!string.IsNullOrEmpty(title))
        //    {
        //        postQuery = postQuery.Where(x => x.Title.Contains(title));
        //    }

        //    return await mapper(postQuery)
        //        .ToPagedListAsync(pagingParams, cancellationToken);
        //}

        public async Task<IPagedList<T>> GetPagedPostAsync<T>(PostQuery pq, IPagingParams pagingParams, Func<IQueryable<Post>, IQueryable<T>> mapper, CancellationToken cancellationToken = default)
        {
            var posts = FilterPost(pq);
            var mapperPosts = mapper(posts);
            return await mapperPosts
                .ToPagedListAsync(pagingParams, cancellationToken);
        }
        public IQueryable<Post> FilterPost(PostQuery condition)
        {
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

        // giu
        public async Task<bool> SetImageUrlAsync(
            int postId, string imageUrl,
            CancellationToken cancellationToken = default)
        {
            return await _context.Posts
                .Where(x => x.Id == postId)
                .ExecuteUpdateAsync(x =>
                    x.SetProperty(a => a.ImageUrl, a => imageUrl),
                    cancellationToken) > 0;
        }

        public async Task<IList<Post>> GetPopularArticlesAsync(int numPosts, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>()
                .Include(x => x.Author)
                .Include(x => x.Category)
                .OrderByDescending(p => p.Viewcount)
                .Take(numPosts)
                .ToListAsync(cancellationToken);

        }

        public async Task<IPagedList<AuthorItem>> GetPagedBestAuthorsAsync(IPagingParams pagingParams, int amount = 1, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Author>()
                .AsNoTracking()
                .OrderByDescending(a => a.Posts.Count())
                .Take(amount)
                .Select(a => new AuthorItem()
                {
                    Id = a.Id,
                    FullName = a.FullName,
                    Email = a.Email,
                    JoinedDate = a.JoinedDate,
                    ImageUrl = a.ImageUrl,
                    UrlSlug = a.UrlSlug,
                    PostCount = a.Posts.Count(p => p.Publisded)
                })
                .ToPagedListAsync(pagingParams, cancellationToken);
        }

        public async Task<IList<Post>> GetRandomArticlesAsync(int numPosts, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>()
              .OrderBy(x => Guid.NewGuid())
              .Take(numPosts)
              .ToListAsync(cancellationToken);
        }

        public async Task<IList<TagItem>> GetTagsAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Set<Tag>()
            .OrderBy(x => x.Name)
            .Select(x => new TagItem()
            {
                Id = x.Id,
                Name = x.Name,
                UrlSlug = x.UrlSlug,
                Description = x.Description,
                PostCount = x.Posts.Count(p => p.Publisded)
            })
            .ToListAsync(cancellationToken);
        }
    }
}


//public async Task<IList<Post>> GetPostsAsync(
//    CancellationToken cancellationToken = default)
//{
//    return await _context.Set<Post>()
//        .OrderBy(a => a.Title)
//        .Select(a => new Post()
//        {
//            Id = a.Id,
//            Title = a.Title,
//            UrlSlug = a.UrlSlug,
//            Description = a.Description,
//            ShortDesciption = a.ShortDesciption,
//            Meta = a.Meta,
//            ImageUrl = a.ImageUrl,
//            Viewcount = a.Viewcount,
//         //   CategoryName = a.Category.Name,
//            Publisded = a.Publisded
//           // PostCount = a.Tags.Count(p => p.)
//        })
//        .ToListAsync(cancellationToken);
//}

//public async Task<IPagedList<PostItem>> GetPagedPostsAsync(
//    IPagingParams pagingParams,
//    string title = null,
//    CancellationToken cancellationToken = default)
//{
//    return await _context.Set<Post>()
//        .AsNoTracking()
//        .WhereIf(!string.IsNullOrWhiteSpace(title),
//            x => x.Title.Contains(title))
//        .Select(a => new PostItem()
//        {
//            Id = a.Id,
//            Title = a.Title,
//            UrlSlug = a.UrlSlug,
//            Description = a.Description,
//            ShortDesciption = a.ShortDesciption,
//            Meta = a.Meta,
//            ImageUrl = a.ImageUrl,
//            Viewcount = a.Viewcount,
//            CategoryName = a.Category.Name,
//            Publisded = a.Publisded
//        })
//        .ToPagedListAsync(pagingParams, cancellationToken);
//}
//public async Task<IPagedList<PostItem>> GetPagedPostsAsync(
//    int pageSize = 10, int pageNumber = 1,
//    string title = null,
//    CancellationToken cancellationToken = default)
//{
//    return await _context.Set<Post>()
//        .AsNoTracking()
//        .WhereIf(!string.IsNullOrWhiteSpace(title),
//            x => x.Title.Contains(title))
//        .Select(a => new PostItem()
//        {

//            Id = a.Id,
//            Title = a.Title,
//            UrlSlug = a.UrlSlug,
//            Description = a.Description,
//            ShortDesciption = a.ShortDesciption,
//            Meta = a.Meta,
//            ImageUrl = a.ImageUrl,
//            Viewcount = a.Viewcount,
//            CategoryName = a.Category.Name,
//            Publisded = a.Publisded
//        })
//        .ToPagedListAsync(pageSize, pageNumber, nameof(Post.Title), "DESC", cancellationToken);
//}


//public async Task<Post> GetPostBySlugAsync(
//    string slug, CancellationToken cancellationToken = default)
//{
//    return await _context.Set<Post>()
//        .FirstOrDefaultAsync(a => a.UrlSlug == slug, cancellationToken);
//}

//public async Task<Post> GetCachedPostBySlugAsync(
//    string slug, CancellationToken cancellationToken = default)
//{
//    return await _memoryCache.GetOrCreateAsync(
//        $"post.by-slug.{slug}",
//        async (entry) =>
//        {
//            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
//            return await GetPostBySlugAsync(slug, cancellationToken);
//        });
//}