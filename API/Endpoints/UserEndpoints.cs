using UsersApi.Application.Users.Commands.CreateUser;
using UsersApi.Application.Users.Commands.DeleteUser;
using UsersApi.Application.Users.Commands.UpdateUser;
using UsersApi.Application.Users.DTOs;
using UsersApi.Application.Users.Queries.GetUserById;
using UsersApi.Application.Users.Queries.GetUsers;
using UsersApi.Application.Users.Commands.BulkCreateUsers;

namespace UsersApi.API.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/users")
            .WithTags("Users");

        group.MapPost("/", async (
            CreateUserRequest request,
            CreateUserHandler handler,
            CreateUserValidator validator) =>
        {
            var command = new CreateUserCommand(request);

            var validationResult =
                await validator.ValidateAsync(command);

            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(
                    validationResult.ToDictionary());
            }

            try
            {
                var result = await handler.HandleAsync(command);

                return Results.Created(
                    $"/users/{result.Id}",
                    result);
            }
            catch (InvalidOperationException ex)
            {
                return Results.Conflict(new
                {
                    message = ex.Message
                });
            }
        });

        group.MapGet("/", async (
            GetUsersHandler handler) =>
        {
            var result = await handler.HandleAsync(
                new GetUsersQuery());

            return Results.Ok(result);
        });

        group.MapGet("/{id:int}", async (
            int id,
            GetUserByIdHandler handler) =>
        {
            var result = await handler.HandleAsync(
                new GetUserByIdQuery(id));

            return result is null
                ? Results.NotFound(new
                {
                    message = "Usuario no encontrado."
                })
                : Results.Ok(result);
        });

        group.MapPut("/{id:int}", async (
            int id,
            UpdateUserRequest request,
            UpdateUserHandler handler,
            UpdateUserValidator validator) =>
        {
            var command = new UpdateUserCommand(
                id,
                request);

            var validationResult =
                await validator.ValidateAsync(command);

            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(
                    validationResult.ToDictionary());
            }

            try
            {
                var result = await handler.HandleAsync(command);

                return result is null
                    ? Results.NotFound(new
                    {
                        message = "Usuario no encontrado."
                    })
                    : Results.Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return Results.Conflict(new
                {
                    message = ex.Message
                });
            }
        });

        group.MapDelete("/{id:int}", async (
            int id,
            DeleteUserHandler handler) =>
        {
            var deleted = await handler.HandleAsync(
                new DeleteUserCommand(id));

            return deleted
                ? Results.NoContent()
                : Results.NotFound(new
                {
                    message = "Usuario no encontrado."
                });
        });

        group.MapPost("/bulk", async (
    BulkCreateUsersRequest request,
    BulkCreateUsersHandler handler) =>
{
    if (request.Users is null || request.Users.Count == 0)
    {
        return Results.BadRequest(new
        {
            message = "Debe enviar al menos un usuario."
        });
    }

    var command = new BulkCreateUsersCommand(request.Users);

    var result = await handler.HandleAsync(command);

    return Results.Ok(result);
});


    }
}