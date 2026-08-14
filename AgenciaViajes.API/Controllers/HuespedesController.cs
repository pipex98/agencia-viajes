using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using AgenciaViajes.Application.Commands.Huesped;
using AgenciaViajes.Application.Dto.Habitacion;

namespace AgenciaViajes.API.Controllers
{
    [Route("api/")]
    [ApiController]
    [EnableRateLimiting("fixed")]

    public class HuespedesController(ISender sender, IValidator<AddHuespedDto> _huespedValidator, ILogger<HuespedesController> _logger) : ControllerBase
    {
        [ProducesResponseType(typeof(AddHuespedDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(IEnumerable<ValidationFailure>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Produces("application/json")]
        [HttpPost("huespedes")]
        public async Task<IActionResult> AddHuespedAsync([FromBody] AddHuespedDto dto)
        {
            _logger.LogInformation("Iniciando la solicitud");

            var validationResult = await _huespedValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                _logger.LogInformation("Fallo en la solicitud");

                throw new ValidationException(validationResult.Errors);
            }

            var result = await sender.Send(new AddHuespedCommand(dto));

            _logger.LogInformation("Recuperando resultado del comando AddHuespedCommand");

            return Ok(result);
        }
    }
}
