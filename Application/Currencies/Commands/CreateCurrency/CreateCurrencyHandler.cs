using Microsoft.EntityFrameworkCore;
using UsersApi.Application.Currencies.DTOs;
using UsersApi.Domain.Entities;
using UsersApi.Infrastructure.Data;

namespace UsersApi.Application.Currencies.Commands.CreateCurrency;

public class CreateCurrencyHandler
{
    private readonly IDbContextFactory<AppDbContext> _contextFactory;

    public CreateCurrencyHandler(
        IDbContextFactory<AppDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<CurrencyResponse> HandleAsync(
        CreateCurrencyCommand command)
    {
        await using var context =
            await _contextFactory.CreateDbContextAsync();

        var code = command.Request.Code
            .Trim()
            .ToUpperInvariant();

        var exists = await context.Currencies
            .AnyAsync(x => x.Code == code);

        if (exists)
        {
            throw new InvalidOperationException(
                "Ya existe una moneda con ese código.");
        }

        var currency = new Currency
        {
            Code = code,
            Name = command.Request.Name.Trim(),
            RateToBase = command.Request.RateToBase
        };

        context.Currencies.Add(currency);

        await context.SaveChangesAsync();

        return new CurrencyResponse(
            currency.Id,
            currency.Code,
            currency.Name,
            currency.RateToBase);
    }
}