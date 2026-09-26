using Microsoft.EntityFrameworkCore;
using UsersApi.Infrastructure.Data;

namespace UsersApi.Application.Currencies.Commands.DeleteCurrency;

public class DeleteCurrencyHandler
{
    private readonly IDbContextFactory<AppDbContext> _contextFactory;

    public DeleteCurrencyHandler(
        IDbContextFactory<AppDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<bool> HandleAsync(
        DeleteCurrencyCommand command)
    {
        await using var context =
            await _contextFactory.CreateDbContextAsync();

        var currency = await context.Currencies
            .FirstOrDefaultAsync(x => x.Id == command.Id);

        if (currency is null)
        {
            return false;
        }

        context.Currencies.Remove(currency);

        await context.SaveChangesAsync();

        return true;
    }
}