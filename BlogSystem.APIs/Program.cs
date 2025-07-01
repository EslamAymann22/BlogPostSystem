using BlogSystem.APIs.Extensions;
using BlogSystem.APIs.Middlewares;
using BlogSystem.Core.Entities.Identity;
using BlogSystem.Repository.Data;
using BlogSystem.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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

                await DataSeedingContext.AddAsync(DbContext, _UserManager);

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
                app.UseMiddleware<ExceptionMiddleWare>();
                app.MapOpenApi();
            }

            // Zero is a index not value 
            //app.UseStatusCodePagesWithRedirects("/errors/{0}");
            app.UseStatusCodePagesWithReExecute("/errors/{0}"); // Faster than Redirects

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            #endregion

            app.Run();
        }
    }
}