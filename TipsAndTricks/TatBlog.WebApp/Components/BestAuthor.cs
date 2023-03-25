using Microsoft.AspNetCore.Mvc;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Components
{
    public class BestAuthor : ViewComponent
    {
        private readonly IBlogRepository _blogRepository;
        public BestAuthor(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var tags = await _blogRepository.ListAuthorAsync(4);
            return View(tags);
        }
    }
}
