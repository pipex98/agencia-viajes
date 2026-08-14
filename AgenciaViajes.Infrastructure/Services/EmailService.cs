using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using AgenciaViajes.Application.Interfaces.Services;
using AgenciaViajes.Application.Dto.Reserva;

namespace AgenciaViajes.Infrastructure.Services
{
    public class EmailService(IConfiguration _configuration, ILogger<EmailService> _logger) : IEmailService
    {
        public async Task EnviarEmail(List<DestinatarioEmailDto> destinatarios, string asunto, string cuerpo)
        {
            _logger.LogInformation("Obteniendo valores de configuracion para el envio de correo");

            var from = _configuration["Mail:From"];
            var name = _configuration["Mail:Name"];
            var smtp = _configuration["Mail:Smtp"];
            var port = _configuration["Mail:Port"];
            var password = _configuration["Mail:Password"];

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(name, from!));

            _logger.LogInformation("Agregando destinatarios");
            
            foreach (var destinatario in destinatarios)
            {
                message.To.Add(new MailboxAddress(destinatario.Nombre, destinatario.Email));
            }

            message.Subject = asunto;

            _logger.LogInformation("Agregando seccion de datos principales al correo");

            BodyBuilder bodyBuilder = new()
            {
                HtmlBody = cuerpo
            };
            message.Body = bodyBuilder.ToMessageBody();

            _logger.LogInformation("Preparando cliente SMTP");

            using (var client = new SmtpClient())
            {
                client.Connect(smtp!, int.Parse(port!), false);
                client.Authenticate(from!, password!);
                client.Send(message);
                client.Disconnect(true);
            }

            _logger.LogInformation("Correos enviados desde: {CorreoElectronico}", from);

        }
    }
}
