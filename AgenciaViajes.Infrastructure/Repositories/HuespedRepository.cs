using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PasswordHashing;
using AgenciaViajes.Application.Dto.Habitacion;
using AgenciaViajes.Application.Dto.Usuario;
using AgenciaViajes.Application.Interfaces.Repositories;
using AgenciaViajes.Application.Interfaces.Services;
using AgenciaViajes.Domain.Entities;
using AgenciaViajes.Infrastructure.Data;

namespace AgenciaViajes.Infrastructure.Repositories
{
    public class HuespedRepository(AppDbContext dbContext, IMapper mapper,
    ITokenService tokenService, ILogger<HuespedRepository> _logger, IGeneroRepository generoRepository,
    ITipoDocumentoRepository tipoDocumentoRepository) : IHuespedRepository
    {
        public async Task<AddHuespedDto> AddHuespedAsync(AddHuespedDto dto)
        {
            var tipoDocumento = await tipoDocumentoRepository.ObtenerTipoDocumentoPorIdAsync(dto.IdTipoDocumento);

            var genero = await generoRepository.ObtenerGeneroPorIdAsync(dto.IdGenero);

            var huesped = mapper.Map<Huesped>(dto);

            huesped.Contraseña = PasswordHasher.Hash(dto.Contraseña);
            huesped.TipoDocumento = tipoDocumento;
            huesped.Genero = genero;

            dbContext.Add(huesped);
            await dbContext.SaveChangesAsync();

            _logger.LogInformation("Huesped creado con exito con ID: {IdGenero}", huesped.IdGenero);

            return dto;
        }

        public async Task<string> LoginHuespedAsync(LoginDto dto)
        {
            var huesped = await dbContext.Huespedes.AsNoTracking()
            .FirstOrDefaultAsync(x => x.CorreoElectronico == dto.CorreoElectronico);

            _logger.LogInformation("Recuperando huesped con correo electronico: {CorreoElectronico}", 
            dto.CorreoElectronico);

            if (huesped != null && PasswordHasher.Validate(dto.Contraseña, huesped.Contraseña))
            {
                _logger.LogInformation("Creando token para el huesped: {Nombres} {Apellidos}", huesped.Nombres,
                huesped.Apellidos);

                return tokenService.Create(huesped);
            }

            _logger.LogWarning("La autenticacion del huesped fallo. usuario o clave no validos");

            throw new KeyNotFoundException("usuario o clave no validos");
        }

        public async Task<Huesped> ObtenerHuespedPorIdAsync(int id)
        {
            var huesped = await dbContext.Huespedes
            .FirstOrDefaultAsync(x => x.IdHuesped == id);

            _logger.LogInformation("Recuperando huesped con ID: {IdHuesped}", id);

            if (huesped == null)
            {
                _logger.LogWarning("La recuperacion del huesped fallo con ID: {IdHuesped} no se encontro", id);

                throw new KeyNotFoundException("huesped no encontrado");
            }

            return huesped;
        }
    }
}
