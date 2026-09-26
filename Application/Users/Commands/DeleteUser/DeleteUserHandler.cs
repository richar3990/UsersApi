using Microsoft.EntityFrameworkCore;
using UsersApi.Infrastructure.Data;

namespace UsersApi.Application.Users.Commands.DeleteUser;

public class DeleteUserHandler
{
    private readonly IDbContextFactory<AppDbContext> _contextFactory;

    public DeleteUserHandler(
        IDbContextFactory<AppDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<bool> HandleAsync(
        DeleteUserCommand command)
    {
        await using var context =
            await _contextFactory.CreateDbContextAsync();

        var user = await context.Users
            .FirstOrDefaultAsync(x => x.Id == command.Id);

        if (user is null)
        {
            return false;
        }

        context.Users.Remove(user);

        await context.SaveChangesAsync();

        return true;
    }
}