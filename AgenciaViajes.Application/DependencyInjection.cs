using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AgenciaViajes.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationDI(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
            services.AddAutoMapper(cfg => { cfg.AddMaps(typeof(MappingProfile).Assembly); });
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}
