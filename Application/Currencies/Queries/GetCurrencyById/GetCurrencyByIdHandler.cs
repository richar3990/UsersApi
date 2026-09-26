using Microsoft.EntityFrameworkCore;
using UsersApi.Application.Currencies.DTOs;
using UsersApi.Infrastructure.Data;

namespace UsersApi.Application.Currencies.Queries.GetCurrencyById;

public class GetCurrencyByIdHandler
{
    private readonly IDbContextFactory<AppDbContext> _contextFactory;

    public GetCurrencyByIdHandler(
        IDbContextFactory<AppDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<CurrencyResponse?> HandleAsync(
        GetCurrencyByIdQuery query)
    {
        await using var context =
            await _contextFactory.CreateDbContextAsync();

        return await context.Currencies
            .AsNoTracking()
            .Where(x => x.Id == query.Id)
            .Select(x => new CurrencyResponse(
                x.Id,
                x.Code,
                x.Name,
                x.RateToBase))
            .FirstOrDefaultAsync();
    }
}