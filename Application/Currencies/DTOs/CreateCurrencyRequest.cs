namespace UsersApi.Application.Currencies.DTOs;

public record CreateCurrencyRequest(
    string Code,
    string Name,
    decimal RateToBase
);