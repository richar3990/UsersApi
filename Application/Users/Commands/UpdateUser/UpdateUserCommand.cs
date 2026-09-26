using UsersApi.Application.Users.DTOs;

namespace UsersApi.Application.Users.Commands.UpdateUser;

public record UpdateUserCommand(
    int Id,
    UpdateUserRequest Request
);