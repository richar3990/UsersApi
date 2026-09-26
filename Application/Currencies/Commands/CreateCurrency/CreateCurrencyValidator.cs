using FluentValidation;

namespace UsersApi.Application.Currencies.Commands.CreateCurrency;

public class CreateCurrencyValidator
    : AbstractValidator<CreateCurrencyCommand>
{
    public CreateCurrencyValidator()
    {
        RuleFor(x => x.Request.Code)
            .NotEmpty()
            .WithMessage("El código de moneda es obligatorio.")
            .Length(3)
            .WithMessage("El código de moneda debe tener 3 caracteres.");

        RuleFor(x => x.Request.Name)
            .NotEmpty()
            .WithMessage("El nombre de la moneda es obligatorio.")
            .MaximumLength(100)
            .WithMessage("El nombre no puede superar los 100 caracteres.");

        RuleFor(x => x.Request.RateToBase)
            .GreaterThan(0)
            .WithMessage("RateToBase debe ser mayor que cero.");
    }
}