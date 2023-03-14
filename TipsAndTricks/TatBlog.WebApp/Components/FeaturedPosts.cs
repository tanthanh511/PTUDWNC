using Microsoft.AspNetCore.Mvc;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Components;
public class FeaturedPosts :  ViewComponent
{
    public readonly IBlogRepository _blogRepository;

    public FeaturedPosts(IBlogRepository blogRepository)
    {
        _blogRepository = blogRepository;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        // lấy danh sách chủ đề
        var featuredPosts = await _blogRepository.GetPopularArticlesAsync(3);
        return View(featuredPosts);


    }
}

