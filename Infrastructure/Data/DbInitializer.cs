using Microsoft.EntityFrameworkCore;
using UsersApi.Domain.Entities;

namespace UsersApi.Infrastructure.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(
        IDbContextFactory<AppDbContext> contextFactory)
    {
        await using var context =
            await contextFactory.CreateDbContextAsync();

        await context.Database.MigrateAsync();

        if (await context.Currencies.AnyAsync())
        {
            return;
        }

        var currencies = new[]
        {
            new Currency
            {
                Code = "PYG",
                Name = "Guaraní Paraguayo",
                RateToBase = 1m
            },
            new Currency
            {
                Code = "USD",
                Name = "Dólar Estadounidense",
                RateToBase = 7300m
            },
            new Currency
            {
                Code = "EUR",
                Name = "Euro",
                RateToBase = 8500m
            },
            new Currency
            {
                Code = "BRL",
                Name = "Real Brasileño",
                RateToBase = 1400m
            }
        };

        await context.Currencies.AddRangeAsync(currencies);

        await context.SaveChangesAsync();
    }
}