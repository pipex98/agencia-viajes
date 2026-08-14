using AgenciaViajes.Application.Dto.Hotel;
using AgenciaViajes.Domain.Entities;

namespace AgenciaViajes.Application.Interfaces.Repositories
{
    public interface IHotelRepository
    {
        Task<UpsertHotelDto> AddHotelAsync(UpsertHotelDto dto);

        Task<UpsertHotelDto> UpdateHotelAsync(int id, UpsertHotelDto dto);

        Task<bool> DesabilitarHotelAsync(int id);

        Task<bool> HabilitarHotelAsync(int id);

        Task<UpsertHabitacionDto> AssignHabitacionAsync(UpsertHabitacionDto dto);

        Task<Hotel> ObtenerHotelPorId(int id);
    }
}
