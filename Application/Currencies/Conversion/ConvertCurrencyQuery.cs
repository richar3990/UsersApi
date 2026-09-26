using UsersApi.Application.Currencies.DTOs;

namespace UsersApi.Application.Currencies;

public record ConvertCurrencyQuery(
    ConvertCurrencyRequest Request
);