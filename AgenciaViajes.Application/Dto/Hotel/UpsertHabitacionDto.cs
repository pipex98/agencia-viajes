namespace AgenciaViajes.Application.Dto.Hotel
{
    public class UpsertHabitacionDto
    {
        public int IdHotel { get; set; }

        public int IdTipoHabitacion { get; set; }

        public decimal CostoBase { get; set; }

        public decimal Impuestos { get; set; }

        public int CantidadHuespedes { get; set; }

        public string Ubicacion { get; set; } = string.Empty;
    }
}
