using Microsoft.EntityFrameworkCore;
using UsersApi.Application.Addresses.DTOs;
using UsersApi.Infrastructure.Data;

namespace UsersApi.Application.Addresses.Commands.UpdateAddress;

public class UpdateAddressHandler
{
    private readonly IDbContextFactory<AppDbContext> _contextFactory;

    public UpdateAddressHandler(
        IDbContextFactory<AppDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<AddressResponse?> HandleAsync(
        UpdateAddressCommand command)
    {
        await using var context =
            await _contextFactory.CreateDbContextAsync();

        var address = await context.Addresses
            .FirstOrDefaultAsync(x => x.Id == command.Id);

        if (address is null)
        {
            return null;
        }

        address.Street = command.Request.Street.Trim();
        address.City = command.Request.City.Trim();
        address.Country = command.Request.Country.Trim();
        address.ZipCode = string.IsNullOrWhiteSpace(
            command.Request.ZipCode)
            ? null
            : command.Request.ZipCode.Trim();

        await context.SaveChangesAsync();

        return new AddressResponse(
            address.Id,
            address.UserId,
            address.Street,
            address.City,
            address.Country,
            address.ZipCode);
    }
}