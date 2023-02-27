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
    public class TagMap : IEntityTypeConfiguration<Tag>
    {
        // TẠO BẢNG TAG
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.ToTable("Tags");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(t => t.Description)
                .HasMaxLength(100);

            builder.Property(t => t.UrlSlug)
                .IsRequired()
                .HasMaxLength(50);

        }
    }
}
