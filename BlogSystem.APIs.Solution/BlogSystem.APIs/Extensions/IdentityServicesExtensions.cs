using BlogSystem.Core.Entities.Identity;
using BlogSystem.Repository.Data;
using Microsoft.AspNetCore.Identity;

namespace BlogSystem.APIs.Extensions
{
    public static class IdentityServicesExtensions
    {

        public static IServiceCollection AddIdentityServices (this IServiceCollection Services)
        {
            Services.AddIdentity<AppUser, IdentityRole>()
                         .AddEntityFrameworkStores<DbContextIdentity>();

            Services.AddAuthentication();
            //services.AddAuthentication(Options =>
            //{
            //    Options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    Options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            return Services;
        }

    }
}
