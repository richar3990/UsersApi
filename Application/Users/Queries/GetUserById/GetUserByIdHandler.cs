using Microsoft.EntityFrameworkCore;
using UsersApi.Application.Users.DTOs;
using UsersApi.Infrastructure.Data;

namespace UsersApi.Application.Users.Queries.GetUserById;

public class GetUserByIdHandler
{
    private readonly IDbContextFactory<AppDbContext> _contextFactory;

    public GetUserByIdHandler(
        IDbContextFactory<AppDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<UserResponse?> HandleAsync(
        GetUserByIdQuery query)
    {
        await using var context =
            await _contextFactory.CreateDbContextAsync();

        return await context.Users
            .AsNoTracking()
            .Where(x => x.Id == query.Id)
            .Select(x => new UserResponse(
                x.Id,
                x.Name,
                x.Email,
                x.IsActive))
            .FirstOrDefaultAsync();
    }
}