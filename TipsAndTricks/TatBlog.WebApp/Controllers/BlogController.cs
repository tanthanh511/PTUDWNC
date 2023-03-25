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

        #region tìm theo từ khóa
        public async Task<IActionResult> Index(
             [FromQuery(Name = "k")] string keyword = null,
             [FromQuery(Name = "p")] int pageNumber = 1,
             [FromQuery(Name = "ps")] int pageSize = 5)
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
        #endregion

        #region tìm category theo slug
        public async Task<IActionResult> Category(
             [FromRoute(Name = "slug")] string slug = null,
             [FromQuery(Name = "p")] int pageNumber = 1,
             [FromQuery(Name = "ps")] int pageSize = 10)
        {       
            // tạo đối tượng chứa các điều kiện truy vấn
            var postQuery = new PostQuery()
            {
                // chỉ lấy những bài viết có trạng thái pub
                PublishedOnly = true,
                CategorySlug = slug,
                // tìm bài viết theo từ khóa
                
            };

            // truy vấn các bài viết theo điều kiện đã tạo
            var postsList = await _blogRepository
                .GetPagedPostAsync(postQuery, pageNumber, pageSize);


            // lưu lại điều kiện truy vấn để hiển thị trong view
            ViewBag.PostQuery = postQuery;

            // truyền danh sách bài viết vào view để render ra html
            return View(postsList);
        }
        #endregion

        #region tìm author theo slug
        public async Task<IActionResult> Author(
             [FromRoute(Name = "slug")] string slug,
             [FromQuery(Name = "p")] int pageNumber = 1,
             [FromQuery(Name = "ps")] int pageSize = 5)
        {
            // tạo đối tượng chứa các điều kiện truy vấn
            var postQuery = new PostQuery()
            {
                // chỉ lấy những bài viết có trạng thái pub
                PublishedOnly = true,
                AuthorSlug = slug,
                // tìm bài viết theo từ khóa

            };

            // truy vấn các bài viết theo điều kiện đã tạo
            var postsList = await _blogRepository
                .GetPagedPostAsync(postQuery, pageNumber, pageSize);


            // lưu lại điều kiện truy vấn để hiển thị trong view
            ViewBag.PostQuery = postQuery;

            // truyền danh sách bài viết vào view để render ra html
            return View(postsList);
        }
        #endregion

        #region tìm tag theo slug
        public async Task<IActionResult> Tag(
             [FromRoute(Name = "slug")] string slug ,
             [FromQuery(Name = "p")] int pageNumber = 1,
             [FromQuery(Name = "ps")] int pageSize = 10)
        {
            // tạo đối tượng chứa các điều kiện truy vấn
            var postQuery = new PostQuery()
            {
                // chỉ lấy những bài viết có trạng thái pub
                PublishedOnly = true,
                TagSlug = slug,
                // tìm bài viết theo từ khóa

            };

            // truy vấn các bài viết theo điều kiện đã tạo
            var postsList = await _blogRepository
                .GetPagedPostAsync(postQuery, pageNumber, pageSize);


            // lưu lại điều kiện truy vấn để hiển thị trong view
            ViewBag.PostQuery = postQuery;

            // truyền danh sách bài viết vào view để render ra html
            return View(postsList);
        }
        #endregion

        #region tìm post theo slug
        public async Task<IActionResult> Post(
             
             [FromRoute(Name ="year")] int year ,
             [FromRoute(Name = "month")] int month,
             [FromRoute(Name = "day")] int day,
             [FromRoute(Name = "slug")] string slug = null,
             int pageNumber = 1,
             int pageSize = 5)
        {
 
            // tạo đối tượng chứa các điều kiện truy vấn
            var postQuery = new PostQuery()
            {
                
                // chỉ lấy những bài viết có trạng thái pub
                PublishedOnly = true,
                PostedYear = year,
                PostedMonth = month,
                PostedDay = day,
                PostSlug = slug
                
                

                // tìm bài viết theo từ khóa

            };
             if(!postQuery.PublishedOnly)
            {
                throw new Exception("error: cannot web");
            }    

            // truy vấn các bài viết theo điều kiện đã tạo
            var postsList = await _blogRepository
                .GetPostAsync(year, month, slug);


            // lưu lại điều kiện truy vấn để hiển thị trong view
            ViewBag.PostQuery = postQuery;

            // truyền danh sách bài viết vào view để render ra html
            return View(postsList);
        }
        #endregion

        #region postInfo
        public async Task<IActionResult> PostInfo(

             [FromRoute(Name = "year")] int year,
             [FromRoute(Name = "month")] int month,
             [FromRoute(Name = "day")] int day,
             [FromRoute(Name = "slug")] string slug = null,
             int pageNumber = 1,
             int pageSize = 5)
        {


            // truy vấn các bài viết theo điều kiện đã tạo
            var postsList = await _blogRepository
                .GetPostAsync(year, month, slug);


            // lưu lại điều kiện truy vấn để hiển thị trong view
            //ViewBag.PostQuery = postQuery;

            // truyền danh sách bài viết vào view để render ra html
            return View(postsList);
        }
        #endregion

        #region Archives năm tháng
        public async Task<IActionResult> Archives(

            [FromRoute(Name = "year")] int year,
            [FromRoute(Name = "month")] int month,
            int pageNumber = 1,
            int pageSize = 5)
        {

            var postQuery = new PostQuery()
            {

                // chỉ lấy những bài viết có trạng thái pub
                PublishedOnly = true,
                PostedYear = year,
                PostedMonth = month
                // tìm bài viết theo từ khóa

            };

            // truy vấn các bài viết theo điều kiện đã tạo
            var postsList = await _blogRepository
                .GetPagedPostAsync(postQuery, pageNumber, pageSize);


            // lưu lại điều kiện truy vấn để hiển thị trong view
                ViewBag.PostQuery = postQuery;

            // truyền danh sách bài viết vào view để render ra html
            return View(postsList);
        }
        #endregion


        public IActionResult Contact()=> View();

        public IActionResult About()=>View();
      
        public IActionResult Rss()
            => Content("Nội dung sẽ được cập nhập");

        public IActionResult Login() => View();
        
    }
}
