var builder = WebApplication.CreateBuilder(args);
{
    // thêm các dịch vụ được yêu cầ bởi MVC Framework
    builder.Services.AddControllersWithViews();
}

var app = builder.Build();
{
    // cấu hình http reques pipeline 
    // theem middleware để hiển thị thông báo lỗi
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


app.Run();
