using Microsoft.EntityFrameworkCore;
using UsersApi.Application.Addresses.DTOs;
using UsersApi.Domain.Entities;
using UsersApi.Infrastructure.Data;

namespace UsersApi.Application.Addresses.Commands.CreateAddress;

public class CreateAddressHandler
{
    private readonly IDbContextFactory<AppDbContext> _contextFactory;

    public CreateAddressHandler(
        IDbContextFactory<AppDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<AddressResponse?> HandleAsync(
        CreateAddressCommand command)
    {
        await using var context =
            await _contextFactory.CreateDbContextAsync();

        var userExists = await context.Users
            .AnyAsync(x => x.Id == command.Request.UserId);

        if (!userExists)
        {
            return null;
        }

        var address = new Address
        {
            UserId = command.Request.UserId,
            Street = command.Request.Street.Trim(),
            City = command.Request.City.Trim(),
            Country = command.Request.Country.Trim(),
            ZipCode = string.IsNullOrWhiteSpace(command.Request.ZipCode)
                ? null
                : command.Request.ZipCode.Trim()
        };

        context.Addresses.Add(address);

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