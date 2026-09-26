using UsersApi.Application.Currencies.DTOs;

namespace UsersApi.Application.Currencies.Commands.UpdateCurrency;

public record UpdateCurrencyCommand(
    int Id,
    UpdateCurrencyRequest Request
);