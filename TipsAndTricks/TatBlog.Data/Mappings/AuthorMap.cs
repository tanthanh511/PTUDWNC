using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TatBlog.Core.Entities;


namespace TatBlog.Data.Mappings
{
    public class AuthorMap : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            // TẠO BẢNG 
            builder.ToTable("Authors");
            // trong bảng tạo khóa chính 
            builder.HasKey(x => x.Id);
            // và tạo các trường trường trong bảng
            builder.Property(a => a.FullName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(a => a.UrlSlug)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(a => a.ImageUrl)
                .HasMaxLength(500);

            builder.Property(a => a.Email)
                .HasMaxLength(100);

            builder.Property(a => a.JoinedDate)
                .HasColumnType("dateTime");

            builder.Property(a=>a.Notes)
                .HasMaxLength(500);

        }
    }
}
