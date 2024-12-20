using BlogSystem.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Repository.Data.Config
{
    public class BlogPostConfig : IEntityTypeConfiguration<BlogPost>
    {
        public void Configure(EntityTypeBuilder<BlogPost> builder)
        {
            builder.HasMany(P => P.Tags)
                .WithOne()
                .OnDelete(DeleteBehavior.SetNull);

            builder
            .HasOne(P => P.category)
                .WithMany()
                .HasForeignKey(P => P.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasKey(P=>P.Id);

        }
    }
}
