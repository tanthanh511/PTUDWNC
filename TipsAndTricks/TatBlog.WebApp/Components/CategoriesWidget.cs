﻿using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Components;

    public class CategoriesWidget : ViewComponent
    {
        private readonly IBlogRepository _blogRepository;

        public CategoriesWidget(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // lấy danh sách chủ đề
            var categories = await _blogRepository.GetCategoriesAsync();
            return View(categories);

       
        }
    }

