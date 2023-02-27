using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;

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
           // throw new NotImplementedException();
        }

        // thực thi câu truy vấn 
   

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

      
    }
}
