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
    public class BlogPostConfig : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {

            builder.HasOne(P => P.Author)
                .WithMany()
                .HasForeignKey(P => P.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(P => P.Category)
                .WithMany()
                .HasForeignKey(P => P.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);


            builder.HasMany(P => P.Tags)
                //.WithMany(T=>T.Posts);
                .WithMany();


            builder.HasKey(P=>P.Id);

        }
    }
}
