using Microsoft.EntityFrameworkCore;
using UsersApi.Application.Addresses.DTOs;
using UsersApi.Infrastructure.Data;

namespace UsersApi.Application.Addresses.Queries.GetAddressById;

public class GetAddressByIdHandler
{
    private readonly IDbContextFactory<AppDbContext> _contextFactory;

    public GetAddressByIdHandler(
        IDbContextFactory<AppDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<AddressResponse?> HandleAsync(
        GetAddressByIdQuery query)
    {
        await using var context =
            await _contextFactory.CreateDbContextAsync();

        return await context.Addresses
            .AsNoTracking()
            .Where(x => x.Id == query.Id)
            .Select(x => new AddressResponse(
                x.Id,
                x.UserId,
                x.Street,
                x.City,
                x.Country,
                x.ZipCode))
            .FirstOrDefaultAsync();
    }
}