using AgenciaViajes.Domain.Entities;

namespace AgenciaViajes.Application.Interfaces.Repositories
{
    public interface IReservaRepository
    {
        void Crear(Reserva reserva);

        Task<List<Reserva>> ObtenerReservasPorHuespedAsync(int id);
    }
}
