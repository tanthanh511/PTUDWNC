Console.WriteLine("Hello");

////See https://aka.ms/new-console-template for more information
//using System.Text;
//using TatBlog.Data.Contexts;
//using TatBlog.Data.Seeders;
//using TatBlog.Services.Blogs;
//using TatBlog.WinApp;
//using TatBlog.Core.Entities;
//using Microsoft.EntityFrameworkCore;

//#region"hàm tiếng việt"
//Console.OutputEncoding = Encoding.UTF8;
//#endregion

//#region"dấu gạch ngang"
//static async void Strikethrough(int a)
//{
//    Console.WriteLine("".PadRight(a, '-'));

//}
//#endregion

//#region"hàm menu"
//// MENU 
//int menu;
//Console.WriteLine("1.  xuất dữ liệu của bảng ");
//Console.WriteLine("2.  xuất dữ liệu của tác giả và tiêu đề ");
//Console.WriteLine("3.  xuất dữ liệu bài viết có người tim nhiều nhất.");
//Console.WriteLine("4.  xuất dữ liệu 9/2021 có slug là ASP.NET");
//Console.WriteLine("5.  tăng view");
//Console.WriteLine("6.  xuất danh sách chuyên mục và số lượng bài post.");
//Console.WriteLine("7.  lay danh sach tu khoa");
//Console.WriteLine("8.  xuất dữ liệu Tag slug là ASP.NET");
//Console.WriteLine("9.  xuất ra all tag và số bài viết.");
//Console.WriteLine("10. xóa tag có ID=1.");
//Console.WriteLine("11. xuất chuyên mục slug là ....");
//Console.WriteLine("12. tìm chuyên mục có ID:2 ");
//Console.WriteLine("13. thêm hoặc cập nhập một chuyên mục");
//Console.WriteLine("14. xóa một chuyên mục");
//Console.WriteLine("15. lấy và phân trang category");
//Console.WriteLine("0.  thoat ");
//Console.Write("bạn nhap tai đây: ");

//menu = Convert.ToInt32(Console.ReadLine());
//Console.WriteLine(" số bạn vừa5 nhập là:" + menu);
//await xuatMenu(menu);
//#endregion

//#region"điều kiện menu"
//if (menu < 0 || menu > 20)
//{

//    Console.WriteLine("vui lòng nhập lại!");
//    menu = Convert.ToInt32(Console.ReadLine());
//    await xuatMenu(menu);
//    //xuatMenu(menu);
//}
//// dùng bất đồng bộ thì phải dùng readkey hoặc dùng task để chờ lấy dữ liệu
////Console.ReadKey();
//#endregion

//#region"xuất menu"
//static async Task xuatMenu(int menu)
////static async void xuatMenu(int menu)
//{

//    #region"hàm dùng chung để tạo phiên làm việc với csdl"
//    // tạo context để quản lí phiên làm việc với  CSDL và trạng thái 
//    var context = new BlogDbContext();
//    //tạo đối tượng blogRepossitory
//    IBlogRepository blogRepo = new BlogRepository(context);
//    #endregion

//    switch (menu)
//    {
//        case 0:
//            #region
//            Console.WriteLine(" thoát khỏi chương trình ");
//            #endregion
//            break;
//        case 1:
//            #region"xuất dữ liệu tác giả"
//            Console.WriteLine("xuất dữ liệu tác giả:");
//            //  tạo đối tượng khởi tạo dữ liệu mẫu  
//            var seeder = new DataSeeder(context);

//            // nhập dữ liệu mẫu
//            seeder.Initialize();

//            // đọc danh sách từ csdl 
//            var authors = context.Authors.ToList();

//            //xuất danh sách tác giả ra màn hình 
//            Console.WriteLine("{0,-4}{1,-20}{2,-30}{3,-20}{4,-30}{5,-20}",
//                              "ID", "FullName", "Email", "joined date", "ImageUrl", "Notes");

//            foreach (var author in authors)
//            {
//                Console.WriteLine("{0, -4}{1, -20}{2,-30}{3,-20:MM/dd/yyyy}{4,-30}{5,-20}",
//                 author.Id, author.FullName, author.Email, author.JoinedDate, author.ImageUrl, author.Notes);
//                Strikethrough(120);
//            }
//            #endregion
//            break;
//        case 2:
//            #region"Xuất dữ liệu của tác giả và tiêu đề"
//            Console.WriteLine("Xuất dữ liệu của tác giả và tiêu đề:");
//            var posts = context.posts
//            .Where(p => p.Publisded)
//            .OrderBy(p => p.Title)
//            .Select(p => new
//            {
//                Id = p.Id,
//                Tiltle = p.Title,
//                Viewcount = p.Viewcount,
//                PostedDate = p.PostedDate,
//                Author = p.Author.FullName,
//                Category = p.Category.Name
//            })
//            .ToList();


//            foreach (var postByAuthor in posts)
//            {
//                Console.WriteLine("ID:          {0}", postByAuthor.Id);
//                Console.WriteLine("Title:       {0}", postByAuthor.Tiltle);
//                Console.WriteLine("View:        {0}", postByAuthor.Viewcount);
//                Console.WriteLine("Date:        {0:MM/dd/yyyy}", postByAuthor.PostedDate);
//                Console.WriteLine("Author:      {0}", postByAuthor.Author);
//                Console.WriteLine("Category:    {0}", postByAuthor.Category);
//                Strikethrough(120);
//            }
//            #endregion
//            break;
//        case 3:
//            #region"xuất dữ liệu bài viết có nhiều ng xem nhất"
//            Console.WriteLine(" xuất dữ liệu bài viết có nhiều ng xem nhất ");

//            var postsView = await blogRepo.GetPopularArticlesAsync(2);


//            foreach (var postP in postsView)
//            {
//                Console.WriteLine("ID      :{0}", postP.Id);
//                Console.WriteLine("Tiltel  :{0}", postP.Title);
//                Console.WriteLine("View    :{0}", postP.Viewcount);
//                Console.WriteLine("Date    :{0:MM/dd/yyyy}", postP.PostedDate);
//                Console.WriteLine("Author  :{0}", postP.Author.FullName);
//                Console.WriteLine("Category:{0}", postP.Category.Name);
//                Strikethrough(120);
//            }
//            #endregion
//            break;
//        case 4:
//            #region"xuất dữ liệu 9/2021 có slug là ASP.NET"
//            Console.WriteLine("xuất dữ liệu 9/2021 có slug là ASP.NET");
//            var post = await blogRepo.GetPostAsync(2021, 9, "ASP.NET");
//            Console.WriteLine("ID      :{0}", post.Id);
//            Console.WriteLine("Tiltel  :{0}", post.Title);
//            Console.WriteLine("View    :{0}", post.Viewcount);
//            Console.WriteLine("Date    :{0:MM/dd/yyyy}", post.PostedDate);
//            Console.WriteLine("Author  :{0}", post.Author.FullName);
//            Console.WriteLine("Category:{0}", post.Category.Name);
//            Strikethrough(120);
//            #endregion
//            break;
//        case 5:
//            #region"tang view cho mot bai viet."
//            Console.WriteLine(" tang view cho mot bai viet.");
//            Console.WriteLine("ID      :{0}", blogRepo.IncreaseViewCountAsync(1));
//            #endregion
//            break;
//        case 6:
//            #region"lấy danh sách chuyên mục va dem so luong"
//            Console.WriteLine("lấy danh sách chuyên mục va dem so luong");
//            var categories = await blogRepo.GetCategoriesAsync();
//            Console.WriteLine("{0,-5}{1,-50}{2,10}", "ID", "Name", "Count");
//            foreach (var category in categories)
//            {
//                Console.WriteLine("{0,-5}{1,-50}{2,10}", category.Id, category.Name, category.PostCount);

//            }
//            #endregion
//            break;
//        case 7:
//            #region"lay và phân trang tag"
//            Console.WriteLine("lay tu khoa");
//            var pagingParams = new PagingParams
//            {
//                PageNumber = 1, // lấy kết quả ở trang số 1 
//                PageSize = 4, //lấy 4 mẫu tin 
//                SortColumn = "Name", //sắp xếp theo tên 
//                SortOrder = "DESC" // theo chiều giảm dần 
//            };
//            // lấy danh sách từ khóa 
//            var tagsList = await blogRepo.GetPagedTagsAsync(pagingParams);

//            Console.WriteLine("{0,-5}{1,-50}{2,10}",
//                "ID", "Name", "Count");
//            foreach (var item in tagsList)
//            {
//                Console.WriteLine("{0,-5}{1,-50}{2,10}",
//                    item.Id, item.Name, item.PostCount);
//            }
//            #endregion
//            break;
//        case 8:
//            #region"xuất dữ liệu Tag slug là GOOGLE"
//            Console.WriteLine("xuất dữ liệu Tag slug là GOOGLE");
//            var tag = await blogRepo.GetTagAsync("GOOGLE");
//            Console.WriteLine("ID      :{0}", tag.Id);
//            Console.WriteLine("Name  :{0}", tag.Name);
//            Console.WriteLine("Slug    :{0}", tag.UrlSlug);
//            Console.WriteLine("Description:{0}", tag.Description);
//            Strikethrough(120);
//            #endregion
//            break;
//        case 9:
//            #region"xuất tất cả các tag"
//            Console.WriteLine("xuất tất cả các tag");
//            var tagItem = await blogRepo.GetAllByTagNumberAsync();
//            foreach (var all in tagItem)
//            {
//                Console.WriteLine("ID      :{0}", all.Id);
//                Console.WriteLine("Name  :{0}", all.Name);
//                Console.WriteLine("Slug    :{0}", all.UrlSlug);
//                Console.WriteLine("Description:{0}", all.Description);
//                Console.WriteLine("Post Count:{0}", all.PostCount);
//                Strikethrough(120);
//            }
//            #endregion
//            break;
//        case 10:
//            #region"Xóa tag có ID = 1"
//            Console.WriteLine("Xóa tag có ID = 1");
//            var DelTag = await blogRepo.TagDeleteByID(1);
//            #endregion
//            break;
//        case 11:
//            #region"xuất chuyên mục có định danh slug là .dot net core"
//            Console.WriteLine("xuất chuyên mục có định danh slug là .dot net core.");
//            var categoryBySlug = await blogRepo.GetCategorybySlugAsync(".dot net core");
//            Console.WriteLine("ID      :{0}", categoryBySlug.Id);
//            Console.WriteLine("Name  :{0}", categoryBySlug.Name);
//            Console.WriteLine("Slug    :{0}", categoryBySlug.UrlSlug);
//            Console.WriteLine("Description:{0}", categoryBySlug.Description);
//            Strikethrough(120);
//            #endregion
//            break;
//        case 12:
//            #region"tìm chuyên mục có ID là 2."
//            Console.WriteLine("tìm chuyên mục có ID là 2.");
//            var categoryById = await blogRepo.GetCategoryByID(2);
//            Console.WriteLine("ID      :{0}", categoryById.Id);
//            Console.WriteLine("Name  :{0}", categoryById.Name);
//            Console.WriteLine("Slug    :{0}", categoryById.UrlSlug);
//            Console.WriteLine("Description:{0}", categoryById.Description);
//            #endregion
//            break;
//        case 13:
//            #region"thêm hoặc sửa một chuyên đề"
//            Console.WriteLine("thêm hoặc sửa một chuyên đề nếu có");
//            Category addCategory = new()
//            {
//                Name = "JAVA",
//                Description = "java",
//                UrlSlug = ".net bean",
//                ShowOnMenu = true
//            };
//            await blogRepo.AddOrUpdateCategory(addCategory);
//            Console.Out.WriteLine("theem thanh cong");
//            Console.WriteLine("ID      :{0}", addCategory.Id);
//            Console.WriteLine("Name  :{0}", addCategory.Name);
//            Console.WriteLine("Slug    :{0}", addCategory.UrlSlug);
//            Console.WriteLine("Description:{0}", addCategory.Description);
//            #endregion
//            break;
//        case 14:
//            #region xóa một chuyên mục theo ID= 1 
//            Console.WriteLine("xóa một chuyên mục theo ID");
//            var DelCategory = await blogRepo.DeleteCategoryByID(1);
//            #endregion
//            break;
//        case 15:
//            #region lấy và phân trang category
//            await Console.Out.WriteLineAsync("lấy và phân trang category");
//            var pagingParamsCategory = new PagingParams
//            {
//                PageNumber = 1, // lấy kết quả ở trang số 1 
//                PageSize = 2, //lấy 2 mẫu tin 
//                SortColumn = "Name", //sắp xếp theo tên 
//                SortOrder = "DESC" // theo chiều giảm dần 
//            };
//            // lấy danh sách chuyên mục
//            var categoryList = await blogRepo.GetPagedCategoriesAsync(pagingParamsCategory);

//            Console.WriteLine("{0,-5}{1,-50}{2,10}",
//                "ID", "Name", "Count");
//            foreach (var item in categoryList)
//            {
//                Console.WriteLine("{0,-5}{1,-50}{2,10}",
//                    item.Id, item.Name, item.PostCount);
//            }
//            #endregion
//            break;
//        default:
//            break;

//    }
//}
//#endregion







