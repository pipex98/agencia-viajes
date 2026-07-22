using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using AgenciaViajes.Application.Commands.Hotel;
using AgenciaViajes.Application.Dto.Hotel;
using FluentValidation.Results;

namespace AgenciaViajes.API.Controllers
{
    [Route("api/")]
    [ApiController]
    [EnableRateLimiting("fixed")]
    public class HotelesController(ISender sender, IValidator<UpsertHotelDto> _hotelValidator,
    ILogger<HotelesController> _logger, IValidator<UpsertHabitacionDto> _habitacionValidator) : ControllerBase
    {
        [ProducesResponseType(typeof(UpsertHotelDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(IEnumerable<ValidationFailure>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Produces("application/json")]
        [HttpPost("hoteles")]
        public async Task<IActionResult> AddHotelAsync([FromBody] UpsertHotelDto dto)
        {
            _logger.LogInformation("Iniciando la solicitud");

            var validationResult = await _hotelValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                _logger.LogInformation("Fallo en la solicitud");

                throw new ValidationException(validationResult.Errors);
            }

            var result = await sender.Send(new AddHotelCommand(dto));

            _logger.LogInformation("Recuperando resultado del comando AddHotelCommand");

            return Ok(result);
        }

        [ProducesResponseType(typeof(UpsertHotelDto), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(IEnumerable<ValidationFailure>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Produces("application/json")]
        [HttpPut("hoteles/{id}")]
        public async Task<IActionResult> UpdateHotelAsync(int id, [FromBody] UpsertHotelDto dto)
        {
            _logger.LogInformation("Iniciando la solicitud");

            var validationResult = await _hotelValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                _logger.LogInformation("Fallo en la solicitud");

                throw new ValidationException(validationResult.Errors);
            }

            var result = await sender.Send(new UpdateHotelCommand(id, dto));

            _logger.LogInformation("Recuperando resultado del comando UpdateHotelCommand");

            return Ok(result);
        }

        [ProducesResponseType(typeof(Boolean), StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [HttpPatch("hoteles/desabilitar_hotel/{id}")]
        public async Task<IActionResult> DesabilitarHotelAsync(int id)
        {
            _logger.LogInformation("Iniciando la solicitud");

            var result = await sender.Send(new DisableHotelCommand(id));

            _logger.LogInformation("Recuperando resultado del comando DisableHotelCommand");

            return Ok(result);
        }

        [ProducesResponseType(typeof(Boolean), StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [HttpPatch("hoteles/habilitar_hotel/{id}")]
        public async Task<IActionResult> HabilitarHotelAsync(int id)
        {
            _logger.LogInformation("Iniciando la solicitud");

            var result = await sender.Send(new EnableHotelCommand(id));

            _logger.LogInformation("Recuperando resultado del comando EnableHotelCommand");

            return Ok(result);
        }

        [ProducesResponseType(typeof(UpsertHabitacionDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(IEnumerable<ValidationFailure>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Produces("application/json")]
        [HttpPost("hoteles/asignar_habitacion")]
        public async Task<IActionResult> AsignarHabitacionAsync([FromBody] UpsertHabitacionDto dto)
        {
            _logger.LogInformation("Iniciando la solicitud");

            var validationResult = await _habitacionValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                _logger.LogInformation("Fallo en la solicitud");

                throw new ValidationException(validationResult.Errors);
            }

            var result = await sender.Send(new AssignHabitacionCommand(dto));

            _logger.LogInformation("Recuperando resultado del comando AddHabitacionCommand");

            return Ok(result);
        }
    }
}
