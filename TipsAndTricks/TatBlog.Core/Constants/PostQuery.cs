using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatBlog.Core.Constants
{
    public class PostQuery
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public int PostId { get; set; }
        public int TagId { get; set; }
        public int CategoryId { get; set; }
        public string CategorySlug { get; set; }
        public string TagSlug { get; set; }
        public string AuthorSlug { get; set; }
        public bool PublishedOnly { get; set; }
        public int PostedYear { get; set; }
        public int PostedMonth { get; set; }
        public string Keyword { get; set; }
        public DateTime PostedDate { get; set; }
    }
}
