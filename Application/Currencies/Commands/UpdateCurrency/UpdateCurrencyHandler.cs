using Microsoft.EntityFrameworkCore;
using UsersApi.Application.Currencies.DTOs;
using UsersApi.Infrastructure.Data;

namespace UsersApi.Application.Currencies.Commands.UpdateCurrency;

public class UpdateCurrencyHandler
{
    private readonly IDbContextFactory<AppDbContext> _contextFactory;

    public UpdateCurrencyHandler(
        IDbContextFactory<AppDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<CurrencyResponse?> HandleAsync(
        UpdateCurrencyCommand command)
    {
        await using var context =
            await _contextFactory.CreateDbContextAsync();

        var currency = await context.Currencies
            .FirstOrDefaultAsync(x => x.Id == command.Id);

        if (currency is null)
        {
            return null;
        }

        var code = command.Request.Code
            .Trim()
            .ToUpperInvariant();

        var duplicate = await context.Currencies
            .AnyAsync(x =>
                x.Code == code &&
                x.Id != command.Id);

        if (duplicate)
        {
            throw new InvalidOperationException(
                "Ya existe otra moneda con ese código.");
        }

        currency.Code = code;
        currency.Name = command.Request.Name.Trim();
        currency.RateToBase = command.Request.RateToBase;

        await context.SaveChangesAsync();

        return new CurrencyResponse(
            currency.Id,
            currency.Code,
            currency.Name,
            currency.RateToBase);
    }
}