using FluentValidation;

namespace UsersApi.Application.Currencies;

public class ConvertCurrencyValidator
    : AbstractValidator<ConvertCurrencyQuery>
{
    public ConvertCurrencyValidator()
    {
        RuleFor(x => x.Request.Amount)
            .GreaterThan(0)
            .WithMessage("El monto debe ser mayor que cero.");

        RuleFor(x => x.Request.FromCurrency)
            .NotEmpty()
            .WithMessage("La moneda origen es obligatoria.")
            .Length(3)
            .WithMessage("La moneda origen debe tener 3 caracteres.");

        RuleFor(x => x.Request.ToCurrency)
            .NotEmpty()
            .WithMessage("La moneda destino es obligatoria.")
            .Length(3)
            .WithMessage("La moneda destino debe tener 3 caracteres.");
    }
}