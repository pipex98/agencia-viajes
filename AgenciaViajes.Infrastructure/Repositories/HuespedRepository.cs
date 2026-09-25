using AgenciaViajes.Application.Interfaces.Repositories;
using AgenciaViajes.Domain.Entities;
using AgenciaViajes.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AgenciaViajes.Infrastructure.Repositories
{
    public class HuespedRepository(AppDbContext dbContext, ILogger<HuespedRepository> _logger)
    : IHuespedRepository
    {
        public void Crear(Huesped huesped)
        {
            _logger.LogInformation("Agregando el objeto huesped al DbSet");

            dbContext.Huespedes.Add(huesped);
        }

        public async Task<Huesped> ObtenerHuespedPorCorreoElectronicoAsync(string correoElectronico)
        {
            var huesped = await dbContext.Huespedes.AsNoTracking()
            .FirstOrDefaultAsync(x => x.CorreoElectronico == correoElectronico);

            _logger.LogInformation("Recuperando huesped con correo electronico: {CorreoElectronico}",
            correoElectronico);

            if (huesped == null)
            {
                _logger.LogWarning("La recuperacion del huesped fallo con correo electronico: {CorreoElectronico} no se encontro", correoElectronico);

                throw new KeyNotFoundException("huesped no encontrado");
            }

            return huesped;

        }

        public async Task<Huesped> ObtenerHuespedPorIdAsync(int id)
        {
            var huesped = await dbContext.Huespedes.FirstOrDefaultAsync(x => x.IdHuesped == id);

            _logger.LogInformation("Recuperando huesped con ID: {IdHuesped}", id);

            if (huesped == null)
            {
                _logger.LogWarning("La recuperacion del huesped fallo con el ID: {IdHuesped} no se encontro", id);

                throw new KeyNotFoundException("huesped no encontrado");
            }

            return huesped;
        }
    }
}
