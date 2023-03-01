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

namespace TatBlog.Services.Blogs
{
    public class BlogRepository : IBlogRepository
    {
        // cài đặt phương thức khỏi tạo cho blogRepossitiory
        private readonly BlogDbContext _context;
        public BlogRepository( BlogDbContext context)
        {
            _context = context;
        }



        // **** điểm cần chú ý là throw new notImplementedException: đưa ra ngoài lệ cho một phương thức 

        // tìm bài viết có định danh là (slug) và được đăng vào tháng năm nào đó ....
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

        // tìm một bài post top N bài viết được moiij người xem nhất 
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

        // tăng(view)  cho một bài viết 
        public async Task IncreaseViewCountAsync(int postID, CancellationToken cancellationToken = default)
        {
            await _context.Set<Post>()
           .Where(x => x.Id == postID)
           .ExecuteUpdateAsync(p =>
           p.SetProperty(x => x.Viewcount, x => x.Viewcount + 1), cancellationToken);
           // throw new NotImplementedException();
        }

    


        // kiểm tra xem tên định danh của bài viết đã có hay chưa có 
        public async Task<bool> IsPostSlugExistedAsync(int postID, string slug, CancellationToken cancellationToken = default)
        {

            return await _context.Set<Post>()
               .AnyAsync(x => x.Id != postID && x.UrlSlug == slug, cancellationToken);
            //throw new NotImplementedException();
        }


        // lấy danh sách chuyên mục và sô lượng bài viết và nảm thuộc từng chuyên mục chủ đề
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

        //lấy danh sach tu khao the va phan trang 

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



    }
}
