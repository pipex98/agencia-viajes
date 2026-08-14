namespace AgenciaViajes.Application.Dto.Reserva
{
    public class HabitacionDto
    {
        public int NumeroHabitacion { get; set; }

        public string Ciudad { get; set; } = string.Empty;

        public string Hotel { get; set; } = string.Empty;

        public string TipoHabitacion { get; set; } = string.Empty;

        public decimal Subtotal { get; set; }

        public decimal Impuestos { get; set; }

        public int CapacidadMaxima { get; set; }
    }
}
