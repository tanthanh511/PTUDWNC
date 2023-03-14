using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Data.Seeders;
using TatBlog.Data.Contexts;
using TatBlog.Core.Entities;

namespace TatBlog.Data.Seeders
{
    public class DataSeeder : IDataSeeder
    {
        // dbcontext: là cầu nối giữa lớp thực thể với lại CSDL 
        // để có thể (truy vấn, theo dõi thay đổi, dữ liệu bền vững, bộ nhớ đệm, quản lí mối quan hệ , ánh xạ đối tượng)
        private readonly BlogDbContext _dbContext;
        public DataSeeder (BlogDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Initialize()
        {
             _dbContext.Database.EnsureCreated();
            // cẩn thận dòng này bị lỗi
            if (_dbContext.Posts.Any()) return;
            var authors = AddAuthors();
            var categoris = AddCategories();
            var tags = AddTags();
            var posts = AddPosts(authors, categoris,tags);
          

        }

        private IList<Author> AddAuthors() {
            var authors = new List<Author>()
            {
                new()
                {
                 FullName= "DANG NGOC THANG",
                 UrlSlug= "dang-ngoc-thang",
                 Email= "thang@gmail.com",
                 JoinedDate= new DateTime(2022,10,21),
                 ImageUrl="",
                 Notes= " dangngocthang"

                },

                new()
                {
                 FullName= "DOAN CAO NHAT HA",
                 UrlSlug= "doan-cao-nhat-ha",
                 Email= "ha@gmail.com",
                 JoinedDate= new DateTime(2022,10,30),
                 ImageUrl="",
                 Notes= "doancaonhatha"

                },

                new()
                {
                 FullName= "BUI VAN DU",
                 UrlSlug= "bui-van-du",
                 Email= "du@gmail.com",
                 JoinedDate= new DateTime(2022,10,21),
                 ImageUrl="",
                 Notes= "buivandu"

                },

                new()
                {
                 FullName= "NGUYEN HUU TRONG VY",
                 UrlSlug= "nguyen-huu-trong-vy",
                 Email= "thang@gmail.com",
                 JoinedDate= new DateTime(2022,10,21),
                 ImageUrl="",
                 Notes= " dangngocthang"

                },

                new()
                {
                 FullName= "HUYNH TAN THANH",
                 UrlSlug= "huynh-tan-thanh",
                 Email= "thanh@gmail.com",
                 JoinedDate= new DateTime(2022,10,21),
                 ImageUrl="",
                 Notes= "huynhtanthanh"

                },

                new()
                {
                 FullName= "NGUYEN TUAN KIET",
                 UrlSlug= "nguyen-tuan-kiet",
                 Email= "kiet@motip.com",
                 JoinedDate= new DateTime(2020,4,19),
                 ImageUrl="",
                 Notes="nguyentuankiet "
                }
            };
            _dbContext.Authors.AddRange(authors);
            _dbContext.SaveChanges();
            return authors;
        }
        private IList<Category> AddCategories() {
            var categories = new List<Category>()
            {

                new(){Name=".Net Core", Description=".Net Core", UrlSlug= "dot-net-core", ShowOnMenu= true},
                new(){Name="OOP", Description="OOP", UrlSlug= "object-oriented-progam", ShowOnMenu= true},
                new(){Name="HTML", Description="HTML Tutorial", UrlSlug= "hyper-text-markup-language", ShowOnMenu= true},
                new(){Name="CSS", Description="CSS Tutorial", UrlSlug= "cascading-style-sheets", ShowOnMenu= true},
                new(){Name="JAVA", Description="JAVA Tutorial", UrlSlug= "java", ShowOnMenu= true},
                new(){Name="SQL", Description="SQL Tutorial", UrlSlug= "sql-server", ShowOnMenu= true},
                new(){Name="JAVASCRIPT", Description="JAVASCRIPT Tutorial", UrlSlug= "java-script", ShowOnMenu= true},
            };
            _dbContext.AddRange(categories);
            _dbContext.SaveChanges ();
            return categories;
        }
        private IList<Tag> AddTags() {
            var tags = new List<Tag>()
            {
                new(){Name="GG", UrlSlug="GOOGLE", Description="google"},
                new(){Name="CC", UrlSlug="COCCOC", Description="coccoc"},
                new(){Name="FF", UrlSlug="FIREFOX", Description="firefox"},
                new(){Name="JAVA", UrlSlug="JAVA", Description="java"},
                new(){Name="NC", UrlSlug="NETCORE", Description="dotnetcore"},
                new(){Name="OOP", UrlSlug="OBJECT-ORIENTED-PROGRAM", Description="object oriented progam la lap trinh huong doi tuong"},
                new(){Name="HTML", UrlSlug="HYPER-TEXT-MARKUP-LANGUAGE", Description="hyper text markup language file tinh"},
                new(){Name="CSS", UrlSlug="CACSCADING-STYLE-SHEETS", Description="cascading style sheets them mau cho file html tinh"},
                new(){Name="SQL", UrlSlug="SQL-SERVER", Description="sql server la co so du lieu"},
                new(){Name="JAVASCRIPT", UrlSlug="JAVASCRIPT", Description="javascript la mot ngon ngu"}
            };
            _dbContext.AddRange (tags);
            _dbContext.SaveChanges () ;
            return tags;
        }

        private IList<Post> AddPosts(
            IList<Author> authors ,
            IList<Category>categories,
            IList<Tag> tags) {
            var posts = new List<Post>()
            {
                new()
                {
                    Title="ASP.NETt",
                    ShortDesciption="ASP. NET là một mã nguồn mở",
                    Description="ASP. NET là một mã nguồn mở dành cho web được tạo bởi Microsoft. Hiện mã nguồn này chạy trên nền tảng Windows và được bắt đầu vào đầu những năm 2000. ASP.NET cho phép các nhà phát triển tạo các ứng dụng web, dịch vụ web và các trang web động.",
                    Meta="Phiên bản ASP.NET đầu tiên được triển khai là 1.0 được ra mắt vào tháng 1 năm 2002 và hiện nay, phiên bản ASP.NET mới nhất là 4.6. ASP.NET được phát triển để tương thích với giao thức HTTP. Đó là giao thức chuẩn được sử dụng trên tất cả các ứng dụng web.",
                    UrlSlug="ASP.NEttT",
                    Publisded=true,
                    PostedDate= new DateTime(2021, 1, 1, 10, 20, 0),
                    ModifiedDate= null,
                    Viewcount=1110,
                    Author = authors[0],
                    Category= categories[0],
                    Tags= new List<Tag>()
                    {
                        tags[0]
                    }

                },
                
                new()
                {
                    Title="EFCoree",
                    ShortDesciption="EFCore là một mã nguồn mở",
                    Description="EFCore là một mã nguồn mở dành cho web được tạo bởi Microsoft. Hiện mã nguồn này chạy trên nền tảng Windows và được bắt đầu vào đầu những năm 2000. ASP.NET cho phép các nhà phát triển tạo các ứng dụng web, dịch vụ web và các trang web động.",
                    Meta="Phiên bản EFCore đầu tiên được triển khai là 1.0 được ra mắt vào tháng 1 năm 2002 và hiện nay, phiên bản ASP.NET mới nhất là 4.6. ASP.NET được phát triển để tương thích với giao thức HTTP. Đó là giao thức chuẩn được sử dụng trên tất cả các ứng dụng web.",
                    UrlSlug="EFCoreee",
                    Publisded=true,
                    PostedDate= new DateTime(2021, 3, 1, 10, 20, 0),
                    ModifiedDate= null,
                    Viewcount=50,
                    Author = authors[1],
                    Category= categories[1],
                    Tags= new List<Tag>()
                    {
                        tags[1]
                    }

                },
                 new()
                {
                    Title="OOPl",
                    ShortDesciption="OOP là lap trinh huong doi tuong",
                    Description="Lập trình hướng đối tượng (tiếng Anh: Object-oriented programming - OOP) là một mẫu hình lập trình dựa trên khái niệm \"đối tượng\", mà trong đó, đối tượng chứa đựng các dữ liệu, trên các trường, thường được gọi là các thuộc tính; và mã nguồn, được tổ chức thành các phương thức.",
                    Meta="OOP chuan",
                    UrlSlug="OOPpp",
                    Publisded=true,
                    PostedDate= new DateTime(2021, 10, 10, 10, 20, 0),
                    ModifiedDate= null,
                    Viewcount=1000,
                    Author = authors[2],
                    Category= categories[1],
                    Tags= new List<Tag>()
                    {
                        tags[5]
                    }

                },

                 new()
                {
                    Title="HTMLl",
                    ShortDesciption="HTML là một file tinh",
                    Description="HTML (viết tắt của từ HyperText Markup Language, hay là \"Ngôn ngữ Đánh dấu Siêu văn bản\") là một ngôn ngữ đánh dấu được thiết kế ra để tạo nên các trang web trên World Wide Web. Nó có thể được trợ giúp bởi các công nghệ như CSS và các ngôn ngữ kịch bản giống như JavaScript.",
                    Meta="HTML3,HTML4,HTML5",
                    UrlSlug="HTMLll",
                    Publisded=true,
                    PostedDate= new DateTime(2021, 10, 1, 10, 20, 0),
                    ModifiedDate= null,
                    Viewcount=30,
                    Author = authors[3],
                    Category= categories[2],
                    Tags= new List<Tag>()
                    {
                        tags[6]
                    }

                },
                new()
                {
                    Title="CSSs",
                    ShortDesciption="CSS là một file tinh",
                    Description="Trong tin học, các tập tin định kiểu theo tầng – dịch từ tiếng Anh là Cascading Style Sheets (CSS) – được dùng để miêu tả cách trình bày các tài liệu viết bằng ngôn ngữ HTML và XHTML.[1] Ngoài ra ngôn ngữ định kiểu theo tầng cũng có thể dùng cho XML, SVG, XUL. Các đặc điểm kỹ thuật của CSS được duy trì bởi World Wide Web Consortium (W3C). Thay vì đặt các thẻ quy định kiểu dáng cho văn bản HTML (hoặc XHTML) ngay trong nội dung của nó, bạn nên sử dụng CSS.",
                    Meta="CSS1,CSS2,CSS3",
                    UrlSlug="CSSss",
                    Publisded=true,
                    PostedDate= new DateTime(2021, 5, 1, 10, 20, 0),
                    ModifiedDate= null,
                    Viewcount=70,
                    Author = authors[4],
                    Category= categories[3],
                    Tags= new List<Tag>()
                    {
                        tags[7]
                    }

                },

                new()
                {
                    Title="SQLl",
                    ShortDesciption="SQL là một file tinh",
                    Description="Trong tin học, các tập tin định kiểu theo tầng – dịch từ tiếng Anh là Cascading Style Sheets (CSS) – được dùng để miêu tả cách trình bày các tài liệu viết bằng ngôn ngữ HTML và XHTML.[1] Ngoài ra ngôn ngữ định kiểu theo tầng cũng có thể dùng cho XML, SVG, XUL. Các đặc điểm kỹ thuật của CSS được duy trì bởi World Wide Web Consortium (W3C). Thay vì đặt các thẻ quy định kiểu dáng cho văn bản HTML (hoặc XHTML) ngay trong nội dung của nó, bạn nên sử dụng CSS.",
                    Meta="SQL1,SQL2,SQL3",
                    UrlSlug="SQLll",
                    Publisded=true,
                    PostedDate= new DateTime(2021, 1, 2, 10, 20, 0),
                    ModifiedDate= null,
                    Viewcount=50,
                    Author = authors[5],
                    Category= categories[5],
                    Tags= new List<Tag>()
                    {
                        tags[8]
                    }

                },

                new()
                {
                    Title="JAVASCRIPT",
                    ShortDesciption="JAVASCRIPT là một file tinh",
                    Description="Trong tin học, các tập tin định kiểu theo tầng – dịch từ tiếng Anh là Cascading Style Sheets (CSS) – được dùng để miêu tả cách trình bày các tài liệu viết bằng ngôn ngữ HTML và XHTML.[1] Ngoài ra ngôn ngữ định kiểu theo tầng cũng có thể dùng cho XML, SVG, XUL. Các đặc điểm kỹ thuật của CSS được duy trì bởi World Wide Web Consortium (W3C). Thay vì đặt các thẻ quy định kiểu dáng cho văn bản HTML (hoặc XHTML) ngay trong nội dung của nó, bạn nên sử dụng CSS.",
                    Meta="JAVASCRIPT1,JAVASCRIPT2,JAVASCRIPT3",
                    UrlSlug="JAVASCRIPttT",
                    Publisded=true,
                    PostedDate= new DateTime(2021, 4, 3, 10, 20, 0),
                    ModifiedDate= null,
                    Viewcount=10,
                    Author = authors[5],
                    Category= categories[6],
                    Tags= new List<Tag>()
                    {
                        tags[9]
                    }

                },

                    new()
                {
                    Title="JAVA",
                    ShortDesciption="JAVA là một file tinh",
                    Description="Trong tin học, các tập tin định kiểu theo tầng – dịch từ tiếng Anh là Cascading Style Sheets (CSS) – được dùng để miêu tả cách trình bày các tài liệu viết bằng ngôn ngữ HTML và XHTML.[1] Ngoài ra ngôn ngữ định kiểu theo tầng cũng có thể dùng cho XML, SVG, XUL. Các đặc điểm kỹ thuật của CSS được duy trì bởi World Wide Web Consortium (W3C). Thay vì đặt các thẻ quy định kiểu dáng cho văn bản HTML (hoặc XHTML) ngay trong nội dung của nó, bạn nên sử dụng CSS.",
                    Meta="JAVA1,JAVA2,JAVAS3",
                    UrlSlug="JAVAtt",
                    Publisded=true,
                    PostedDate= new DateTime(2021, 10, 12, 10, 20, 0),
                    ModifiedDate= null,
                    Viewcount=40,
                    Author = authors[5],
                    Category= categories[4],
                    Tags= new List<Tag>()
                    {
                        tags[3]
                    }

                }
            };
            _dbContext.Posts.AddRange(posts);
            _dbContext.SaveChanges ();
            return posts;
        }

    
    }
}
