using AgenciaViajes.Application.Dto.Reserva;
using AgenciaViajes.Application.Dto.Usuario;
using AgenciaViajes.Application.Queries.Auth;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace AgenciaViajes.API.Controllers
{
    [Route("api/")]
    [ApiController]
    [EnableRateLimiting("fixed")]
    public class AuthController(ISender sender, IValidator<LoginDto> _loginValidator, ILogger<AuthController> _logger) : ControllerBase
    {
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(IEnumerable<ValidationFailure>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [HttpPost("auth/login_huesped")]
        public async Task<IActionResult> LoginHuespedAsync([FromBody] LoginDto dto)
        {
            _logger.LogInformation("Iniciando la solicitud");

            var validationResult = await _loginValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                _logger.LogInformation("Fallo en la solicitud");

                throw new ValidationException(validationResult.Errors);
            }

            var result = await sender.Send(new LoginHuespedQuery(dto));

            _logger.LogInformation("Recuperando resultado del comando LoginHuespedQuery");

            return Ok(result);
        }
    }
}
