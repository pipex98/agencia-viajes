using AgenciaViajes.Application.Dto.Reserva;
using FluentValidation;

namespace AgenciaViajes.Application.Validations
{
    public class AddReservaValidator : AbstractValidator<AddReservaDto>
    {
        public AddReservaValidator()
        {
            RuleFor(q => q.IdHuesped)
            .GreaterThan(0)
            .WithMessage("{PropertyName} debe ser mayor que 0");

            RuleFor(q => q.IdHabitacion)
            .GreaterThan(0)
            .WithMessage("{PropertyName} debe ser mayor que 0");

            RuleFor(x => x.FechaIngreso)
            .NotEmpty()
            .WithMessage("{PropertyName} es requerida.")
            .GreaterThanOrEqualTo(DateTime.Today)
            .WithMessage("{PropertyName} no puede ser una fecha pasada.");

            RuleFor(x => x.FechaSalida)
            .NotEmpty()
            .WithMessage("{PropertyName} es requerida.")
            .GreaterThan(x => x.FechaIngreso)
            .WithMessage("{PropertyName} debe ser posterior a la fecha de ingreso.");

            RuleFor(q => q.ContactoEmergencia)
            .NotEmpty()
            .WithMessage("{PropertyName} es requerido")
            .MaximumLength(255)
            .WithMessage("{PropertyName} no puede exceder los 255 caracteres");
        }
    }
}
