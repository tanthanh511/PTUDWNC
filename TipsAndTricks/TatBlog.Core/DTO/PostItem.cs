using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatBlog.Core.DTO
{
    public class PostItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortDesciption { get; set; }
        public string Description { get; set; }
        public string Meta { get; set; }
        public string UrlSlug { get; set; }
        public string ImageUrl { get; set; }
        public int Viewcount { get; set; }
        public bool Publisded { get; set; }
        public string Tags { get; set; }
        public string CategoryName { get; set; }
        public int PostedYear { get; set; }
        public int PostedMonth { get; set; }
       // public string TagUrl { get; set; }
        public int PostCount { get; set; }
    }
}
