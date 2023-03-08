using Microsoft.EntityFrameworkCore;
using TatBlog.Data.Contexts;
using TatBlog.Data.Seeders;
using TatBlog.Services.Blogs;


var builder = WebApplication.CreateBuilder(args);
{
    // thêm các dịch vụ được yêu cầ bởi MVC Framework
    builder.Services.AddControllersWithViews();

    //đăng kí các dịch vụ với DI container
    builder.Services.AddDbContext<BlogDbContext>(options=> options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));
    
    builder.Services.AddScoped<IBlogRepository, BlogRepository>();
    builder.Services.AddScoped<IDataSeeder, DataSeeder>();
}

var app = builder.Build();
{
    // cấu hình http reques pipeline 
    // thêm middleware để hiển thị thông báo lỗi
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    else
    {
        app.UseExceptionHandler("/Blog/Error");
        // thêm middleware cho việc áp dụng hsts( thêm header 
        // strict-transport-security vào http response
        app.UseHsts();
    }
    // thêm middlineware để chuyển hướng HTTp sang HTTPS
    app.UseHttpsRedirection();

    // thêm middlineware phục hồi các yêu cầu liên quan tới các tập tin nội dung tĩnh như hình ảnh, css,...
    app.UseStaticFiles();
    // thêm middlineware lựa chon endpoint phù hợp nhất để xử lí một http request 
    app.UseRouting();
    // định nghĩa route template , router constraint cho các endpoints kết hợp với các action trong các controller
    app.MapControllerRoute(
        name: "default",
        pattern:"{controller=Blog}/{action=Index}/{id?}");
}

// thêm dữ liệu vào mẫu cơ sở dữ liệu
using (var scope = app.Services.CreateScope())
{
    var seeder= scope.ServiceProvider.GetRequiredService<IDataSeeder>();
    seeder.Initialize();
}

app.Run();
