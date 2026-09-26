using Microsoft.EntityFrameworkCore;
using UsersApi.Application.Users.DTOs;
using UsersApi.Infrastructure.Data;

namespace UsersApi.Application.Users.Commands.UpdateUser;

public class UpdateUserHandler
{
    private readonly IDbContextFactory<AppDbContext> _contextFactory;

    public UpdateUserHandler(
        IDbContextFactory<AppDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<UserResponse?> HandleAsync(
        UpdateUserCommand command)
    {
        await using var context =
            await _contextFactory.CreateDbContextAsync();

        var user = await context.Users
            .FirstOrDefaultAsync(x => x.Id == command.Id);

        if (user is null)
        {
            return null;
        }

        var email = command.Request.Email.Trim();

        var emailExists = await context.Users
            .AnyAsync(x =>
                x.Email == email &&
                x.Id != command.Id);

        if (emailExists)
        {
            throw new InvalidOperationException(
                "Ya existe otro usuario con ese email.");
        }

        user.Name = command.Request.Name.Trim();
        user.Email = email;
        user.IsActive = command.Request.IsActive;

        await context.SaveChangesAsync();

        return new UserResponse(
            user.Id,
            user.Name,
            user.Email,
            user.IsActive);
    }
}