namespace UsersApi.Application.Users.DTOs;

public record UpdateUserRequest(
    string Name,
    string Email,
    bool IsActive
);