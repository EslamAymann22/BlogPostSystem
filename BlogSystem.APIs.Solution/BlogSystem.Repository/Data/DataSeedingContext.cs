using BlogSystem.Core.Entities;
using BlogSystem.Core.Entities.Identity;
//using BlogSystem.Repository.Identity;
using Microsoft.AspNetCore.Identity;
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
    public static class DataSeedingContext
    {

        public static async Task AddAsync(DbContextIdentity MyDbContext , UserManager<AppUser> _UserManager)
        {

            if (!_UserManager.Users.Any())
            {
                var UsersData = File.ReadAllText("../BlogSystem.Repository/Data/DataSeed/Users.json");
                var users = JsonSerializer.Deserialize<List<AppUser>>(UsersData);
                if (users?.Count() > 0)
                {
                    foreach (var User in users)
                    {
                        //await _UserManager.CreateAsync(User, "123");
                        await _UserManager.CreateAsync(User, "Pa$$w0rD");
                    }
                    //await dbContextIdentity.SaveChangesAsync();
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
                var Posts = JsonSerializer.Deserialize<List<Post>>(PostsData);
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
                        //Post.Author = await _UserManager.Users.Where(x => x.Id == Post.AuthorId).FirstOrDefaultAsync();
                        await MyDbContext.Set<Post>().AddAsync(Post);
                    }
                    await MyDbContext.SaveChangesAsync();
                }
            }
        }

    }
}
