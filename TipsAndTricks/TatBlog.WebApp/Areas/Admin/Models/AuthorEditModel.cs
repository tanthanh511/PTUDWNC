using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace TatBlog.WebApp.Areas.Admin.Models
{
    public class AuthorEditModel
    {

        public int Id { get; set; }

        ////////////////////////////////
        [DisplayName("Full name")]
        // [Required(ErrorMessage ="Tieu de khong duoc de trong")]
        //  [MaxLength(500,ErrorMessage ="Tieu de toi da 500 ky tu")]
        public string FullName { get; set; }

       /////////////////////////////////////////
        [DisplayName("Slug")]
        [Remote("VerifyPostSlug", "Author", "Admin",
            HttpMethod = "AUTHOR", AdditionalFields = "Id")]
        // [Required(ErrorMessage = "URL Slug khong duoc de trong")]
        // [MaxLength(200, ErrorMessage = "Slug toi da 200 ky tu")]
        public string UrlSlug { get; set; }

        //////////////////////////////////////////////
        /// </summary>
        [DisplayName("Email")]
        //[Required(ErrorMessage = "Noi dung khong duoc de trong")]
        //[MaxLength(5000, ErrorMessage = "Noi dung toi da 5000 ky tu")]
        public string Email { get; set; }


  

        ////////////////////////////////////////
        [DisplayName("Notes")]
        public string Notes { get; set; }



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
