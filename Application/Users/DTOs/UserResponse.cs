namespace UsersApi.Application.Users.DTOs;

public record UserResponse(
    int Id,
    string Name,
    string Email,
    bool IsActive
);