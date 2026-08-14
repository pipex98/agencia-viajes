using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AgenciaViajes.Application.Dto.Hotel;
using AgenciaViajes.Application.Interfaces.Repositories;
using AgenciaViajes.Domain.Entities;
using AgenciaViajes.Infrastructure.Data;

namespace AgenciaViajes.Infrastructure.Repositories
{
    public class HotelRepository(AppDbContext dbContext, IMapper mapper, ILogger<IHotelRepository> _logger,
    ITipoHabitacionRepository tipoHabitacionRepository, IAgenteRepository agenteRepository, 
    ICiudadRepository ciudadRepository) : IHotelRepository
    {
        public async Task<UpsertHotelDto> AddHotelAsync(UpsertHotelDto dto)
        {
            var agente = await agenteRepository.ObtenerAgentePorId(dto.IdAgente);

            var ciudad = await ciudadRepository.ObtenerCiudadPorId(dto.IdCiudad);

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
            var hotel = await this.ObtenerHotelPorId(id);

            var agente = await agenteRepository.ObtenerAgentePorId(dto.IdAgente);

            var ciudad = await ciudadRepository.ObtenerCiudadPorId(dto.IdCiudad);

            hotel.Agente = agente;
            hotel.Ciudad = ciudad;

            mapper.Map(dto, hotel);

            await dbContext.SaveChangesAsync();

            _logger.LogInformation("Hotel actualizado con exito con ID: {IdHotel}", hotel.IdHotel);

            return dto;
        }

        public async Task<bool> DesabilitarHotelAsync(int id)
        {
            var hotel = await this.ObtenerHotelPorId(id);

            hotel.Desabilitar();

            _logger.LogInformation("Desabilitando hotel con ID: {IdHotel}", id);

            return await dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> HabilitarHotelAsync(int id)
        {
            var hotel = await this.ObtenerHotelPorId(id);

            hotel.Habilitar();

            _logger.LogInformation("Habilitando hotel con ID: {IdHotel}", id);

            return await dbContext.SaveChangesAsync() > 0;
        }

        public async Task<UpsertHabitacionDto> AssignHabitacionAsync(UpsertHabitacionDto dto)
        {
            var hotel = await this.ObtenerHotelPorId(dto.IdHotel);

            var tipoHabitacion = await tipoHabitacionRepository.ObtenerTipoHabitacionPorIdAsync(dto.IdHotel);

            var habitacion = mapper.Map<Habitacion>(dto);

            habitacion.Hotel = hotel;
            habitacion.TipoHabitacion = tipoHabitacion;

            dbContext.Add(habitacion);
            await dbContext.SaveChangesAsync();

            _logger.LogInformation("Hotel guardado con exito con ID: {IdHotel}", hotel.IdHotel);

            return dto;
        }

        public async Task<Hotel> ObtenerHotelPorId(int id)
        {
            var hotel = await dbContext.Hoteles.FirstOrDefaultAsync(x => x.IdHotel == id);

            _logger.LogInformation("Recuperando hotel con ID: {IdHotel}", id);

            if (hotel == null)
            {
                _logger.LogWarning("La recuperacion del hotel fallo con ID: {IdHotel} no se encontro", id);

                throw new KeyNotFoundException("Hotel no encontrado");
            }

            return hotel;
        }
    }
}
