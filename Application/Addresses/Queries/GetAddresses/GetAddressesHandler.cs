using Microsoft.EntityFrameworkCore;
using UsersApi.Application.Addresses.DTOs;
using UsersApi.Infrastructure.Data;

namespace UsersApi.Application.Addresses.Queries.GetAddresses;

public class GetAddressesHandler
{
    private readonly IDbContextFactory<AppDbContext> _contextFactory;

    public GetAddressesHandler(
        IDbContextFactory<AppDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<List<AddressResponse>> HandleAsync(
        GetAddressesQuery query)
    {
        await using var context =
            await _contextFactory.CreateDbContextAsync();

        return await context.Addresses
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .Select(x => new AddressResponse(
                x.Id,
                x.UserId,
                x.Street,
                x.City,
                x.Country,
                x.ZipCode))
            .ToListAsync();
    }
}