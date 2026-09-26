using FluentValidation;

namespace UsersApi.Application.Addresses.Commands.CreateAddress;

public class CreateAddressValidator
    : AbstractValidator<CreateAddressCommand>
{
    public CreateAddressValidator()
    {
        RuleFor(x => x.Request.UserId)
            .GreaterThan(0)
            .WithMessage("El usuario es obligatorio.");

        RuleFor(x => x.Request.Street)
            .NotEmpty()
            .WithMessage("La calle es obligatoria.")
            .MaximumLength(250)
            .WithMessage("La calle no puede superar los 250 caracteres.");

        RuleFor(x => x.Request.City)
            .NotEmpty()
            .WithMessage("La ciudad es obligatoria.")
            .MaximumLength(100)
            .WithMessage("La ciudad no puede superar los 100 caracteres.");

        RuleFor(x => x.Request.Country)
            .NotEmpty()
            .WithMessage("El país es obligatorio.")
            .MaximumLength(100)
            .WithMessage("El país no puede superar los 100 caracteres.");

        RuleFor(x => x.Request.ZipCode)
            .MaximumLength(20)
            .When(x => !string.IsNullOrWhiteSpace(x.Request.ZipCode))
            .WithMessage("El código postal no puede superar los 20 caracteres.");
    }
}