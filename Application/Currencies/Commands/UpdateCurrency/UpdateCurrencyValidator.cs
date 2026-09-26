using FluentValidation;

namespace UsersApi.Application.Currencies.Commands.UpdateCurrency;

public class UpdateCurrencyValidator
    : AbstractValidator<UpdateCurrencyCommand>
{
    public UpdateCurrencyValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("El id debe ser mayor que cero.");

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