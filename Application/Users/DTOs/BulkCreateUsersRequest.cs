namespace UsersApi.Application.Users.DTOs;

public record BulkCreateUsersRequest(
    List<CreateUserRequest> Users
);