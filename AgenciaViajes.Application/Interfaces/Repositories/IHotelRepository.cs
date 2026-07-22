using AgenciaViajes.Application.Dto.Hotel;

namespace AgenciaViajes.Application.Interfaces.Repositories
{
    public interface IHotelRepository
    {
        Task<UpsertHotelDto> AddHotelAsync(UpsertHotelDto dto);
        Task<UpsertHotelDto> UpdateHotelAsync(int id, UpsertHotelDto dto);
        Task<bool> DesabilitarHotelAsync(int id);
        Task<bool> HabilitarHotelAsync(int id);
        Task<UpsertHabitacionDto> AssignHabitacionAsync(UpsertHabitacionDto dto);
    }
}
