using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AgenciaViajes.Application.Dto.DetalleReserva;
using AgenciaViajes.Application.Dto.Hotel;
using AgenciaViajes.Application.Dto.Reserva;
using AgenciaViajes.Application.Interfaces.Repositories;
using AgenciaViajes.Domain.Entities;
using AgenciaViajes.Infrastructure.Data;

namespace AgenciaViajes.Infrastructure.Repositories
{
    public class HabitacionRepository(AppDbContext dbContext, IMapper mapper,
    ILogger<HabitacionRepository> _logger, IHotelRepository hotelRepository, 
    ITipoHabitacionRepository tipoHabitacionRepository, IHuespedRepository huespedRepository) 
    : IHabitacionRepository
    {
        public async Task<List<HabitacionDto>> BuscarHabitacion(ParametrosBusquedaHabitacionDto dto)
        {
            _logger.LogInformation("Recuperando habitaciones disponibles");

            var query = await dbContext.Habitaciones.AsNoTracking()
                .Where(ha => ha.Hotel.Ciudad.Nombre == dto.CiudadDestino
                    && ha.Hotel.Estado == "Habilitado"
                    && ha.Estado == "Habilitada"
                    && ha.CantidadHuespedes >= dto.CantidadHuespedes
                    && !dbContext.Reservas.Any(r => r.IdHabitacion == ha.IdHabitacion
                    && r.Estado != "Cancelada"
                    && (
                        (dto.FechaIngreso < r.FechaSalida && dto.FechaSalida > r.FechaIngreso)
                    )))
                .Select(ha => new
                {
                    ha.IdHabitacion,
                    Ciudad = ha.Hotel.Ciudad.Nombre,
                    Hotel = ha.Hotel.Nombre,
                    TipoHabitacion = ha.TipoHabitacion.Nombre,
                    ha.CostoBase,
                    ha.Impuestos,
                    ha.CantidadHuespedes
                })
                .ToListAsync();

            if (query.Count == 0)
            {
                _logger.LogWarning("La recuperacion de las habitaciones fallo. Habitaciones disponibles con los parametros {CiudadDestino} {FechaIngreso} {FechaSalida} {CantidadHuespedes} no se encontro", dto.CiudadDestino, dto.FechaIngreso, dto.FechaSalida, dto.CantidadHuespedes);

                throw new KeyNotFoundException("No se encontraron habitaciones disponibles");
            }

            _logger.LogInformation("Proyectando informacion de las habitaciones disponibles");

            int diasEstancia = (dto.FechaSalida - dto.FechaIngreso).Days;

            var habitaciones = query.Select(ha => new HabitacionDto
            {
                NumeroHabitacion = ha.IdHabitacion,
                Ciudad = ha.Ciudad,
                Hotel = ha.Hotel,
                TipoHabitacion = ha.TipoHabitacion,
                Subtotal = Math.Round(diasEstancia * ha.CostoBase, 2),
                Impuestos = Math.Round((diasEstancia * ha.CostoBase) * ha.Impuestos / 100m, 2),
                CapacidadMaxima = ha.CantidadHuespedes
            })
            .ToList();

            return habitaciones;
        }

        public async Task<List<ReservaDto>> ObtenerReservaciones(int id)
        {
            var reservaciones = await dbContext.Reservas.AsNoTracking()
                .Include(r => r.Habitacion)
                .ThenInclude(r => r.Hotel)
                .Include(r => r.Huesped)
                .Include(r => r.DetalleReservas)
                .Where(r => r.IdHuesped == id)
                .Select(r => new ReservaDto
                {
                    FechaIngreso = DateOnly.FromDateTime(r.FechaIngreso),
                    FechaSalida = DateOnly.FromDateTime(r.FechaSalida),
                    CantidadHuespedes = r.Habitacion.CantidadHuespedes,
                    Hotel = r.Habitacion.Hotel.Nombre,
                    NombreCliente = r.Huesped.Nombres + " " + r.Huesped.Apellidos,
                    NumeroReserva = r.IdReserva.ToString(),
                    FechaReserva = DateOnly.FromDateTime(r.FechaReserva),
                    Estado = r.Estado,
                    Subtotal = r.Subtotal,
                    Iva = r.Iva,
                    Total = r.Total,
                    DetalleReserva = r.DetalleReservas.Select(dr => new ReservaDetalleReservaDto
                    {
                        Cantidad = dr.Cantidad,
                        Concepto = dr.Concepto,
                        PrecioUnitario = dr.PrecioUnitario,
                        Importe = dr.Importe,
                    })
                    .ToList()
                })
                .ToListAsync();

            _logger.LogInformation("Recuperando reservaciones con ID: {IdHuesped}", id);

            if (reservaciones == null)
            {
                _logger.LogWarning("La recuperacion de las reservaciones fallo con ID: {IdHuesped}", id);

                throw new KeyNotFoundException("No se encontraron reservaciones");
            }

            return reservaciones;
        }

        public async Task<AddReservaDto> ReservarHabitacion(AddReservaDto dto)
        {
            var huesped = await huespedRepository.ObtenerHuespedPorIdAsync(dto.IdHuesped);

            var habitacion = await this.ObtenerHabitacionPorIdAsync(dto.IdHabitacion);

            habitacion.Desabilitar();

            _logger.LogInformation("Actualizando estado de la habitacion con ID: {IdHabitacion}", dto.IdHabitacion);

            dbContext.Habitaciones.Update(habitacion);
            await dbContext.SaveChangesAsync();

            _logger.LogInformation("Habitacion actualizada con exito con ID: {IdHabitacion}", dto.IdHabitacion);

            var reserva = mapper.Map<Reserva>(dto);

            reserva.Huesped = huesped;
            reserva.Habitacion = habitacion;

            foreach (var detalle in reserva.DetalleReservas)
            {
                detalle.Importe = detalle.ObtenerImporte();
            }

            reserva.Subtotal = reserva.ObtenerSubtotal();
            reserva.Total = reserva.ObtenerTotal();

            var comision = new ComisionReserva();

            comision.MontoBaseReserva = reserva.Total;
            comision.PorcentajeComision = 0.50m;
            comision.MontoComision = comision.ObtenerMontoComision();

            reserva.ComisionReservas.Add(comision);

            dbContext.Reservas.Add(reserva);
            await dbContext.SaveChangesAsync();

            _logger.LogInformation("Reserva creada con exito con ID: {IdReserva}", reserva.IdReserva);

            return dto;
        }

        public async Task<UpsertHabitacionDto> UpdateHabitacionAsync(int id, UpsertHabitacionDto dto)
        {
            var habitacion = await this.ObtenerHabitacionPorIdAsync(id);

            var hotel = await hotelRepository.ObtenerHotelPorId(dto.IdHotel);

            var tipoHabitacion = await tipoHabitacionRepository.ObtenerTipoHabitacionPorIdAsync(dto.IdTipoHabitacion);

            habitacion.Hotel = hotel;
            habitacion.TipoHabitacion = tipoHabitacion;

            mapper.Map(dto, habitacion);

            await dbContext.SaveChangesAsync();

            _logger.LogInformation("Habitacion actualizada con exito con ID: {IdHabitacion}", habitacion.IdHabitacion);

            return dto;
        }

        public async Task<bool> DesabilitarHabitacionAsync(int id)
        {
            var habitacion = await this.ObtenerHabitacionPorIdAsync(id);

            habitacion.Desabilitar();

            _logger.LogInformation("Desabilitando habitacion con ID: {IdHabitacion}", id);

            return await dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> HabilitarHabitacionAsync(int id)
        {
            var habitacion = await this.ObtenerHabitacionPorIdAsync(id);

            habitacion.Habilitar();

            _logger.LogInformation("Habilitando habitacion con ID: {IdHabitacion}", id);

            return await dbContext.SaveChangesAsync() > 0;
        }

        public async Task<Habitacion> ObtenerHabitacionPorIdAsync(int id)
        {
            var habitacion = await dbContext.Habitaciones.FirstOrDefaultAsync(h => h.IdHabitacion == id);

            _logger.LogInformation("Recuperando habitacion con ID: {IdHabitacion}", id);

            if (habitacion == null)
            {
                _logger.LogWarning("La recuperacion de la habitacion fallo con ID: {IdHabitacion} no se encontro", id);

                throw new KeyNotFoundException("habitacion no encontrada");
            }

            return habitacion;
        }

        public async Task<Habitacion> ObtenerHabitacionConDetallesPorIdAsync(int id)
        {
            var habitacion = await dbContext.Habitaciones.AsNoTracking()
            .Include(h => h.Hotel)
            .ThenInclude(h => h.Agente)
            .FirstOrDefaultAsync(x => x.IdHabitacion == id);

            _logger.LogInformation("Recuperando habitacion con ID: {IdHabitacion}", id);

            if (habitacion == null)
            {
                _logger.LogWarning("La recuperacion de la habitacion fallo con ID: {IdHabitacion} no se encontro", id);

                throw new KeyNotFoundException("habitacion no encontrada");
            }

            return habitacion;
        }
    }
}
