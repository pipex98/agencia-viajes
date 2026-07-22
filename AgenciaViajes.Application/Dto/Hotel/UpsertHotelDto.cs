namespace AgenciaViajes.Application.Dto.Hotel
{
    public class UpsertHotelDto
    {
        public int? IdAgente { get; set; }

        public int? IdCiudad { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string Direccion { get; set; } = string.Empty;

        public string Descripcion { get; set; } = string.Empty;
    }
}
