namespace UsersApi.Application.Currencies.DTOs;

public record UpdateCurrencyRequest(
    string Code,
    string Name,
    decimal RateToBase
);