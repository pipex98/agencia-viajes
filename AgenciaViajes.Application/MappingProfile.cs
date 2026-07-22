using AutoMapper;
using AgenciaViajes.Application.Dto.DetalleReserva;
using AgenciaViajes.Application.Dto.Habitacion;
using AgenciaViajes.Application.Dto.Hotel;
using AgenciaViajes.Application.Dto.Reserva;
using AgenciaViajes.Domain.Entities;

namespace AgenciaViajes.Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UpsertHotelDto, Hotel>()
                .ForMember(dest => dest.IdAgente, opt => opt.Ignore())
                .ForMember(dest => dest.IdCiudad, opt => opt.Ignore())
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre))
                .ForMember(dest => dest.Direccion, opt => opt.MapFrom(src => src.Direccion))
                .ForMember(dest => dest.Descripcion, opt => opt.MapFrom(src => src.Descripcion));

            CreateMap<UpsertHabitacionDto, Habitacion>()
                .ForMember(dest => dest.IdHotel, opt => opt.Ignore())
                .ForMember(dest => dest.IdTipoHabitacion, opt => opt.Ignore())
                .ForMember(dest => dest.CostoBase, opt => opt.MapFrom(src => src.CostoBase))
                .ForMember(dest => dest.Impuestos, opt => opt.MapFrom(src => src.Impuestos))
                .ForMember(dest => dest.CantidadHuespedes, opt => opt.MapFrom(src => src.CantidadHuespedes))
                .ForMember(dest => dest.Ubicacion, opt => opt.MapFrom(src => src.Ubicacion));

            CreateMap<AddHuespedDto, Huesped>()
                .ForMember(dest => dest.IdTipoDocumento, opt => opt.Ignore())
                .ForMember(dest => dest.IdGenero, opt => opt.Ignore())
                .ForMember(dest => dest.Nombres, opt => opt.MapFrom(src => src.Nombres))
                .ForMember(dest => dest.Apellidos, opt => opt.MapFrom(src => src.Apellidos))
                .ForMember(dest => dest.FechaNacimiento, opt => opt.MapFrom(src => src.FechaNacimiento))
                .ForMember(dest => dest.NumeroDocumento, opt => opt.MapFrom(src => src.NumeroDocumento))
                .ForMember(dest => dest.CorreoElectronico, opt => opt.MapFrom(src => src.CorreoElectronico))
                .ForMember(dest => dest.Contraseña, opt => opt.Ignore())
                .ForMember(dest => dest.Telefono, opt => opt.MapFrom(src => src.Telefono));

            CreateMap<AddReservaDto, Reserva>()
                .ForMember(dest => dest.IdHuesped, opt => opt.Ignore())
                .ForMember(dest => dest.IdHabitacion, opt => opt.Ignore())
                .ForMember(dest => dest.FechaIngreso, opt => opt.MapFrom(src => src.FechaIngreso))
                .ForMember(dest => dest.FechaSalida, opt => opt.MapFrom(src => src.FechaSalida))
                .ForMember(dest => dest.ContactoEmergencia, opt => opt.MapFrom(src => src.ContactoEmergencia))
                .ForMember(dest => dest.Subtotal, opt => opt.Ignore())
                .ForMember(dest => dest.Total, opt => opt.Ignore());

            CreateMap<AddDetalleReservaDto, DetalleReserva>()
                .ForMember(dest => dest.Concepto, opt => opt.MapFrom(src => src.Concepto))
                .ForMember(dest => dest.Cantidad, opt => opt.MapFrom(src => src.Cantidad))
                .ForMember(dest => dest.PrecioUnitario, opt => opt.MapFrom(src => src.PrecioUnitario))
                .ForMember(dest => dest.Importe, opt => opt.Ignore());
        }
    }
}
