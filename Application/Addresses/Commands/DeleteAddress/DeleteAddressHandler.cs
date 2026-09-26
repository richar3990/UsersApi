using Microsoft.EntityFrameworkCore;
using UsersApi.Infrastructure.Data;

namespace UsersApi.Application.Addresses.Commands.DeleteAddress;

public class DeleteAddressHandler
{
    private readonly IDbContextFactory<AppDbContext> _contextFactory;

    public DeleteAddressHandler(
        IDbContextFactory<AppDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<bool> HandleAsync(
        DeleteAddressCommand command)
    {
        await using var context =
            await _contextFactory.CreateDbContextAsync();

        var address = await context.Addresses
            .FirstOrDefaultAsync(x => x.Id == command.Id);

        if (address is null)
        {
            return false;
        }

        context.Addresses.Remove(address);

        await context.SaveChangesAsync();

        return true;
    }
}