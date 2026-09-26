using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UsersApi.Application.Users.DTOs;
using UsersApi.Domain.Entities;
using UsersApi.Infrastructure.Data;

namespace UsersApi.Application.Users.Commands.CreateUser;

public class CreateUserHandler
{
    private readonly IDbContextFactory<AppDbContext> _contextFactory;
    private readonly PasswordHasher<User> _passwordHasher;

    public CreateUserHandler(
        IDbContextFactory<AppDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
        _passwordHasher = new PasswordHasher<User>();
    }

    public async Task<UserResponse> HandleAsync(
        CreateUserCommand command)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var email = command.Request.Email.Trim();

        var emailExists = await context.Users
            .AnyAsync(x => x.Email == email);

        if (emailExists)
        {
            throw new InvalidOperationException("Ya existe un usuario con ese email.");
        }

        var user = new User
        {
            Name = command.Request.Name.Trim(),
            Email = email,
            IsActive = true
        };

        if (!string.IsNullOrWhiteSpace(command.Request.Password))
        {
            user.PasswordHash = _passwordHasher.HashPassword(
                user,
                command.Request.Password);
        }

        context.Users.Add(user);

        await context.SaveChangesAsync();

        return new UserResponse(
            user.Id,
            user.Name,
            user.Email,
            user.IsActive);
    }
}