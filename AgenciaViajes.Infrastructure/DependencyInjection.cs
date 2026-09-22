using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AgenciaViajes.Application.Interfaces.Repositories;
using AgenciaViajes.Application.Interfaces.Services;
using AgenciaViajes.Infrastructure.Data;
using AgenciaViajes.Infrastructure.Repositories;
using AgenciaViajes.Infrastructure.Services;
using AgenciaViajes.Application.Interfaces;

namespace AgenciaViajes.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureDI(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                 sqlServerOptionsAction: sqlOptions => {
                     sqlOptions.EnableRetryOnFailure(
                         maxRetryCount: 10,
                         maxRetryDelay: TimeSpan.FromSeconds(30),
                         errorNumbersToAdd: null
                     );
                 }
            );
            });

            services.AddScoped<IHotelRepository, HotelRepository>();
            services.AddScoped<IHabitacionRepository, HabitacionRepository>();
            services.AddScoped<IHuespedRepository, HuespedRepository>();
            services.AddScoped<ITipoHabitacionRepository, TipoHabitacionRepository>();
            services.AddScoped<IAgenteRepository, AgenteRepository>();
            services.AddScoped<ITipoDocumentoRepository, TipoDocumentoRepository>();
            services.AddScoped<IGeneroRepository, GeneroRepository>();
            services.AddScoped<ICiudadRepository, CiudadRepository>();
            services.AddScoped<IReservaRepository, ReservaRepository>();

            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ITokenService, TokenService>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
