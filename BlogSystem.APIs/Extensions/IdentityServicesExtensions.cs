using BlogSystem.Core.Entities.Identity;
using BlogSystem.Core.Services;
using BlogSystem.Repository.Data;
using BlogSystem.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BlogSystem.APIs.Extensions
{
    public static class IdentityServicesExtensions
    {

        public static IServiceCollection AddIdentityServices (this IServiceCollection Services , IConfiguration _configuration)
        {

            Services.AddScoped<ITokenService, TokenService>();

            Services.AddIdentity<AppUser, IdentityRole>()
                         .AddEntityFrameworkStores<BlogPostDbContext>();

            Services.AddAuthentication(Options =>
            {
                Options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                Options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(Options =>
                {
                    Options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = _configuration["JWT:ValidIssuer"],
                        ValidateAudience = true,
                        ValidAudience = _configuration["JWT:ValidAudience"],
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]))
                    };
                }); // UserManager SignInManager RoleManager  
            //services.AddAuthentication(Options =>
            //{
            //    Options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    Options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            return Services;
        }

    }
}
