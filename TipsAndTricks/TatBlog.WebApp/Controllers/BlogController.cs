using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using TatBlog.Services.Blogs;
using TatBlog.Core.Constants;

namespace TatBlog.WebApp.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogRepository _blogRepository;

        public BlogController(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public async Task<IActionResult> Index(
             [FromQuery(Name = "k")] string keyword = null,
             [FromQuery(Name = "p")] int pageNumber = 1,
             [FromQuery(Name = "ps")] int pageSize = 10)
        {
            // tạo đối tượng chứa các điều kiện truy vấn
            var postQuery = new PostQuery()
            {
                // chỉ lấy những bài viết có trạng thái pub
                PublishedOnly = true,

                // tìm bài viết theo từ khóa
                Keyword = keyword
            };

            // truy vấn các bài viết theo điều kiện đã tạo
            var postsList = await _blogRepository
                .GetPagedPostAsync(postQuery, pageNumber, pageSize);
            // lưu lại điều kiện truy vấn để hiển thị trong view
            ViewBag.PostQuery = postQuery;

            // truyền danh sách bài viết vào view để render ra html
            return View(postsList);
        }

        public IActionResult Contact()=> View();

        public IActionResult About()=>View();
      
        public IActionResult Rss()
            => Content("Nội dung sẽ được cập nhập");

    }
}
