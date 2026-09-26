using Microsoft.EntityFrameworkCore;
using UsersApi.Application.Users.DTOs;
using UsersApi.Infrastructure.Data;

namespace UsersApi.Application.Users.Queries.GetUsers;

public class GetUsersHandler
{
    private readonly IDbContextFactory<AppDbContext> _contextFactory;

    public GetUsersHandler(
        IDbContextFactory<AppDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<List<UserResponse>> HandleAsync(
        GetUsersQuery query)
    {
        await using var context =
            await _contextFactory.CreateDbContextAsync();

        return await context.Users
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .Select(x => new UserResponse(
                x.Id,
                x.Name,
                x.Email,
                x.IsActive))
            .ToListAsync();
    }
}