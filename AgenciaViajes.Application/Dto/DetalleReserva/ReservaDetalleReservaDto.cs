namespace AgenciaViajes.Application.Dto.DetalleReserva
{
    public class ReservaDetalleReservaDto
    {
        public string Concepto { get; set; } = string.Empty;

        public int Cantidad { get; set; }

        public decimal PrecioUnitario { get; set; }

        public decimal Importe { get; set; }
    }
}
