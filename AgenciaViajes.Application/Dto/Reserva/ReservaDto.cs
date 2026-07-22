using AgenciaViajes.Application.Dto.DetalleReserva;

namespace AgenciaViajes.Application.Dto.Reserva
{
    public class ReservaDto
    {
        public DateOnly FechaIngreso { get; set; }

        public DateOnly FechaSalida { get; set; }

        public int? CantidadHuespedes { get; set; }

        public string Hotel { get; set; } = string.Empty;

        public string NombreCliente { get; set; } = string.Empty;

        public string NumeroReserva { get; set; } = string.Empty;

        public DateOnly FechaReserva { get; set; }

        public decimal Subtotal { get; set; }

        public decimal Iva { get; set; }

        public decimal Total { get; set; }

        public string? Estado { get; set; }

        public List<AddDetalleReservaDto> DetalleReserva { get; set; } = new List<AddDetalleReservaDto>();
    }
}
