using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace TatBlog.WebApp.Areas.Admin.Models
{
    public class TagEditModel
    {
  
        public int Id { get; set; }

        ////////////////////////////////
        [DisplayName("Tên")]
        // [Required(ErrorMessage ="Tieu de khong duoc de trong")]
        //  [MaxLength(500,ErrorMessage ="Tieu de toi da 500 ky tu")]
        public string Name { get; set; }

        //////////////////////////////////////
        [DisplayName("Noi dung")]
        //[Required(ErrorMessage = "Noi dung khong duoc de trong")]
        //[MaxLength(5000, ErrorMessage = "Noi dung toi da 5000 ky tu")]
        public string Description { get; set; }


        /////////////////////////////////////////
        [DisplayName("Slug")]
        [Remote("VerifyPostSlug", "Tags", "Admin",
            HttpMethod = "TAG", AdditionalFields = "Id")]
        // [Required(ErrorMessage = "URL Slug khong duoc de trong")]
        // [MaxLength(200, ErrorMessage = "Slug toi da 200 ky tu")]
        public string UrlSlug { get; set; }

        ////////////////////////////////////////
        //[DisplayName("show on menu")]
        //public bool ShowOnMenu { get; set; }

      

        ////////////////////////////////////////////
        //[DisplayName("post(moi tu 1 dong)")]
        //// [Required(ErrorMessage ="ban chua nhap tu khoa")]
        //public string SelectedPosts { get; set; }


        //public List<string> GetSelectedTags()
        //{
        //    return (SelectedPosts ?? "")
        //        .Split(new[] { ',', ';', '\r', '\n' },
        //        StringSplitOptions.RemoveEmptyEntries)
        //        .ToList();
        //}
    }


}
