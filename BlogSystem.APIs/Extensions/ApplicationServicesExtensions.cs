using BlogSystem.APIs.Errors;
using BlogSystem.Core.Repositories;
using BlogSystem.Repository;
using Microsoft.AspNetCore.Mvc;

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

            Services.Configure<ApiBehaviorOptions>(options =>   // For Validation Errors 
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var ValidationErrorResponse = new ApiValidationErrorResponse()
                    {
                        errors = actionContext.ModelState.Values.Where(V => V.Errors.Count > 0)
                            .SelectMany(E => E.Errors)
                        .Select(EM => EM.ErrorMessage)
                        .ToList()
                    };
                    return new BadRequestObjectResult(ValidationErrorResponse);
                };
            });
            return Services;
        }

    }
}
