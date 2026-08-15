using AgenciaViajes.Application.Dto.Hotel;
using AgenciaViajes.Application.Dto.Reserva;
using AgenciaViajes.Domain.Entities;

namespace AgenciaViajes.Application.Interfaces.Repositories
{
    public interface IHabitacionRepository
    {
        Task<List<HabitacionDto>> BuscarHabitacion(ParametrosBusquedaHabitacionDto dto);

        Task<List<ReservaDto>> ObtenerReservaciones(int id);

        Task<UpsertHabitacionDto> UpdateHabitacionAsync(int id, UpsertHabitacionDto dto);

        Task<bool> DesabilitarHabitacionAsync(int id);

        Task<bool> HabilitarHabitacionAsync(int id);

        Task<AddReservaDto> ReservarHabitacion(AddReservaDto dto);

        Task<Habitacion> ObtenerHabitacionPorIdAsync(int id);

        Task<Habitacion> ObtenerHabitacionConDetallesPorIdAsync(int id);
    }
}
