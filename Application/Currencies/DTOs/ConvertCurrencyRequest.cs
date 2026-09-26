namespace UsersApi.Application.Currencies.DTOs;

public record ConvertCurrencyRequest(
    decimal Amount,
    string FromCurrency,
    string ToCurrency
);