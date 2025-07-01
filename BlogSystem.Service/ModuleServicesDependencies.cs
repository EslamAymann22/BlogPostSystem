using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BlogSystem.Service
{
    public static class ModuleServicesDependencies
    {
        public static IServiceCollection AddServicesDependencies(this IServiceCollection Service)
        {

            Service.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            return Service;


        }


    }
}
