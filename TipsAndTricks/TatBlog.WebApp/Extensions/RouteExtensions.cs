namespace TatBlog.WebApp.Extensions
{
    public static class RouteExtensions
    {
        public static IEndpointRouteBuilder UseBlogRoutes(
            this IEndpointRouteBuilder endpoints)
        {
            // blog.com/category/abc

            // category
            endpoints.MapControllerRoute(
                name: "posts-by-category",
                pattern: "blog/category/{slug}",
                defaults: new { controller = "Blog", action = "Category" });

            // author name
            endpoints.MapControllerRoute(
                name: "post-by-author",
                pattern: "blog/author/{slug}",
                defaults: new { controller = "Blog", action = "Author" });

            // tag
            endpoints.MapControllerRoute(
                name: "post-by-tag",
                pattern: "blog/tag/{slug}",
                defaults: new { controller = "Blog", action = "Tag" });

            // post year/month/day
            endpoints.MapControllerRoute(
                name: "single-post",
                pattern: "blog/post/{year:int}/{month:int}/{day:int}/{slug}",
                defaults: new { controller = "Blog", action = "Post" });

            // main
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Blog}/{action=Index}/{id?}");

            return endpoints;
        }
    }
}
