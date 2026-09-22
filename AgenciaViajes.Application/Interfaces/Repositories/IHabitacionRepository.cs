using AgenciaViajes.Application.Dto.Hotel;
using AgenciaViajes.Application.Dto.Reserva;
using AgenciaViajes.Domain.Entities;

namespace AgenciaViajes.Application.Interfaces.Repositories
{
    public interface IHabitacionRepository
    {
        Task<List<Habitacion>> ObtenerHabitacionesDisponibles(ParametrosBusquedaHabitacionDto dto);

        Task<Habitacion> ObtenerHabitacionPorIdAsync(int id);

        Task<Habitacion> ObtenerHabitacionConDetallesPorIdAsync(int id);

        void Actualizar(Habitacion habitacion);

        void Crear(Habitacion habitacion);

    }
}
