using MediatR;
using Microsoft.Extensions.Logging;
using AgenciaViajes.Application.Dto.Usuario;
using AgenciaViajes.Application.Interfaces.Repositories;

namespace AgenciaViajes.Application.Queries.Auth
{
    public record LoginHuespedQuery(LoginDto dto) : IRequest<string>;

    public class LoginHuespedQueryHandler(IHuespedRepository huespedRepository, ILogger<LoginHuespedQuery> _logger)
    : IRequestHandler<LoginHuespedQuery, string>
    {
        public async Task<string> Handle(LoginHuespedQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Enviando solicitud desde el mediator");

            return await huespedRepository.LoginHuespedAsync(request.dto);
        }
    }
}
