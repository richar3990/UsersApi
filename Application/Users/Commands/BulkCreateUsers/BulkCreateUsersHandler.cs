using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UsersApi.Application.Users.DTOs;
using UsersApi.Domain.Entities;
using UsersApi.Infrastructure.Data;

namespace UsersApi.Application.Users.Commands.BulkCreateUsers;

public class BulkCreateUsersHandler
{
    private readonly IDbContextFactory<AppDbContext> _contextFactory;
    private readonly PasswordHasher<User> _passwordHasher = new();

    public BulkCreateUsersHandler(
        IDbContextFactory<AppDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<BulkCreateUsersResponse> HandleAsync(
        BulkCreateUsersCommand command)
    {
        var tasks = command.Users
            .Select((request, index) =>
                CreateUserAsync(request, index))
            .ToList();

        var results = await Task.WhenAll(tasks);

        var failures = results
            .Where(x => x.Failure is not null)
            .Select(x => x.Failure!)
            .ToList();

        return new BulkCreateUsersResponse(
            Total: results.Length,
            Created: results.Count(x => x.Success),
            Failed: failures.Count,
            Failures: failures
        );
    }

    private async Task<BulkCreateUserResult> CreateUserAsync(
        CreateUserRequest request,
        int index)
    {
        await using var context =
            await _contextFactory.CreateDbContextAsync();

        var email = request.Email.Trim();

        var exists = await context.Users
            .AnyAsync(x => x.Email == email);

        if (exists)
        {
            return BulkCreateUserResult.Failed(
                new BulkUserFailure(
                    index,
                    email,
                    "El email ya está registrado."
                ));
        }

        var user = new User
        {
            Name = request.Name.Trim(),
            Email = email,
            IsActive = true
        };

        if (!string.IsNullOrWhiteSpace(request.Password))
        {
            user.PasswordHash =
                _passwordHasher.HashPassword(
                    user,
                    request.Password);
        }

        try
        {
            context.Users.Add(user);

            await context.SaveChangesAsync();

            return BulkCreateUserResult.Successful();
        }
        catch (DbUpdateException)
        {
            return BulkCreateUserResult.Failed(
                new BulkUserFailure(
                    index,
                    email,
                    "No fue posible crear el usuario."
                ));
        }
    }

    private record BulkCreateUserResult(
        bool Success,
        BulkUserFailure? Failure)
    {
        public static BulkCreateUserResult Successful() =>
            new(true, null);

        public static BulkCreateUserResult Failed(
            BulkUserFailure failure) =>
            new(false, failure);
    }
}