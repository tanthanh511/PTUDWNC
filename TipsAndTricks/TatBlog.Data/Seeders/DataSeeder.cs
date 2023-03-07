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
            if (_dbContext.posts.Any()) return;
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
                 FullName= "Jason Mouth",
                 UrlSlug= "jason mouth",
                 Email= "json@gmail.com",
                 JoinedDate= new DateTime(2022,10,21),
                 ImageUrl="https://bom.so/lV6DiS",
                 Notes= " dangngocthanhg"

                },

                new()
                {
                 FullName= "Jessica Wonder",
                 UrlSlug= "jessica Wonder",
                 Email= "jessica665@motip.com",
                 JoinedDate= new DateTime(2020,4,19),
                 ImageUrl="https://bom.so/lV6DiS",
                 Notes="xinchao cac ban "
                }
            };
            _dbContext.Authors.AddRange(authors);
            _dbContext.SaveChanges();
            return authors;
        }
        private IList<Category> AddCategories() {
            var categories = new List<Category>()
            {
                new(){Name=".Net Core", Description=".Net Core", UrlSlug= ".dot net core", ShowOnMenu= true},
                new(){Name="OPP", Description="OOP", UrlSlug= "object oriented progam", ShowOnMenu= true},
                new(){Name="Design Patterns", Description="Design Patterns", UrlSlug= "design patterns", ShowOnMenu= true}
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
                new(){Name="BR", UrlSlug="BROWSER", Description="browser"}
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
                    Title="ASP.NET",
                    ShortDesciption="ASP. NET là một mã nguồn mở",
                    Description="ASP. NET là một mã nguồn mở dành cho web được tạo bởi Microsoft. Hiện mã nguồn này chạy trên nền tảng Windows và được bắt đầu vào đầu những năm 2000. ASP.NET cho phép các nhà phát triển tạo các ứng dụng web, dịch vụ web và các trang web động.",
                    Meta="Phiên bản ASP.NET đầu tiên được triển khai là 1.0 được ra mắt vào tháng 1 năm 2002 và hiện nay, phiên bản ASP.NET mới nhất là 4.6. ASP.NET được phát triển để tương thích với giao thức HTTP. Đó là giao thức chuẩn được sử dụng trên tất cả các ứng dụng web.",
                    UrlSlug="ASP.NET",
                    Publisded=true,
                    PostedDate= new DateTime(2021, 9, 30, 10, 20, 0),
                    ModifiedDate= null,
                    Viewcount=10,
                    Author = authors[0],
                    Category= categories[0],
                    Tags= new List<Tag>()
                    {
                        tags[0]
                    }

                },
                
                new()
                {
                    Title="EFCore",
                    ShortDesciption="EFCore là một mã nguồn mở",
                    Description="EFCore là một mã nguồn mở dành cho web được tạo bởi Microsoft. Hiện mã nguồn này chạy trên nền tảng Windows và được bắt đầu vào đầu những năm 2000. ASP.NET cho phép các nhà phát triển tạo các ứng dụng web, dịch vụ web và các trang web động.",
                    Meta="Phiên bản EFCore đầu tiên được triển khai là 1.0 được ra mắt vào tháng 1 năm 2002 và hiện nay, phiên bản ASP.NET mới nhất là 4.6. ASP.NET được phát triển để tương thích với giao thức HTTP. Đó là giao thức chuẩn được sử dụng trên tất cả các ứng dụng web.",
                    UrlSlug="EFCore",
                    Publisded=true,
                    PostedDate= new DateTime(2021, 10, 30, 10, 20, 0),
                    ModifiedDate= null,
                    Viewcount=30,
                    Author = authors[1],
                    Category= categories[1],
                    Tags= new List<Tag>()
                    {
                        tags[1]
                    }

                }
            };
            _dbContext.AddRange(posts);
            _dbContext.SaveChanges ();
            return posts;
        }

    
    }
}
