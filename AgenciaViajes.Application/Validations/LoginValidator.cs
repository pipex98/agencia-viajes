using FluentValidation;
using AgenciaViajes.Application.Dto.Usuario;

namespace AgenciaViajes.Application.Validations
{
    public class LoginValidator : AbstractValidator<LoginDto>
    {
        public LoginValidator()
        {
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
        }
    }
}
