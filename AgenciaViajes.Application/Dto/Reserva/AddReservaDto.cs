using System.Text.Json.Serialization;
using AgenciaViajes.Application.Dto.DetalleReserva;

namespace AgenciaViajes.Application.Dto.Reserva
{
    public class AddReservaDto
    {
        public int IdHuesped { get; set; }

        public int IdHabitacion { get; set; }

        public DateTime FechaIngreso { get; set; }

        public DateTime FechaSalida { get; set; }

        public string ContactoEmergencia { get; set; } = string.Empty;

        public List<AddDetalleReservaDto> DetalleReservas { get; set; } = new List<AddDetalleReservaDto>();
    }
}
