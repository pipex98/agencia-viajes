using AgenciaViajes.Domain.Entities;

namespace AgenciaViajes.Application.Interfaces.Repositories
{
    public interface IAgenteRepository
    {
        Task<Agente> ObtenerAgentePorId(int id);
    }
}
