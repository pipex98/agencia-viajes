using AgenciaViajes.Application.Commands.Hotel;
using AgenciaViajes.Application.Dto.Hotel;
using AgenciaViajes.Application.Dto.Reserva;
using AgenciaViajes.Application.Queries.Habitacion;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace AgenciaViajes.API.Controllers
{
    [ApiController]
    [Route("api/")]
    [EnableRateLimiting("fixed")]
    public class HabitacionesController(ISender sender, ILogger<HabitacionDto> _logger, IValidator<UpsertHabitacionDto> _habitacionValidator) : ControllerBase
    {
        [ProducesResponseType(typeof(List<HabitacionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Produces("application/json")]
        [HttpGet("habitaciones")]
        public async Task<IActionResult> BuscarHabitacionAsync([FromQuery] ParametrosBusquedaHabitacionDto dto)
        {
            _logger.LogInformation("Iniciando la solicitud");

            var result = await sender.Send(new SearchHabitacionQuery(dto));

            _logger.LogInformation("Recuperando el resultado del comando SearchHabitacionQuery");

            return Ok(result);
        }

        [ProducesResponseType(typeof(List<ReservaDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Produces("application/json")]
        [HttpGet("habitaciones/obtener_reservaciones/{id_agente}")]
        public async Task<IActionResult> ObtenerReservacionesAsync(int id_agente)
        {
            _logger.LogInformation("Iniciando la solicitud");

            var result = await sender.Send(new ObtenerReservacionesQuery(id_agente));

            _logger.LogInformation("Recuperando el resultado del comando ObtenerReservacionesQuery");

            return Ok(result);
        }

        [ProducesResponseType(typeof(UpsertHabitacionDto), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(IEnumerable<ValidationFailure>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [HttpPut("habitaciones/{id}")]
        public async Task<IActionResult> UpdateHabitacionAsync(int id, [FromBody] UpsertHabitacionDto dto)
        {
            _logger.LogInformation("Iniciando la solicitud");

            var validationResult = await _habitacionValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                _logger.LogInformation("Fallo en la solicitud");

                throw new ValidationException(validationResult.Errors);
            }
            var result = await sender.Send(new UpdateHabitacionCommand(id, dto));

            _logger.LogInformation("Recuperando resultado del comando UpdateHabitacionCommand");

            return Ok(result);
        }

        [ProducesResponseType(typeof(Boolean), StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [HttpPatch("habitaciones/desabilitar_habitacion/{id}")]
        public async Task<IActionResult> DesabilitarHabitacionAsync(int id)
        {
            var result = await sender.Send(new DisableHabitacionCommand(id));
            return Ok(result);
        }

        [ProducesResponseType(typeof(Boolean), StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [HttpPatch("habitaciones/habilitar_habitacion/{id}")]
        public async Task<IActionResult> HabilitarHabitacionAsync(int id)
        {
            var result = await sender.Send(new EnableHabitacionCommand(id));
            return Ok(result);
        }
    }
}
