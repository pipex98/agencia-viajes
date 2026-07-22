namespace AgenciaViajes.Application.Dto.Reserva
{
    public class ParametrosBusquedaHabitacionDto
    {
        public string? CiudadDestino { get; set; }

        public DateTime FechaIngreso { get; set; }

        public DateTime FechaSalida { get; set; }

        public int CantidadHuespedes { get; set; }

    }
}
