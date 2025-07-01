using BlogSystem.Core.Repositories;
using BlogSystem.Repository;

namespace BlogSystem.APIs.Extensions
{
    public static class ApplicationServicesExtensions
    {

        public static IServiceCollection AddApplicationServices(this IServiceCollection Services)
        {
            #region AskedCLR.ToInjectOpject

            Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            #endregion
            //builder.Services.AddAutoMapper(typeof(MappingProfiles));
            Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); //from // https://www.youtube.com/watch?v=87fhsf8gfDg&t=71s 

            return Services;
        }

    }
}
