using FluentValidation;
using FluentValidation.AspNetCore;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TatBlog.Core.Constants;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Areas.Admin.Controllers;

public class PostsController : Controller
{
    private readonly ILogger<PostsController> _logger;
    private readonly IBlogRepository _blogRepository;
    private readonly IMediaManager _mediaManager;
  
    private readonly IMapper _mapper;
    private readonly IValidator<PostEditModel> _postValidator;

    public PostsController(IBlogRepository blogRepository,IValidator<PostEditModel> postValidator,ILogger<PostsController> logger, IMediaManager mediaManager, IMapper mapper)
    {
        _postValidator = postValidator;
        _logger = logger;
        _blogRepository = blogRepository;
        _mediaManager = mediaManager;
        _mapper = mapper;
    }
    public async Task<IActionResult> Index(
        PostFilterModel model,
        [FromQuery(Name ="p")] int pageNumber =1,
        [FromQuery(Name = "ps")] int pageSize = 5)
    {
        // var postQuery = new PostQuery()

        //{
        //    Keyword = model.Keyword,
        //    CategoryId = model.CategoryID,
        //    AuthorId = model.AuthorID,
        //    PostedYear = model.Year,
        //    PostedMonth = model.Month
        //};
       
         
        _logger.LogInformation("Tạo điều kiện truy vấn");

        // Sử dụng mapster để tạo đối tượng từ đối tượng postFilterModel model 
        var postQuery = _mapper.Map<PostQuery>(model);

        _logger.LogInformation("lấy danh sách bài viết từ cơ sở dữ liệu");
        ViewBag.PostsList = await _blogRepository.GetPagedPostAsync(postQuery, pageNumber, pageSize);

        _logger.LogInformation(" chuẩn bị dữ liệu cho viewmodel");

        await PopulatePostFilterModelAsync(model);
        return View(model);
    }

   
    public async Task<IActionResult> DeletePost(int id = 0)
    {
        await _blogRepository.DeletePostAsync(id);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> TogglePublished(int id = 0)
    {
        await _blogRepository.TogglePublishedFlagAsync(id);
        return RedirectToAction(nameof(Index));
    }


    #region PopulatePostFilterModelAsync
    private async Task PopulatePostFilterModelAsync(PostFilterModel model)
    {
        var authors = await _blogRepository.GetAuthorsAsync();

        var categories = await _blogRepository.GetCategoriesAsync();

        model.AuthorList = authors.Select(a => new SelectListItem()
        {
            Text = a.FullName,
            Value = a.Id.ToString()
        });
        model.CategoryList = categories.Select(c => new SelectListItem()
        {
            Text = c.Name,
            Value = c.Id.ToString()
        });
    }
    #endregion

    #region Edit
    [HttpGet]
    public async Task<IActionResult> Edit(int id = 0)
    {
        //ID= 0 (theem)
        //ID>0: doc du lieu cua bai viet tu csdl
        var post = id > 0
            ? await _blogRepository.GetPostByIdAsync(id, true)
            : null;
        //tao view model tu du lieu cua bai viet
       var model = post == null
           ? new PostEditModel()
           : _mapper.Map<PostEditModel>(post);

        await PopulatePostEditModelAsync(model);
        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> Edit(PostEditModel model)
    {
        var validationResult = await _postValidator.ValidateAsync(model);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
        }
        if (!ModelState.IsValid)
        {
            await PopulatePostEditModelAsync(model);
            return View(model);
        }
        var post = model.Id > 0
            ? await _blogRepository.GetPostByIdAsync(model.Id)
            : null;
        if (post == null)
        {
            post = _mapper.Map<Post>(model);
            post.Id = 0;
            post.PostedDate = DateTime.Now;
        }
        else
        {
            _mapper.Map(model, post);
            post.Category = null;
            post.ModifiedDate = DateTime.Now;

        }

        if (model.ImageFile?.Length>0)
        {
            var newImagePath = await _mediaManager.SaveFileAsync(
                model.ImageFile.OpenReadStream(),
                model.ImageFile.FileName,
                model.ImageFile.ContentType);
            if(!string.IsNullOrWhiteSpace(newImagePath))
            {
                await _mediaManager.DeleteFileAsync(post.ImageUrl);
                post.ImageUrl = newImagePath;
            }
        }
        await _blogRepository.CreateOrUpdatePostAsync(post, model.GetSelectedTags());

        return RedirectToAction(nameof(Index));
    }

    private async Task PopulatePostEditModelAsync(PostEditModel model)
    {
        var authors = await _blogRepository.GetAuthorsAsync();

        var categories = await _blogRepository.GetCategoriesAsync();

        model.AuthorList = authors.Select(a => new SelectListItem()
        {
            Text = a.FullName,
            Value = a.Id.ToString()
        });
        model.CategoryList = categories.Select(c => new SelectListItem()
        {
            Text = c.Name,
            Value = c.Id.ToString()
        });
    }

    [HttpPost]
    // kiểm trang xem url slug đã sử dụng bài viết khác hay chưa 
    public async Task<IActionResult> VerifyPostSlug(int id , string urlSlug)
    {
        var slugExisted = await _blogRepository
            .IsPostSlugExistedAsync(id, urlSlug);
        return slugExisted
            ? Json($"Slug '{urlSlug}' đã được sử dụng")
            : Json(true);
    }

    #endregion
}

