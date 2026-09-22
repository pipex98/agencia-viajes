using AgenciaViajes.Application.Dto.Hotel;
using AgenciaViajes.Domain.Entities;

namespace AgenciaViajes.Application.Interfaces.Repositories
{
    public interface IHotelRepository
    {
        Task<Hotel> ObtenerHotelPorId(int id);

        void Crear(Hotel hotel);

        void Actualizar(Hotel hotel);

    }
}
