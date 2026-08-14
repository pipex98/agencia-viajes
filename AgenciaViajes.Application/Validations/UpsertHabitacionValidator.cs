using FluentValidation;
using AgenciaViajes.Application.Dto.Hotel;

namespace AgenciaViajes.Application.Validations
{
    public class UpsertHabitacionValidator : AbstractValidator<UpsertHabitacionDto>
    {
        public UpsertHabitacionValidator()
        {
            RuleFor(q => q.IdHotel)
                .GreaterThan(0)
                .WithMessage("{PropertyName} debe ser mayor que 0.");

            RuleFor(q => q.IdTipoHabitacion)
                .GreaterThan(0)
                .WithMessage("{PropertyName} debe ser mayor que 0.");

            RuleFor(p => p.CostoBase)
            .GreaterThan(0)
            .WithMessage("{PropertyName} debe ser mayor a 0.0")
            .PrecisionScale(10, 2, false)
            .WithMessage("{PropertyName} no puede tener más de 10 dígitos en total, con un máximo de 2 decimales.");

            RuleFor(q => q.Impuestos)
            .GreaterThan(0)
            .WithMessage("{PropertyName} debe ser mayor a 0.0")
            .PrecisionScale(5, 2, false)
            .WithMessage("{PropertyName} no puede tener más de 5 dígitos en total, con un máximo de 2 decimales.");

            RuleFor(q => q.CantidadHuespedes)
                .GreaterThan(0)
                .WithMessage("{PropertyName} debe ser mayor que 0.");

            RuleFor(q => q.Ubicacion)
                .NotEmpty()
                .WithMessage("{PropertyName} es requerido")
                .MaximumLength(50)
                .WithMessage("{PropertyName} no puede exceder los 50 caracteres");
        }
    }
}
