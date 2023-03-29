using Microsoft.AspNetCore.Mvc;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;

namespace TatBlog.WebApp.Components
{
    public class BestAuthor : ViewComponent
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IAuthorRepository _authorRepository;
        public BestAuthor(IBlogRepository blogRepository, IAuthorRepository authorRepository)
        {
            _blogRepository = blogRepository;
            _authorRepository = authorRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var authors = await _blogRepository.ListAuthorAsync(4);
            return View(authors);
        }
    }
}
