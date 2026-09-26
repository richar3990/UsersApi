using UsersApi.Application.Users.DTOs;

namespace UsersApi.Application.Users.Commands.BulkCreateUsers;

public record BulkCreateUsersCommand(
    List<CreateUserRequest> Users
);