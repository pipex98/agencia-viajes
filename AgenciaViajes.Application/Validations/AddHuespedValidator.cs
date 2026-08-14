using FluentValidation;
using AgenciaViajes.Application.Dto.Habitacion;

namespace AgenciaViajes.Application.Validations
{
    public class AddHuespedValidator : AbstractValidator<AddHuespedDto>
    {
        public AddHuespedValidator()
        {
            RuleFor(q => q.IdTipoDocumento)
                .GreaterThan(0)
                .WithMessage("{PropertyName} debe ser mayor que 0.");

            RuleFor(q => q.IdGenero)
                .GreaterThan(0)
                .WithMessage("{PropertyName} debe ser mayor que 0.");

            RuleFor(q => q.Nombres)
                .NotEmpty()
                .WithMessage("{PropertyName} es requerido")
                .MaximumLength(100)
                .WithMessage("{PropertyName} no puede exceder los 100 caracteres");

            RuleFor(q => q.Apellidos)
                .NotEmpty()
                .WithMessage("{PropertyName} es requerido")
                .MaximumLength(100)
                .WithMessage("{PropertyName} no puede exceder los 100 caracteres");

            RuleFor(e => e.FechaNacimiento)
                .NotEmpty()
                .WithMessage("{PropertyName} es requerida.");

            RuleFor(q => q.NumeroDocumento)
                .NotEmpty()
                .WithMessage("{PropertyName} es requerido")
                .MaximumLength(20)
                .WithMessage("{PropertyName} no puede exceder los 20 caracteres");

            RuleFor(q => q.CorreoElectronico)
                .NotEmpty()
                .WithMessage("{PropertyName} es requerido")
                .MaximumLength(150)
                .WithMessage("{PropertyName} no puede exceder los 150 caracteres");

            RuleFor(q => q.Contraseña)
                .NotEmpty()
                .WithMessage("{PropertyName} es requerido")
                .MaximumLength(255)
                .WithMessage("{PropertyName} no puede exceder los 255 caracteres");

            RuleFor(q => q.Telefono)
                .NotEmpty()
                .WithMessage("{PropertyName} es requerido")
                .MaximumLength(20)
                .WithMessage("{PropertyName} no puede exceder los 20 caracteres");
        }
    }
}
