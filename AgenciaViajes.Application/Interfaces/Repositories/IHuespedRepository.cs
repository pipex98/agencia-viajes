using AgenciaViajes.Application.Dto.Habitacion;
using AgenciaViajes.Application.Dto.Usuario;
using AgenciaViajes.Domain.Entities;

namespace AgenciaViajes.Application.Interfaces.Repositories
{
    public interface IHuespedRepository
    {
        Task<AddHuespedDto> AddHuespedAsync(AddHuespedDto dto);

        Task<string> LoginHuespedAsync(LoginDto dto);

        Task<Huesped> ObtenerHuespedPorIdAsync(int id);
    }
}
