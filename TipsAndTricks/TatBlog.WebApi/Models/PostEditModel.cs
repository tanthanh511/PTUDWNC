namespace TatBlog.WebApi.Models
{
    public class PostEditModel
    {
        public string Tags { get; set; }
        public string CategoryName { get; set; }

        public int PostedYear { get; set; }
        public int PostedMonth { get; set; }
        public string UrlSlug { get; set; }
        // public bool ShowOnMenu { get; set; }
    }
}
