using AgenciaViajes.Domain.Entities;

namespace AgenciaViajes.Application.Interfaces.Repositories
{
    public interface ITipoHabitacionRepository
    {
        Task<TipoHabitacion> ObtenerTipoHabitacionPorIdAsync(int id);
    }
}
