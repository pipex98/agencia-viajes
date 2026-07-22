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
    ITokenService tokenService, ILogger<HuespedRepository> _logger) : IHuespedRepository
    {
        public async Task<AddHuespedDto> AddHuespedAsync(AddHuespedDto dto)
        {
            var tipoDocumento = await dbContext.TipoDocumentos
            .FirstOrDefaultAsync(x => x.IdTipoDocumento == dto.IdTipoDocumento);

            _logger.LogInformation("Recuperando tipo de documento con ID: {IdTipoDocumnto}", dto.IdTipoDocumento);

            if (tipoDocumento == null)
            {
                _logger.LogWarning("La insercion del huesped fallo. tipo de documento con ID: {IdTipoDocumento} no se encontro", dto.IdTipoDocumento);

                throw new KeyNotFoundException("Tipo de documento no encontrado");
            }

            var genero = await dbContext.Generos
            .FirstOrDefaultAsync(x => x.IdGenero == dto.IdGenero);

            _logger.LogInformation("Recuperando genero con ID: {IdGenero}", dto.IdGenero);

            if (genero == null)
            {
                _logger.LogWarning("La insercion del huesped fallo. genero con ID: {IdGenero} no se encontro", dto.IdGenero);

                throw new KeyNotFoundException("Genero no encontrado");
            }

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
            Huesped? huesped = await dbContext.Huespedes.AsNoTracking()
                .FirstOrDefaultAsync(x => x.CorreoElectronico == dto.CorreoElectronico);

            _logger.LogInformation("Recuperando huesped con correo electronico: {CorreoElectronico}", dto.CorreoElectronico);

            if (huesped is null)
            {
                _logger.LogWarning("La autenticacion del huesped fallo. huesped con correo electronico: {CorreoElectronico} no se encontro", dto.CorreoElectronico);

                throw new KeyNotFoundException("El huesped no fue encontrado");
            }

            _logger.LogInformation($"validando contraseña del huesped");

            bool verified = PasswordHasher.Validate(dto.Contraseña, huesped.Contraseña);

            if (!verified)
            {
                _logger.LogWarning($"La validacion de la contraseña fallo. no hizo match");

                throw new KeyNotFoundException("La contraseña es incorrecta");
            }

            _logger.LogInformation("Creando token para el huesped: {Nombres} {Apellidos}", huesped.Nombres,
            huesped.Apellidos);

            return tokenService.Create(huesped);
        }
    }
}
