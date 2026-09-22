using AgenciaViajes.Domain.Entities;

namespace AgenciaViajes.Application.Interfaces.Repositories
{
    public interface IHuespedRepository
    {
        Task<Huesped> ObtenerHuespedPorIdAsync(int id);

        Task<Huesped> ObtenerHuespedPorCorreoElectronicoAsync(string correoElectronico);

        void Crear(Huesped huesped);
    }
}
