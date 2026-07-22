using AgenciaViajes.Domain.Entities;

namespace AgenciaViajes.Application.Interfaces.Services
{
    public interface ITokenService
    {
        string Create(Huesped huesped);
    }
}
