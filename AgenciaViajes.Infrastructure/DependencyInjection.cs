using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AgenciaViajes.Application.Interfaces.Repositories;
using AgenciaViajes.Application.Interfaces.Services;
using AgenciaViajes.Infrastructure.Data;
using AgenciaViajes.Infrastructure.Repositories;
using AgenciaViajes.Infrastructure.Services;

namespace AgenciaViajes.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureDI(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddScoped<IHotelRepository, HotelRepository>();
            services.AddScoped<IHabitacionRepository, HabitacionRepository>();
            services.AddScoped<IHuespedRepository, HuespedRepository>();

            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ITokenService, TokenService>();

            return services;
        }
    }
}
