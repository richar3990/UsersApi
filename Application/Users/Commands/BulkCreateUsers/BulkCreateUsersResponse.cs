namespace UsersApi.Application.Users.Commands.BulkCreateUsers;

public record BulkCreateUsersResponse(
    int Total,
    int Created,
    int Failed,
    List<BulkUserFailure> Failures
);

public record BulkUserFailure(
    int Index,
    string? Email,
    string Reason
);