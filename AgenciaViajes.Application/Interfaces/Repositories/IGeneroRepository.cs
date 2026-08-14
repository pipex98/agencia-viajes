using AgenciaViajes.Domain.Entities;

namespace AgenciaViajes.Application.Interfaces.Repositories
{
    public interface IGeneroRepository
    {
        Task<Genero> ObtenerGeneroPorIdAsync(int id);
    }
}
