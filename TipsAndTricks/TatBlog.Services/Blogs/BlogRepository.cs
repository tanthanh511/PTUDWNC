using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Core.Contracts;
using TatBlog.Services.Extensions;
using System.Linq.Dynamic.Core;
using TatBlog.Core.Constants;


namespace TatBlog.Services.Blogs
{
    public class BlogRepository : IBlogRepository
    {
        // cài đặt phương thức khỏi tạo cho blogRepossitiory
        #region
        private readonly BlogDbContext _context;
        public BlogRepository( BlogDbContext context)
        {
            _context = context;
        }
        #endregion

        // **** điểm cần chú ý là throw new notImplementedException: đưa ra ngoài lệ cho một phương thức 

        // tìm bài viết có định danh là (slug) và được đăng vào tháng năm nào đó ....
        #region
        public async Task<Post> GetPostAsync(int year, int month, string slug, CancellationToken cancellationToken = default)
        {
            IQueryable<Post> postsQuery = _context.Set<Post>()
                .Include(x => x.Category)
                .Include(x => x.Author);

            if (year > 0)
            {
                postsQuery = postsQuery.Where(x => x.PostedDate.Year == year);
            }

            if (month > 0)
            {
                postsQuery = postsQuery.Where(x => x.PostedDate.Month == month);
            }
            if (!string.IsNullOrWhiteSpace(slug))
            {
                postsQuery = postsQuery.Where(x => x.UrlSlug == slug);
            }
            // bất đồng bộ sử dụng await để tách hai hàm ra với nhau dể sử lí hơn 
            return await postsQuery.FirstOrDefaultAsync(cancellationToken);
           
        }
        #endregion

        // tìm một bài post top N bài viết được moiij người xem nhất 
        #region
        public async Task<IList<Post>> GetPopularArticlesAsync(int numPosts, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>()
                .Include(x => x.Author)
                .Include(x => x.Category)
                .OrderByDescending(p => p.Viewcount)
                .Take(numPosts)
                .ToListAsync(cancellationToken);
            //throw new NotImplementedException();
        }
        #endregion

        // tăng(view)  cho một bài viết 
        #region
        public async Task IncreaseViewCountAsync(int postID, CancellationToken cancellationToken = default)
        {
            await _context.Set<Post>()
           .Where(x => x.Id == postID)
           .ExecuteUpdateAsync(p =>
           p.SetProperty(x => x.Viewcount, x => x.Viewcount + 1), cancellationToken);
           // throw new NotImplementedException();
        }
        #endregion

        // kiểm tra xem tên định danh của bài viết đã có hay chưa có 
        #region
        public async Task<bool> IsPostSlugExistedAsync(int postID, string slug, CancellationToken cancellationToken = default)
        {

            return await _context.Set<Post>()
               .AnyAsync(x => x.Id != postID && x.UrlSlug == slug, cancellationToken);
            //throw new NotImplementedException();
        }
        #endregion

        // lấy danh sách chuyên mục và sô lượng bài viết và nảm thuộc từng chuyên mục chủ đề
        #region
        public async Task<IList<CategoryItem>> GetCategoriesAsync(bool showOnMenu = false, CancellationToken cancellationToken = default)
        {
            IQueryable<Category> categories = _context.Set<Category>();
            if( showOnMenu)
            {
                categories = categories.Where(x => x.ShowOnMenu == showOnMenu); 
            }
            return await categories
                .OrderBy(x => x.Id)
                .Select(x=> new CategoryItem()
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlSlug = x.UrlSlug,
                    Description = x.Description,
                    ShowOnMenu = x.ShowOnMenu,
                    PostCount = x.Posts.Count(p=> p.Publisded)
                })
                .ToListAsync(cancellationToken);  

        }
        #endregion

        //lấy danh sach tu khao the va phan trang 
        #region
        public async Task<IPagedList<TagItem>> GetPagedTagsAsync(
            IPagingParams pagingParams,
            CancellationToken cancellationToken = default)
        {
            var tagQuery = _context.Set<Tag>()
                .Select(x => new TagItem()
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlSlug= x.UrlSlug,
                    Description= x.Description,
                    PostCount = x.Posts.Count(p=> p.Publisded)
                });
            return await tagQuery
                .ToPagedListAsync(pagingParams, cancellationToken);
        }
        #endregion

        // tìm một thẻ tag có định danh là slug
        #region
        public async Task<Tag> GetTagAsync(string slug, CancellationToken cancellationToken = default)
        {

            IQueryable<Tag> tagsQuery = _context.Set<Tag>();
            if (!string.IsNullOrWhiteSpace(slug))
            {
                tagsQuery = tagsQuery.Where(x => x.UrlSlug == slug);
            }
            return await tagsQuery.FirstOrDefaultAsync(cancellationToken);
        }
        #endregion

        #region"lấy danh sách thẻ tag kèm theo số bài viết chứa thẻ đó"
        public async Task<IList<TagItem>> GetAllByTagNumberAsync(CancellationToken cancellationToken = default)
        {
            var tagsItemQuery = _context.Set<Tag>();
            return await tagsItemQuery
                .OrderBy(x => x.Name)
                .Select(x => new TagItem()
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlSlug = x.UrlSlug,
                    Description = x.Description,
                    PostCount = x.Posts.Count(p => p.Publisded)
                }).ToListAsync(cancellationToken);                                           
        }
        #endregion

        #region"xóa một mã theo thẻ cho trước "
        public async Task<bool> TagDeleteByID(int id, CancellationToken cancellationToken = default)
        {
            await _context.Database
                .ExecuteSqlRawAsync("DELETE FROM PostTags WHERE TagId = " + id, cancellationToken);

            await _context.Database
                .ExecuteSqlRawAsync("DELETE FROM Tags WHERE Id = " + id, cancellationToken);


            var rows = await _context.Set<Tag>()
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync(cancellationToken);

            return rows > 0;
        }
        #endregion

        #region"tìm một chuyên mục category theo định danh slug "
        public async Task<Category> GetCategorybySlugAsync(string slug, CancellationToken cancellationToken = default)
        {

                IQueryable<Category> categoryQuery = _context.Set<Category>();
                if (!string.IsNullOrWhiteSpace(slug))
                {
                     categoryQuery = categoryQuery.Where(x => x.UrlSlug == slug);
                }
                return await categoryQuery.FirstOrDefaultAsync(cancellationToken);
            
           // throw new NotImplementedException();
        }
        #endregion

        #region"tìm chuyên mục theo ID"
        public async Task<Category> GetCategoryByID(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Category>()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
            //throw new NotImplementedException();
        }

        #endregion

        #region"kiem tra category bang slug and id"
        public async Task<bool> IsCategorySlugExistedAsync(int id, string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Category>()
                .AnyAsync(c => c.Id != id && c.UrlSlug == slug, cancellationToken);
            
        }
        #endregion

        #region"thêm một chuyên mục"
        public async Task AddOrUpdateCategory(Category category, CancellationToken cancellationToken = default)
        {
            if (IsCategorySlugExistedAsync(category.Id, category.UrlSlug).Result)
                await Console.Out.WriteLineAsync("Error: Exsited Slug");
            else
                if (category.Id > 0)
                await _context.Set<Category>()
                .Where(c => c.Id == category.Id)
                .ExecuteUpdateAsync(c => c
                .SetProperty(x => x.Name, x => category.Name)
                .SetProperty(x => x.UrlSlug, x => category.UrlSlug)
                .SetProperty(x => x.Description, x => category.Description)
                .SetProperty(x => x.ShowOnMenu, category.ShowOnMenu)
                .SetProperty(x => x.Posts, category.Posts),
                cancellationToken);
            else
            {
                _context.categories.Add(category);
                _context.SaveChanges();
            }
            
        }
        #endregion

        #region"xoa chuyên muc theo ID"
        public async Task<bool> DeleteCategoryByID(int id, CancellationToken cancellationToken = default)
        {
            
          int a =  await _context.Database
               .ExecuteSqlRawAsync("DELETE FROM Posts WHERE CategoryId = " + id, cancellationToken);

          int b=   await _context.Database
                .ExecuteSqlRawAsync("DELETE FROM Category WHERE Id = " + id, cancellationToken);

            if (a > 0 && b>0)
                return true;
            return false;

        }
        #endregion

        #region lấy và phân trang của category
        public async Task<IPagedList<CategoryItem>> GetPagedCategoriesAsync(IPagingParams pagingParams, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Category>()
                .Select(c => new CategoryItem {
                    Id = c.Id,
                    Name = c.Name,
                    UrlSlug = c.UrlSlug,
                    Description = c.Description,
                    ShowOnMenu = c.ShowOnMenu,
                    PostCount = c.Posts.Count(p=> p.Publisded),
                })
                .ToPagedListAsync(pagingParams, cancellationToken);
            //throw new NotImplementedException();
        }
        #endregion

        #region tìm kiếm và phân trang của bài viết(post)
        public async Task<IPagedList<Post>> GetPagedPostsAsync(PostQuery condition, IPagingParams pagingParams, CancellationToken cancellationToken = default)
        {
            return await FilterPost(condition)
                    .ToPagedListAsync(pagingParams, cancellationToken);
        }
        public IQueryable<Post> FilterPost(PostQuery condition)
        {
            return _context.Set<Post>()
                .Include(c => c.Category)
                .Include(t => t.Tags)
                .Include(a => a.Author)
                .WhereIf(condition.AuthorId > 0, p => p.AuthorId == condition.AuthorId)
                .WhereIf(!string.IsNullOrWhiteSpace(condition.AuthorSlug), p => p.UrlSlug == condition.AuthorSlug)
                .WhereIf(condition.PostId > 0, p => p.Id == condition.PostId)
                .WhereIf(condition.CategoryId > 0, p => p.CategoryId == condition.CategoryId)
                .WhereIf(!string.IsNullOrWhiteSpace(condition.CategorySlug), p => p.Category.UrlSlug == condition.CategorySlug)
                .WhereIf(condition.PostedYear > 0, p => p.PostedDate.Year == condition.PostedYear)
                .WhereIf(condition.PostedMonth > 0, p => p.PostedDate.Month == condition.PostedMonth)
                .WhereIf(condition.TagId > 0, p => p.Tags.Any(x => x.Id == condition.TagId))
                .WhereIf(!string.IsNullOrWhiteSpace(condition.TagSlug), p => p.Tags.Any(x => x.UrlSlug == condition.TagSlug))
                .WhereIf(condition.PublishedOnly != null, p => p.Publisded == condition.PublishedOnly);
        }
     

        public async Task<IPagedList<Post>> GetPagedPostAsync(PostQuery condition, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            return await FilterPost(condition)
                .ToPagedListAsync(
                    pageNumber, pageSize,
                    nameof(Post.PostedDate), "DESC",
                    cancellationToken);
        }

        #endregion
    }
}
