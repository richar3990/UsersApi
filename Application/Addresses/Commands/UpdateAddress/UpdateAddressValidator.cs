using FluentValidation;

namespace UsersApi.Application.Addresses.Commands.UpdateAddress;

public class UpdateAddressValidator
    : AbstractValidator<UpdateAddressCommand>
{
    public UpdateAddressValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("El id de la dirección debe ser mayor que cero.");

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