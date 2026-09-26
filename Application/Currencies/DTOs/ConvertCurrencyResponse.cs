namespace UsersApi.Application.Currencies.DTOs;

public record ConvertCurrencyResponse(
    decimal Amount,
    string FromCurrency,
    string ToCurrency,
    decimal ConvertedAmount
);