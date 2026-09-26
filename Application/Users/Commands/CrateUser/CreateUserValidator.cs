using FluentValidation;

namespace UsersApi.Application.Users.Commands.CreateUser;
public class CreateUserValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.Request.Name)
            .NotEmpty()
            .WithMessage("El nombre es obligatorio.")
            .MaximumLength(150)
            .WithMessage("El nombre no puede superar los 150 caracteres.");

        RuleFor(x => x.Request.Email)
            .NotEmpty()
            .WithMessage("El email es obligatorio.")
            .EmailAddress()
            .WithMessage("El email no es válido.")
            .MaximumLength(255)
            .WithMessage("El email no puede superar los 255 caracteres.");

        RuleFor(x => x.Request.Password)
            .MinimumLength(6)
            .When(x => !string.IsNullOrWhiteSpace(x.Request.Password))
            .WithMessage("La contraseña debe tener al menos 6 caracteres.");
    }
}