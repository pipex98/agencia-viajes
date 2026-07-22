using AgenciaViajes.Application.Dto.Reserva;

namespace AgenciaViajes.Application.Interfaces.Services
{
    public interface IEmailService
    {
        Task EnviarEmail(List<DestinatarioEmailDto> destinatarios, string asunto, string cuerpo);
    }
}
