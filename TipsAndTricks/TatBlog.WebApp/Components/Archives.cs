using Microsoft.AspNetCore.Mvc;
using TatBlog.Data.Contexts;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Components;

    public class Archives : ViewComponent
{

    private readonly IBlogRepository _blogRepository;

    public Archives(IBlogRepository blogRepository)
    {
        _blogRepository = blogRepository;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        // lấy danh sách chủ đề
        var archivesPosts = await _blogRepository.CountPostsNMonthAsync(12);
        return View(archivesPosts);
    }
}


