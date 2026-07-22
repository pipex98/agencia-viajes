using FluentValidation;
using AgenciaViajes.Application.Dto.Hotel;

namespace AgenciaViajes.Application.Validations
{
    public class UpsertHotelValidator : AbstractValidator<UpsertHotelDto>
    {
        public UpsertHotelValidator()
        {
            RuleFor(q => q.IdAgente)
                .GreaterThan(0)
                .WithMessage("{PropertyName} debe ser mayor que 0.");

            RuleFor(q => q.IdCiudad)
                .GreaterThan(0)
                .WithMessage("{PropertyName} debe ser mayor que 0.");

            RuleFor(q => q.Nombre)
                .NotEmpty()
                .WithMessage("{PropertyName} es requerido")
                .MaximumLength(150)
                .WithMessage("{PropertyName} no puede exceder los 150 caracteres");

            RuleFor(q => q.Direccion)
                .NotEmpty()
                .WithMessage("{PropertyName} es requerido")
                .MaximumLength(255)
                .WithMessage("{PropertyName} no puede exceder los 255 caracteres");

        }
    }
}
