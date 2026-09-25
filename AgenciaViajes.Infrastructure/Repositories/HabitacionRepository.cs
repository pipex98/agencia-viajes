using AgenciaViajes.Application.Dto.Reserva;
using AgenciaViajes.Application.Interfaces.Repositories;
using AgenciaViajes.Domain.Entities;
using AgenciaViajes.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AgenciaViajes.Infrastructure.Repositories
{
    public class HabitacionRepository(AppDbContext dbContext, ILogger<HabitacionRepository> _logger)
    : IHabitacionRepository
    {
        public async Task<List<Habitacion>> ObtenerHabitacionesDisponibles(ParametrosBusquedaHabitacionDto dto)
        {
            _logger.LogInformation("Recuperando habitaciones disponibles con los parametros {CiudadDestino} {FechaIngreso} {FechaSalida} {CantidadHuespedes}", dto.CiudadDestino, dto.FechaIngreso, dto.FechaSalida, dto.CantidadHuespedes);

            var habitaciones = await dbContext.Habitaciones.AsNoTracking()
                .Include(ha => ha.Hotel)
                .ThenInclude(h => h.Ciudad)
                .Include(ha => ha.TipoHabitacion)
                .Where(ha => ha.Hotel.Ciudad.Nombre == dto.CiudadDestino
                    && ha.Hotel.Estado == "Habilitado"
                    && ha.Estado == "Habilitada"
                    && ha.CantidadHuespedes >= dto.CantidadHuespedes
                    && !dbContext.Reservas.Any(r => r.IdHabitacion == ha.IdHabitacion
                    && r.Estado != "Cancelada"
                    && (
                        (dto.FechaIngreso < r.FechaSalida && dto.FechaSalida > r.FechaIngreso)
                    )))
                    .ToListAsync();

            if (habitaciones.Count == 0)
            {
                _logger.LogWarning("La recuperacion de las habitaciones fallo. Habitaciones disponibles con los parametros {CiudadDestino} {FechaIngreso} {FechaSalida} {CantidadHuespedes} no se encontraron", dto.CiudadDestino, dto.FechaIngreso, dto.FechaSalida, dto.CantidadHuespedes);

                throw new KeyNotFoundException("No se encontraron habitaciones disponibles");
            }

            return habitaciones;
        }

        public async Task<Habitacion> ObtenerHabitacionPorIdAsync(int id)
        {
            var habitacion = await dbContext.Habitaciones.FirstOrDefaultAsync(h => h.IdHabitacion == id);

            _logger.LogInformation("Recuperando habitacion con el ID: {IdHabitacion}", id);

            if (habitacion == null)
            {
                _logger.LogWarning("La recuperacion de la habitacion fallo con el ID: {IdHabitacion} no se encontro", id);

                throw new KeyNotFoundException("habitacion no encontrada");
            }

            return habitacion;
        }

        public async Task<Habitacion> ObtenerHabitacionConDetallesPorIdAsync(int id)
        {
            var habitacion = await dbContext.Habitaciones.AsNoTracking()
            .Include(h => h.Hotel)
            .ThenInclude(h => h.Agente)
            .FirstOrDefaultAsync(x => x.IdHabitacion == id);

            _logger.LogInformation("Recuperando habitacion con el ID: {IdHabitacion}", id);

            if (habitacion == null)
            {
                _logger.LogWarning("La recuperacion de la habitacion fallo con el ID: {IdHabitacion} no se encontro", id);

                throw new KeyNotFoundException("habitacion no encontrada");
            }

            return habitacion;
        }

        public void Crear(Habitacion habitacion)
        {
            _logger.LogInformation("Agregando el objeto habitacion al DbSet");

            dbContext.Habitaciones.Add(habitacion);
        }

        public void Actualizar(Habitacion habitacion)
        {
            _logger.LogInformation("Marcando la entidad habitacion como modificada");

            dbContext.Habitaciones.Update(habitacion);
        }
    }
}
