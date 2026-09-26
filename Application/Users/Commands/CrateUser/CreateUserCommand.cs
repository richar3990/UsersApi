using UsersApi.Application.Users.DTOs;

namespace UsersApi.Application.Users.Commands.CreateUser;

public record CreateUserCommand(
    CreateUserRequest Request
);