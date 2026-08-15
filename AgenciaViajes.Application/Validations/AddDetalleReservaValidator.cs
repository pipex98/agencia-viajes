using FluentValidation;
using AgenciaViajes.Application.Dto.DetalleReserva;

namespace AgenciaViajes.Application.Validations
{
    public class AddDetalleReservaValidator : AbstractValidator<AddDetalleReservaDto>
    {
        public AddDetalleReservaValidator()
        {
            RuleFor(q => q.Concepto)
            .NotEmpty()
            .WithMessage("{PropertyName} es requerido");

            RuleFor(q => q.Cantidad)
            .GreaterThan(0)
            .WithMessage("{PropertyName} debe ser mayor que 0.");

            RuleFor(p => p.PrecioUnitario)
            .GreaterThan(0)
            .WithMessage("{PropertyName} debe ser mayor que 0.")
            .PrecisionScale(10, 0, false)
            .WithMessage("{PropertyName} no puede tener más de 10 dígitos en total");
        }
    }
}
