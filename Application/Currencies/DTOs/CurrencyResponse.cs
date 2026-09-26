namespace UsersApi.Application.Currencies.DTOs;

public record CurrencyResponse(
    int Id,
    string Code,
    string Name,
    decimal RateToBase
);