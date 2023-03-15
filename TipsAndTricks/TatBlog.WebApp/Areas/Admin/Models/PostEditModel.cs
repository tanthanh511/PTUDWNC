using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TatBlog.WebApp.Areas.Admin.Models;

public class PostEditModel
{
    public int Id { get; set; }

    ////////////////////////////////
    [DisplayName("Tieu de")]
   // [Required(ErrorMessage ="Tieu de khong duoc de trong")]
  //  [MaxLength(500,ErrorMessage ="Tieu de toi da 500 ky tu")]
    public string Title { get; set; }

    ////////////////////////////////////
    [DisplayName("Gioi thieu")]
   // [Required(ErrorMessage ="Gioi thieu khong duoc de trong")]
   // [MaxLength(2000, ErrorMessage ="Gioi thieu toi da 2000 ky tu")]
    public string ShortDesciption { get; set; }

    //////////////////////////////////////
    [DisplayName("Noi dung")]
    //[Required(ErrorMessage = "Noi dung khong duoc de trong")]
    //[MaxLength(5000, ErrorMessage = "Noi dung toi da 5000 ky tu")]
    public string Description { get; set; }

    /////////////////////////////////////////
    [DisplayName("Metadata")]
   // [Required(ErrorMessage = "Metadata khong duoc de trong")]
   // [MaxLength(1000, ErrorMessage = "Metadata toi da 1000 ky tu")]
    public string Meta { get; set; }

    /////////////////////////////////////////
    [DisplayName("Slug")]
    [Remote("VerifyPostSlug", "Posts", "Admin",
        HttpMethod = "POST", AdditionalFields = "Id")]
   // [Required(ErrorMessage = "URL Slug khong duoc de trong")]
   // [MaxLength(200, ErrorMessage = "Slug toi da 200 ky tu")]
    public string UrlSlug { get; set; }

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
            .Split(new[] {',',';','\r','\n'},
            StringSplitOptions.RemoveEmptyEntries)
            .ToList();
    }
}

