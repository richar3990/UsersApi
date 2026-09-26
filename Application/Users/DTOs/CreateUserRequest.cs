namespace UsersApi.Application.Users.DTOs;

public record CreateUserRequest(
    string Name,
    string Email,
    string? Password
);