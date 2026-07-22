namespace AgenciaViajes.Application.Dto.Habitacion
{
    public class AddHuespedDto
    {
        public int? IdTipoDocumento { get; set; }

        public int? IdGenero { get; set; }

        public string Nombres { get; set; } = string.Empty;

        public string Apellidos { get; set; } = string.Empty;

        public DateOnly FechaNacimiento { get; set; }

        public string NumeroDocumento { get; set; } = string.Empty;

        public string CorreoElectronico { get; set; } = string.Empty;

        public string Contraseña { get; set; } = string.Empty;

        public string Telefono { get; set; } = string.Empty;
    }
}
