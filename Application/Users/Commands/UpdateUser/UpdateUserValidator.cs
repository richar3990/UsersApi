using FluentValidation;

namespace UsersApi.Application.Users.Commands.UpdateUser;

public class UpdateUserValidator
    : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("El id del usuario debe ser mayor que cero.");

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
    }
}