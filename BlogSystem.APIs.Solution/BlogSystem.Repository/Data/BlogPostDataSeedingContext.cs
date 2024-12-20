using BlogSystem.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlogSystem.Repository.Data
{
    public static class BlogPostDataSeedingContext
    {

        public static async Task AddAsync(BlogPostDbContext MyDbContext)
        {

            if (!MyDbContext.users.Any())
            {
                var UsersData = File.ReadAllText("../BlogSystem.Repository/Data/DataSeed/Users.json");
                var Users = JsonSerializer.Deserialize<List<User>>(UsersData);
                if (Users?.Count() > 0)
                {
                    foreach (var User in Users)
                    {
                        await MyDbContext.Set<User>().AddAsync(User);
                    }
                    await MyDbContext.SaveChangesAsync();
                }
            }

            if (!MyDbContext.tags.Any())
            {
                var TagsData = File.ReadAllText("../BlogSystem.Repository/Data/DataSeed/Tags.json");
                var Tags = JsonSerializer.Deserialize<List<Tag>>(TagsData);
                if (Tags?.Count() > 0)
                {
                    foreach (var Tag in Tags)
                    {
                        await MyDbContext.Set<Tag>().AddAsync(Tag);
                    }
                    await MyDbContext.SaveChangesAsync();
                }
            }

            if (!MyDbContext.categories.Any())
            {
                var CategoriesData = File.ReadAllText("../BlogSystem.Repository/Data/DataSeed/Categories.json");
                var Categories = JsonSerializer.Deserialize<List<Category>>(CategoriesData);
                if (Categories?.Count() > 0)
                {
                    foreach (var Categoty in Categories)
                    {
                        await MyDbContext.Set<Category>().AddAsync(Categoty);
                    }
                    await MyDbContext.SaveChangesAsync();
                }
            }

            if (!MyDbContext.blogPosts.Any())
            {
                var PostsData = File.ReadAllText("../BlogSystem.Repository/Data/DataSeed/Posts.json");
                var Posts = JsonSerializer.Deserialize<List<BlogPost>>(PostsData);
                if (Posts?.Count() > 0)
                {
                    foreach (var Post in Posts)
                    {
                        var TagsId = Post.Tags.Select(T => T.Id).ToList();

                        var PostTags = await MyDbContext.Set<Tag>()
                            .Where(T => TagsId.Contains(T.Id))
                            .ToListAsync();

                        //var PostTags = new List<Tag>();
                        //foreach (var Tag in TagsId)
                        //{
                        //    var tag = await MyDbContext.tags.Where(T => T.Id == Tag).FirstOrDefaultAsync();
                        //    if (tag is not null)
                        //        PostTags.Add(tag);
                        //}

                        Post.Tags = PostTags;
                        await MyDbContext.Set<BlogPost>().AddAsync(Post);
                    }
                    await MyDbContext.SaveChangesAsync();
                }
            }
        }

    }
}
