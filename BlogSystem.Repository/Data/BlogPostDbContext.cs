using BlogSystem.Core.Entities;
using BlogSystem.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Repository.Data
{
    public class BlogPostDbContext : IdentityDbContext<AppUser>
    {
        public BlogPostDbContext(DbContextOptions<BlogPostDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Comment> comments { get; set; }
        public DbSet<Post> blogPosts { get; set; }
        public DbSet<Tag> tags { get; set; }
        public DbSet<Category> categories { get; set; }

    }
}
