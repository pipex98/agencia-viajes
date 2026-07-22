using AgenciaViajes.Application.Dto.DetalleReserva;
using AgenciaViajes.Application.Dto.Hotel;
using AgenciaViajes.Application.Dto.Reserva;
using AgenciaViajes.Application.Interfaces.Repositories;
using AgenciaViajes.Application.Interfaces.Services;
using AgenciaViajes.Domain.Entities;
using AgenciaViajes.Infrastructure.Data;
using AgenciaViajes.Infrastructure.Services;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AgenciaViajes.Infrastructure.Repositories
{
    public class HabitacionRepository(AppDbContext dbContext, IMapper mapper, ILogger<HabitacionRepository> _logger, IEmailService emailService, IConfiguration configuration) : IHabitacionRepository
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
                    DetalleReserva = r.DetalleReservas.Select(dr => new AddDetalleReservaDto
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

        public async Task<UpsertHabitacionDto> UpdateHabitacionAsync(int id, UpsertHabitacionDto dto)
        {
            var habitacion = await dbContext.Habitaciones.FirstOrDefaultAsync(x => x.IdHabitacion == id);

            _logger.LogInformation("Recuperando habitacion con ID: {IdHabitacion}", id);

            if (habitacion == null)
            {
                _logger.LogWarning("La actualizacion de la habitacion fallo. habitacion con ID: {IdHabitacion} no se encontro", id);

                throw new KeyNotFoundException("habitacion no encontrada");
            }

            var hotel = await dbContext.Hoteles.FirstOrDefaultAsync(x => x.IdHotel == dto.IdHotel);

            _logger.LogInformation("Recuperando hotel con ID: {IdHotel}", dto.IdHotel);

            if (hotel == null)
            {
                _logger.LogWarning("La actualizacion de la habitacion fallo. hotel con ID: {IdHotel} no se encontro", dto.IdHotel);

                throw new KeyNotFoundException("Hotel no encontrado");
            }

            var tipoHabitacion = await dbContext.TipoHabitaciones
            .FirstOrDefaultAsync(x => x.IdTipoHabitacion == dto.IdTipoHabitacion);

            _logger.LogInformation("Recuperando tipo de habitacion con ID: {IdTipoHabitacion}", dto.IdTipoHabitacion);

            if (tipoHabitacion == null)
            {
                _logger.LogWarning("La actualizacion de la habitacion fallo. hotel con ID: {IdHotel} no se encontro", dto.IdHotel);

                throw new KeyNotFoundException("Tipo de habitacion no encontrada");
            }

            habitacion.Hotel = hotel;
            habitacion.TipoHabitacion = tipoHabitacion;

            mapper.Map(dto, habitacion);

            await dbContext.SaveChangesAsync();

            _logger.LogInformation("Habitacion actualizada con exito con ID: {IdHabitacion}", habitacion.IdHabitacion);

            return dto;
        }

        public async Task<AddReservaDto> ReservarHabitacion(AddReservaDto dto)
        {
            var huesped = await dbContext.Huespedes
            .FirstOrDefaultAsync(x => x.IdHuesped == dto.IdHuesped);

            _logger.LogInformation("Recuperando huesped con ID: {IdHuesped}", dto.IdHuesped);

            if (huesped == null)
            {
                _logger.LogWarning("La reservacion de la habitacion fallo. huesped con ID: {IdHuesped}", dto.IdHuesped);

                throw new KeyNotFoundException("huesped no encontrado");
            }

            var habitacion = await dbContext.Habitaciones
                .Include(h => h.Hotel)
                .ThenInclude(h => h.Agente)
                .FirstOrDefaultAsync(x => x.IdHabitacion == dto.IdHabitacion);

            _logger.LogInformation("Recuperando habitacion con ID: {IdHabitacion}", dto.IdHabitacion);

            if (habitacion == null)
            {
                _logger.LogWarning("La reservacion de la habitacion fallo. habitacion con ID: {IdHabitacion} no se encontro", dto.IdHabitacion);

                throw new KeyNotFoundException("habitacion no encontrada");
            }

            List<DestinatarioEmailDto> destinatarios = new List<DestinatarioEmailDto>();

            destinatarios.Add(new DestinatarioEmailDto
            {
                Nombre = huesped.Nombres + huesped.Apellidos,
                Email = huesped.CorreoElectronico
            });

            destinatarios.Add(new DestinatarioEmailDto
            {
                Nombre = habitacion.Hotel.Agente.Nombre + habitacion.Hotel.Agente.Apellido,
                Email = habitacion.Hotel.Agente.CorreoElectronico
            });

            _logger.LogInformation($"Iniciando el envio de correo electronico");

            await emailService.EnviarEmail(destinatarios, configuration["Mail:SubjectConfirmation"]!, configuration["Mail:BodyConfirmation"]!);

            _logger.LogInformation($"Enviando el correo electronico");

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

        public async Task<bool> DesabilitarHabitacionAsync(int id)
        {
            var habitacion = await dbContext.Habitaciones.FirstOrDefaultAsync(x => x.IdHabitacion == id);

            _logger.LogInformation("Recuperando habitacion con ID: {IdHabitacion}", id);

            if (habitacion == null)
            {
                _logger.LogWarning("La desabilitacion de la habitacion fallo. habitacion con ID: {IdHabitacion} no se encontro", id);

                throw new KeyNotFoundException("habitacion no encontrada");
            }

            habitacion.Desabilitar();

            _logger.LogInformation("Desabilitando habitacion con ID: {IdHabitacion}", id);

            return await dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> HabilitarHabitacionAsync(int id)
        {
            var habitacion = await dbContext.Habitaciones.FirstOrDefaultAsync(x => x.IdHabitacion == id);

            _logger.LogInformation("Recuperando habitacion con ID: {IdHabitacion}", id);

            if (habitacion == null)
            {
                _logger.LogWarning("La habilitacion de la habitacion fallo. habitacion con ID: {IdHabitacion} no se encontro", id);

                throw new KeyNotFoundException("habitacion no encontrada");
            }

            habitacion.Habilitar();

            _logger.LogInformation("Habilitando habitacion con ID: {IdHabitacion}", id);

            return await dbContext.SaveChangesAsync() > 0;
        }
    }
}
