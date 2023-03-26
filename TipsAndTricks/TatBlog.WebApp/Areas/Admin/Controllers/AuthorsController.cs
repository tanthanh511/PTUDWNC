using FluentValidation;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Areas.Admin.Controllers;
public class AuthorsController :Controller
{
    private readonly ILogger<AuthorsController> _logger;
    private readonly IBlogRepository _blogRepository;
    private readonly IMediaManager _mediaManager;
    private readonly IMapper _mapper;
    //private readonly IValidator<PostEditModel> _postValidator;
    public AuthorsController(IBlogRepository blogRepository, IValidator<PostEditModel> postValidator, ILogger<PostsController> logger, IMediaManager mediaManager, IMapper mapper)
    {
        //_postValidator = postValidator;
        //_logger = logger;
        _blogRepository = blogRepository;
        _mediaManager = mediaManager;
        _mapper = mapper;
    }
    public async Task<IActionResult> Index(
       //CategoryFilterModel model,
       [FromQuery(Name = "p")] int pageNumber = 1,
       [FromQuery(Name = "ps")] int pageSize = 5)
    {

        var model = await _blogRepository.GetPagedAuthorsAsync(pageNumber, pageSize);

        return View(model);
    }

   

 
   
    public async Task<IActionResult> DeleteAuthor(int id = 0)
    {
        await _blogRepository.DeleteAuthorAsync(id);
        return RedirectToAction(nameof(Index));
    }
}

