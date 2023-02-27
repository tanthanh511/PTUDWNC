﻿//See https://aka.ms/new-console-template for more information
using System.Text;
using TatBlog.Data.Contexts;
using TatBlog.Data.Seeders;
using TatBlog.Services.Blogs;


// hàm tiếng việt 
Console.OutputEncoding = Encoding.UTF8;

// MENU 
int menu;
Console.WriteLine("1. xuất dữ liệu của bảng ");
Console.WriteLine("2. xuất dữ liệu của tác giả và tiêu đề ");
Console.WriteLine("3. xuất dữ liệu bài viết có người tim nhiều nhất.");
Console.WriteLine("4. xuất dữ liệu 9/2021 có slug là ASP.NET");
Console.WriteLine("0. thoat ");
Console.Write("bạn nhap tai đây: ");

menu = Convert.ToInt32(Console.ReadLine());
Console.WriteLine(" số bạn vừa nhập là:" + menu);
await xuatMenu(menu);

if (menu < 0 || menu > 10)
{

    Console.WriteLine("vui lòng nhập lại!");
    menu = Convert.ToInt32(Console.ReadLine());
    await xuatMenu(menu);
    //xuatMenu(menu);
}
// dùng bất đồng bộ thì phải dùng readkey hoặc dùng task 
//Console.ReadKey();



static async Task xuatMenu(int menu)
//static async void xuatMenu(int menu)
{

    var context = new BlogDbContext();

    //tạo đối tượng blogRepossitory
    IBlogRepository blogRepo = new BlogRepository(context);
    // tim 2  bai viet xem doc nhiu


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








