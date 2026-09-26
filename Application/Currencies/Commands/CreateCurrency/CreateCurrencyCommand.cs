using UsersApi.Application.Currencies.DTOs;

namespace UsersApi.Application.Currencies.Commands.CreateCurrency;

public record CreateCurrencyCommand(
    CreateCurrencyRequest Request
);