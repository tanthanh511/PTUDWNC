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
        #region Action này xử lí HTTP request đến trang chủ của ứng dụng web hoặc tìm kiếm bài viết theo từ khóa 
        public async Task<IActionResult> Index(
            [FromQuery(Name ="p")] int pageNumber =1,
            [FromQuery(Name ="ps")] int pageSize= 10)
        {
            // tạo đối tuoiwngj chứa các điều kiện truy vấn
            var postQuery = new PostQuery()
            {
                // chỉ lấy những bài viết có trang thái Publised
                PublishedOnly = true
            };

            // truy vấn các bài viết theo điều kiện đã tạo 
            var postsList = await _blogRepository
                .GetPagedPostAsync(postQuery, pageNumber,pageSize);
            // lưu lại các bài viết theo điều kiện đã tạo 
            ViewBag.PostQuery = postQuery;
            // truyền lại danh sách bài viết vào view để render ra HTML
            return View(postsList);
        }


        //public IActionResult Index()
        //{
        //    ViewBag.CurrentTime = DateTime.Now.ToString("HH:mm.ss");
        //    return View();
        //}
        #endregion


        public IActionResult Contact()=> View();

        public IActionResult About()=>View();
      
        public IActionResult Rss()
            => Content("Nội dung sẽ được cập nhập");

    }
}
