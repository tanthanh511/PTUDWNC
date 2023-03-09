using System;
using Microsoft.EntityFrameworkCore;
using TatBlog.Core.Entities;
using TatBlog.Data.Mappings;

namespace TatBlog.Data.Contexts
{
    public class BlogDbContext : DbContext
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<Tag> tags { get; set; }
        public DbSet<Post> posts { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-62GFOD9;Initial Catalog=Tatblog;Integrated Security=True;MultipleActiveResultSets = true;TrustServerCertificate = True");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(CategoryMap).Assembly);
        }

        public BlogDbContext(DbContextOptions<BlogDbContext>options):base(options) 
        {
                   
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //   modelBuilder.ApplyConfigurationsFromAssembly(
        //       typeof(CategoryMap).Assembly );
        //}
    }
}
