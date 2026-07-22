using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AgenciaViajes.Application.Dto.Hotel;
using AgenciaViajes.Application.Interfaces.Repositories;
using AgenciaViajes.Domain.Entities;
using AgenciaViajes.Infrastructure.Data;

namespace AgenciaViajes.Infrastructure.Repositories
{
    public class HotelRepository(AppDbContext dbContext, IMapper mapper, ILogger<IHotelRepository> _logger) : IHotelRepository
    {
        public async Task<UpsertHotelDto> AddHotelAsync(UpsertHotelDto dto)
        {
            var agente = await dbContext.Agentes
            .FirstOrDefaultAsync(x => x.IdAgente == dto.IdAgente);

            _logger.LogInformation("Recuperando huesped con ID: {IdAgente}", dto.IdAgente);

            if (agente == null)
            {
                _logger.LogWarning("La insercion del hotel fallo. agente con ID: {IdAgente} no se encontro", dto.IdAgente);

                throw new KeyNotFoundException("Agente no encontrado");
            }

            var ciudad = await dbContext.Ciudades
            .FirstOrDefaultAsync(x => x.Id == dto.IdCiudad);

            _logger.LogInformation("Recuperando ciudad con ID: {IdCiudad}", dto.IdCiudad);

            if (ciudad == null)
            {
                _logger.LogWarning("La insercion del hotel fallo. ciudad con ID: {IdCiudad} no se encontro", dto.IdCiudad);

                throw new KeyNotFoundException("Curso no encontrado");
            }

            var hotel = mapper.Map<Hotel>(dto);

            hotel.Agente = agente;
            hotel.Ciudad = ciudad;

            dbContext.Add(hotel);
            await dbContext.SaveChangesAsync();

            _logger.LogInformation("Hotel creado con exito con ID: {IdHotel}", hotel.IdHotel);

            return dto;
        }

        public async Task<UpsertHotelDto> UpdateHotelAsync(int id, UpsertHotelDto dto)
        {
            var hotel = await dbContext.Hoteles
            .FirstOrDefaultAsync(x => x.IdHotel == id);

            _logger.LogInformation("Recuperando hotel con ID: {IdHotel}", id);

            if (hotel == null)
            {
                _logger.LogWarning("La actualizacion del hotel fallo. hotel con ID: {IdHotel} no se encontro", id);

                throw new KeyNotFoundException("Hotel no encontrado");
            }

            var agente = await dbContext.Agentes
            .FirstOrDefaultAsync(x => x.IdAgente == dto.IdAgente);

            _logger.LogInformation("Recuperando agente con ID: {IdAgente}", id);

            if (agente == null)
            {
                _logger.LogWarning("La actualizacion del hotel fallo. agente con ID: {IdAgente} no se encontro", dto.IdAgente);

                throw new KeyNotFoundException("Agente no encontrado");
            }

            var ciudad = await dbContext.Ciudades
            .FirstOrDefaultAsync(x => x.Id == dto.IdCiudad);

            _logger.LogInformation("Recuperando ciudad con ID: {IdCiudad}", dto.IdCiudad);

            if (ciudad == null)
            {
                _logger.LogWarning("La actualizacion del hotel fallo. ciudad con ID: {IdCiudad} no se encontro", dto.IdCiudad);

                throw new KeyNotFoundException("Ciudad no encontrada");
            }

            hotel.Agente = agente;
            hotel.Ciudad = ciudad;

            mapper.Map(dto, hotel);

            await dbContext.SaveChangesAsync();

            _logger.LogInformation("Hotel actualizado con exito con ID: {IdHotel}", hotel.IdHotel);

            return dto;
        }

        public async Task<bool> DesabilitarHotelAsync(int id)
        {
            var hotel = await dbContext.Hoteles.FirstOrDefaultAsync(x => x.IdHotel == id);

            _logger.LogInformation($"Recuperando hotel con ID: {id}");

            if (hotel == null)
            {
                _logger.LogWarning("La desabilitacion del hotel fallo. hotel con ID: {IdHotel} no se encontro", id);

                throw new KeyNotFoundException("Hotel no encontrado");
            }

            hotel.Desabilitar();

            _logger.LogInformation("Desabilitando hotel con ID: {IdHotel}", id);

            return await dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> HabilitarHotelAsync(int id)
        {
            var hotel = await dbContext.Hoteles.FirstOrDefaultAsync(x => x.IdHotel == id);

            _logger.LogInformation("Recuperando hotel con ID: {IdHotel}", id);

            if (hotel == null)
            {
                _logger.LogWarning("La habilitacion del hotel fallo. hotel con ID: {IdHotel} no se encontro", id);

                throw new KeyNotFoundException("Hotel no encontrado");
            }

            hotel.Habilitar();

            _logger.LogInformation("Habilitando hotel con ID: {IdHotel}", id);

            return await dbContext.SaveChangesAsync() > 0;
        }

        public async Task<UpsertHabitacionDto> AssignHabitacionAsync(UpsertHabitacionDto dto)
        {
            var hotel = await dbContext.Hoteles.FirstOrDefaultAsync(x => x.IdHotel == dto.IdHotel);

            _logger.LogInformation("Recuperando hotel con ID: {IdHotel}", dto.IdHotel);

            if (hotel == null)
            {
                _logger.LogWarning("La insercion de la habitacion fallo. hotel con ID: {IdHotel} no se encontro", dto.IdHotel);

                throw new KeyNotFoundException("Hotel no encontrado");
            }

            var tipoHabitacion = await dbContext.TipoHabitaciones
            .FirstOrDefaultAsync(x => x.IdTipoHabitacion == dto.IdTipoHabitacion);

            _logger.LogInformation("Recuperando tipo de habitacion con ID: {IdTipoHabitacion}", dto.IdTipoHabitacion);

            if (tipoHabitacion == null)
            {
                _logger.LogWarning("La insercion de la habitacion fallo. tipo de habitacion con ID: {IdTipoHabitacion} no se encontro", dto.IdTipoHabitacion);

                throw new KeyNotFoundException("Tipo de habitacion no encontrada");
            }

            var habitacion = mapper.Map<Habitacion>(dto);

            habitacion.Hotel = hotel;
            habitacion.TipoHabitacion = tipoHabitacion;

            dbContext.Add(habitacion);
            await dbContext.SaveChangesAsync();

            _logger.LogInformation("Hotel guardado con exito con ID: {IdHotel}", hotel.IdHotel);

            return dto;
        }
    }
}
