using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace TatBlog.WebApi.Models
{
    public class PostEditModel
    {
        public int Id { get; set; }

        ////////////////////////////////
        [DisplayName("Tieu de")]
        [Required(ErrorMessage ="Tieu de khong duoc de trong")]
        [MaxLength(500,ErrorMessage ="Tieu de toi da 500 ky tu")]
        public string Title { get; set; }

        ////////////////////////////////////
        [DisplayName("Gioi thieu")]
        [Required]
        
        public string ShortDesciption { get; set; }

        //////////////////////////////////////
        [DisplayName("Noi dung")]
        [Required]
        public string Description { get; set; }

        /////////////////////////////////////////
        [DisplayName("Metadata")]
        [Required]
        public string Meta { get; set; }   

        ////////////////////////////////////////
        [DisplayName("Chon hinh anh")]
        public IFormFile ImageFile { get; set; }

        ////////////////////////////////////////
        [DisplayName("Hinh hien tai")]
        public string ImageUrl { get; set; }

        ////////////////////////////////////////
        [DisplayName("Xuat ban ngay")]
        public bool Published { get; set; }

        ////////////////////////////////////////
        [DisplayName(" Chủ đề")]
        // [Required(ErrorMessage ="Bạn chưa chọn chủ đề")]
        public int CategoryId { get; set; }

        //////////////////////////////////////////
        [DisplayName(" Tác Giả")]
        //[Required(ErrorMessage = "Bạn chưa chọn tác giả")]
        public int AuthorId { get; set; }

        //////////////////////////////////////////
        [DisplayName("tu khoa(moi tu 1 dong)")]
        // [Required(ErrorMessage ="ban chua nhap tu khoa")]
        public string SelectedTags { get; set; }

        public IEnumerable<SelectListItem> AuthorList { get; set; }
        public IEnumerable<SelectListItem> CategoryList { get; set; }

        public List<string> GetSelectedTags()
        {
            return (SelectedTags ?? "")
                .Split(new[] { ',', ';', '\r', '\n' },
                StringSplitOptions.RemoveEmptyEntries)
                .ToList();
        }

     

        public static async ValueTask<PostEditModel> BindAsync (
            HttpContext context)
        {
            var form = await context.Request.ReadFormAsync();
            return new PostEditModel()
            {
                ImageFile = form.Files["ImageFile"],
                Id = int.Parse(form["Id"]),
                Title = form["Title"],
                ShortDesciption = form["ShortDesciption"],
                Description = form["Description"],
                Meta = form["Meta"],
                Published = bool.Parse(form["Published"]),
                CategoryId = int.Parse(form["Id"]),
                AuthorId = int.Parse(form["Id"]),
                SelectedTags = form["SelectedTags"]

            };
        }


    }
}
