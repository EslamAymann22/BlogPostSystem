using BlogSystem.APIs.Extensions;
using BlogSystem.Core.Entities.Identity;
using BlogSystem.Repository.Data;
using BlogSystem.Service;
using GalaxyApp.Core.MiddleWare;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace BlogSystem.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            #region Configure Service 
            // Add services to the container.
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddSwaggerGen(c =>
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Blog Post Api", Version = "V1" })
            );


            builder.Services.AddDbContext<BlogPostDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddServicesDependencies()
                   .AddIdentityServices(builder.Configuration);

            builder.Services.AddApplicationServices();

            #endregion


            var app = builder.Build();

            #region Update DataBase

            using var Scope = app.Services.CreateScope();
            var Services = Scope.ServiceProvider;
            var LoggerFactory = Services.GetRequiredService<ILoggerFactory>();

            try
            {
                var DbContext = Services.GetRequiredService<BlogPostDbContext>();

                await DbContext.Database.MigrateAsync();


                var _UserManager = Services.GetRequiredService<UserManager<AppUser>>();
                var _roleManager = Services.GetRequiredService<RoleManager<IdentityRole>>();

                await DataSeedingContext.AddAsync(DbContext, _UserManager, _roleManager);

            }
            catch (Exception ex)
            {

                var Logger = LoggerFactory.CreateLogger<Program>();
                Logger.LogError(ex, "Error During Update database in Program\n");

            }

            #endregion


            #region Configure - the HTTP Request pipeline 

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Zero is a index not value 
            //app.UseStatusCodePagesWithRedirects("/errors/{0}");
            //app.UseStatusCodePagesWithReExecute("/errors/{0}"); // Faster than Redirects

            app.UseMiddleware<ErrorHandlerMiddleware>();


            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            #endregion

            app.Run();
        }
    }
}