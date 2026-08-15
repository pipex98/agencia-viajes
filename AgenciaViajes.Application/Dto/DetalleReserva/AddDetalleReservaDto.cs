namespace AgenciaViajes.Application.Dto.DetalleReserva
{
    public class AddDetalleReservaDto
    {
        public string Concepto { get; set; } = string.Empty;

        public int Cantidad { get; set; }

        public decimal PrecioUnitario { get; set; }
    }
}
