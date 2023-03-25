namespace TatBlog.WebApp.Extensions
{
    public static class RouteExtensions
    {
        public static IEndpointRouteBuilder UseBlogRoutes(
            this IEndpointRouteBuilder endpoints)
        {
         
            #region postList
            // category
            endpoints.MapControllerRoute(
                name: "posts-by-category",
                pattern: "blog/category/{slug}",
                defaults: new { controller = "Blog", action = "Category" });

            // author 
            endpoints.MapControllerRoute(
                name: "post-by-author",
                pattern: "blog/author/{slug}",
                defaults: new { controller = "Blog", action = "Author" });

            // tag
            endpoints.MapControllerRoute(
                name: "post-by-tag",
                pattern: "blog/tag/{slug}",
                defaults: new { controller = "Blog", action = "Tag" });


            #endregion

            // post year/month/day
            endpoints.MapControllerRoute(
                name: "single-post",
                pattern: "blog/post/{year:int}/{month:int}/{day:int}/{slug}",
                defaults: new { controller = "Blog", action = "Post" });

            // archives
            endpoints.MapControllerRoute(
                name: "archives-post",
                pattern: "blog/archives/{year:int}/{month:int}",
                defaults: new { controller = "Blog", action = "Archives" });

            //featured
            endpoints.MapControllerRoute(
                name: "featured-post",
                pattern: "blog/featuredposts/{slug}/{view}",
                defaults: new { controller = "Blog", action = "FeaturedPosts" });


            //Admin
            endpoints.MapControllerRoute(
                name: "admin-area",
                pattern: "admin/{controller=Dashboard}/{action=Index}/{id?}",
                defaults: new { area = "Admin"});

            // author admin     
            endpoints.MapControllerRoute(
                name: "author-area",
                pattern: "admin/{controller=Authors}/{action=Index}/{id?}",
                defaults: new { area = "Admin" });
            // categori admin  
            endpoints.MapControllerRoute(
               name: "category-area",
               pattern: "admin/{controller=Categories}/{action=Index}/{id?}",
               defaults: new { area = "Admin" });
            // post admin  
            endpoints.MapControllerRoute(
               name: "post-area",
               pattern: "admin/{controller=Posts}/{action=Index}/{id?}",
               defaults: new { area = "Admin" });
            // tag admin  
            endpoints.MapControllerRoute(
               name: "tag-area",
               pattern: "admin/{controller=Tags}/{action=Index}/{id?}",
               defaults: new { area = "Admin" });
            // comment admin  
            endpoints.MapControllerRoute(
               name: "comment-area",
               pattern: "admin/{controller=Comments}/{action=Index}/{id?}",
               defaults: new { area = "Admin" });

            endpoints.MapControllerRoute(
               name: "login-post",
               pattern: "blog/login",
               defaults: new { controller = "Blog", action = "Login" });



            // main
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Blog}/{action=Index}/{id?}");

            return endpoints;
        }
    }
}
