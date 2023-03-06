//See https://aka.ms/new-console-template for more information
using System.Text;
using TatBlog.Data.Contexts;
using TatBlog.Data.Seeders;
using TatBlog.Services.Blogs;
using TatBlog.WinApp;
using TatBlog.Core.Entities;



// hàm tiếng việt 
Console.OutputEncoding = Encoding.UTF8;

// MENU 
int menu;
Console.WriteLine("1. xuất dữ liệu của bảng ");
Console.WriteLine("2. xuất dữ liệu của tác giả và tiêu đề ");
Console.WriteLine("3. xuất dữ liệu bài viết có người tim nhiều nhất.");
Console.WriteLine("4. xuất dữ liệu 9/2021 có slug là ASP.NET");
Console.WriteLine("5. tăng view");
Console.WriteLine("6. xuất danh sách chuyên mục và số lượng bài post.");
Console.WriteLine("7. lay danh sach tu khoa");
Console.WriteLine("8. xuất dữ liệu Tag slug là ASP.NET");
Console.WriteLine("9. xuất ra all tag và số bài viết.");
Console.WriteLine("10. xóa tag có ID=1.");
Console.WriteLine("0. thoat ");
Console.Write("bạn nhap tai đây: ");

menu = Convert.ToInt32(Console.ReadLine());
Console.WriteLine(" số bạn vừa5 nhập là:" + menu);
await xuatMenu(menu);

if (menu < 0 || menu > 10)
{

    Console.WriteLine("vui lòng nhập lại!");
    menu = Convert.ToInt32(Console.ReadLine());
    await xuatMenu(menu);
    //xuatMenu(menu);
}
// dùng bất đồng bộ thì phải dùng readkey hoặc dùng task để chờ lấy dữ liệu
//Console.ReadKey();



static async Task xuatMenu(int menu)
//static async void xuatMenu(int menu)
{
    var context = new BlogDbContext();

    //tạo đối tượng blogRepossitory
    IBlogRepository blogRepo = new BlogRepository(context);

    switch (menu)
    {
        case 0:
            Console.WriteLine(" thoát khỏi chương trình ");
            break;
        case 1:
            Console.WriteLine("xuất dữ liệu của bảng:");
            ReadData();
            break;
        case 2:
            Console.WriteLine("Xuất dữ liệu của tác giả và tiêu đề:");
            AuthorAndTiltle();
            break;
        case 3:
            Console.WriteLine(" xuất dữ liệu bài viết có nhiều ng xem nhất ");

            var posts = await blogRepo.GetPopularArticlesAsync(2);


            foreach (var postP in posts)
            {
                Console.WriteLine("ID      :{0}", postP.Id);
                Console.WriteLine("Tiltel  :{0}", postP.Title);
                Console.WriteLine("View    :{0}", postP.Viewcount);
                Console.WriteLine("Date    :{0:MM/dd/yyyy}", postP.PostedDate);
                Console.WriteLine("Author  :{0}", postP.Author.FullName);
                Console.WriteLine("Category:{0}", postP.Category.Name);
                Strikethrough(120);
            }
            break;
        case 4:
            Console.WriteLine("xuất dữ liệu 9/2021 có slug là ASP.NET");
            var post = await blogRepo.GetPostAsync(2021, 9, "ASP.NET");
            Console.WriteLine("ID      :{0}", post.Id);
            Console.WriteLine("Tiltel  :{0}", post.Title);
            Console.WriteLine("View    :{0}", post.Viewcount);
            Console.WriteLine("Date    :{0:MM/dd/yyyy}", post.PostedDate);
            Console.WriteLine("Author  :{0}", post.Author.FullName);
            Console.WriteLine("Category:{0}", post.Category.Name);
            Strikethrough(120);
            break;
        case 5:
            Console.WriteLine(" tang view cho mot bai viet.");
            // var postV = await blogRepo.IncreaseViewCountAsync();
            Console.WriteLine("ID      :{0}", blogRepo.IncreaseViewCountAsync(1));
            //Console.WriteLine("Tiltel  :{0}", postV.Title);
            //Console.WriteLine("View    :{0}", postV.Viewcount);
            //Console.WriteLine("Date    :{0:MM/dd/yyyy}", postV.PostedDate);
            //Console.WriteLine("Author  :{0}", postV.Author.FullName);
            //Console.WriteLine("Category:{0}", postV.Category.Name);
            break;
        case 6:
            Console.WriteLine("lấy danh sách chuyên mục va dem so luong");
            var categories = await blogRepo.GetCategoriesAsync();
            Console.WriteLine("{0,-5}{1,-50}{2,10}", "ID", "Name", "Count");
            foreach (var category in categories)
            {
                Console.WriteLine("{0,-5}{1,-50}{2,10}", category.Id, category.Name, category.PostCount);

            }
            break;
        case 7:
            Console.WriteLine("lay tu khoa");
            var pagingParams = new PagingParams
            {
                PageNumber = 1, // lấy kết quả ở trang số 1 
                PageSize = 4, //lấy 4 mẫu tin 
                SortColumn = "Name", //sắp xếp theo tên 
                SortOrder = "DESC" // theo chiều giảm dần 
            };
            // lấy danh sách từ khóa 
            var tagsList = await blogRepo.GetPagedTagsAsync(pagingParams);

            Console.WriteLine("{0,-5}{1,-50}{2,10}",
                "ID", "Name", "Count");
            foreach (var item in tagsList)
            {
                Console.WriteLine("{0,-5}{1,-50}{2,10}",
                    item.Id, item.Name, item.PostCount);
            }
            break;

        case 8:
            Console.WriteLine("xuất dữ liệu Tag slug là GOOGLE");
            var tag = await blogRepo.GetTagAsync("GOOGLE");
            Console.WriteLine("ID      :{0}", tag.Id);
            Console.WriteLine("Name  :{0}", tag.Name);
            Console.WriteLine("Slug    :{0}", tag.UrlSlug);
            Console.WriteLine("Description:{0}", tag.Description);
            Strikethrough(120);
            break;

        case 9:
            Console.WriteLine("asdasdsadasd");
            var tagItem = await blogRepo.GetAllByTagNumberAsync();
            foreach (var all in tagItem)
            {
                Console.WriteLine("ID      :{0}", all.Id);
                Console.WriteLine("Name  :{0}", all.Name);
                Console.WriteLine("Slug    :{0}", all.UrlSlug);
                Console.WriteLine("Description:{0}", all.Description);
                Console.WriteLine("Post Count:{0}", all.PostCount);
                Strikethrough(120);
            }
            break;
        case 10:
            Console.WriteLine("Xóa tag có ID = 1");
            var DelTag = await blogRepo.TagDeleteByID(1);

            break;

        default:

            break;

    }
}




// hàm dấu gạch ngang 
static async void Strikethrough(int a)
{
    Console.WriteLine("".PadRight(a, '-'));

}

// hàm đọc dl tác giả 
static void ReadData()
{
    // tạo context để quản lí phiên làm việc với  CSDL và trạng thái 
    var context = new BlogDbContext();

    //  tạo đối tượng khởi tạo dữ liệu mẫu  
    var seeder = new DataSeeder(context);

    // nhập dữ liệu mẫu
    seeder.Initialize();

    // đọc danh sách từ csdl 
    var authors = context.Authors.ToList();

    //xuất danh sách tác giả ra màn hình 
    Console.WriteLine("{0,-4}{1,-20}{2,-30}{3,-20}{4,-30}{5,-20}",
                      "ID", "FullName", "Email", "joined date", "ImageUrl", "Notes");

    foreach (var author in authors)
    {
        Console.WriteLine("{0, -4}{1, -20}{2,-30}{3,-20:MM/dd/yyyy}{4,-30}{5,-20}",
         author.Id, author.FullName, author.Email, author.JoinedDate, author.ImageUrl, author.Notes);
        Strikethrough(120);
    }

}


static void AuthorAndTiltle()
{
    // tạo context để quản lí phiên làm việc với  CSDL và trạng thái 
    var context = new BlogDbContext();

    var posts = context.posts
    .Where(p => p.Publisded)
    .OrderBy(p => p.Title)
    .Select(p => new
    {
        Id = p.Id,
        Tiltle = p.Title,
        Viewcount = p.Viewcount,
        PostedDate = p.PostedDate,
        Author = p.Author.FullName,
        Category = p.Category.Name
    })
    .ToList();


    foreach (var post in posts)
    {
        Console.WriteLine("ID:          {0}", post.Id);
        Console.WriteLine("Title:       {0}", post.Tiltle);
        Console.WriteLine("View:        {0}", post.Viewcount);
        Console.WriteLine("Date:        {0:MM/dd/yyyy}", post.PostedDate);
        Console.WriteLine("Author:      {0}", post.Author);
        Console.WriteLine("Category:    {0}", post.Category);
        Strikethrough(120);
    }
}

//Task NewMethod(IBlogRepository blogRepo)
//{
//    return blogRepo.IncreaseViewCountAsync(1);

