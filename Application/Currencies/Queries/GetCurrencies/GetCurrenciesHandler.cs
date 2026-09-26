using Microsoft.EntityFrameworkCore;
using UsersApi.Application.Currencies.DTOs;
using UsersApi.Infrastructure.Data;

namespace UsersApi.Application.Currencies.Queries.GetCurrencies;

public class GetCurrenciesHandler
{
    private readonly IDbContextFactory<AppDbContext> _contextFactory;

    public GetCurrenciesHandler(
        IDbContextFactory<AppDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<List<CurrencyResponse>> HandleAsync(
        GetCurrenciesQuery query)
    {
        await using var context =
            await _contextFactory.CreateDbContextAsync();

        return await context.Currencies
            .AsNoTracking()
            .OrderBy(x => x.Code)
            .Select(x => new CurrencyResponse(
                x.Id,
                x.Code,
                x.Name,
                x.RateToBase))
            .ToListAsync();
    }
}