using TatBlog.WebApi.Endpoints;
using TatBlog.WebApi.Extensions;
using TatBlog.WebApi.Mapsters;
using TatBlog.WebApi.Validations;

var builder = WebApplication.CreateBuilder(args);
{
    builder
        .ConfigureCors()
        .ConfigureNLog()
        .ConfigureServices()
        .ConfigureSwaggerOpenApi()
        .ConfigureMapster()
        .configureFluentValidation();

  
}

var app = builder.Build();
{
    app.SetupRequestPipeLine();
    app.MapAuthorEndpoints();
    app.MapCategoryEndpoints();
    app.MapPostEndpoints();
    app.Run();
}




