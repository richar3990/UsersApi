using Microsoft.EntityFrameworkCore;
using UsersApi.Application.Currencies.DTOs;
using UsersApi.Infrastructure.Data;

namespace UsersApi.Application.Currencies;

public class ConvertCurrencyHandler
{
    private readonly IDbContextFactory<AppDbContext> _contextFactory;

    public ConvertCurrencyHandler(
        IDbContextFactory<AppDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<ConvertCurrencyResponse?> HandleAsync(
        ConvertCurrencyQuery query)
    {
        var fromCode = query.Request.FromCurrency
            .Trim()
            .ToUpperInvariant();

        var toCode = query.Request.ToCurrency
            .Trim()
            .ToUpperInvariant();

        var fromTask = GetCurrencyAsync(fromCode);
        var toTask = GetCurrencyAsync(toCode);

        await Task.WhenAll(fromTask, toTask);

        var fromCurrency = await fromTask;
        var toCurrency = await toTask;

        if (fromCurrency is null || toCurrency is null)
        {
            return null;
        }

        var amountInBase =
            query.Request.Amount * fromCurrency.RateToBase;

        var convertedAmount =
            amountInBase / toCurrency.RateToBase;

        return new ConvertCurrencyResponse(
            query.Request.Amount,
            fromCurrency.Code,
            toCurrency.Code,
            Math.Round(convertedAmount, 6));
    }

    private async Task<Domain.Entities.Currency?> GetCurrencyAsync(
        string code)
    {
        await using var context =
            await _contextFactory.CreateDbContextAsync();

        return await context.Currencies
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Code == code);
    }
}