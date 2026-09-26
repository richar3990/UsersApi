using UsersApi.Application.Addresses.Commands.CreateAddress;
using UsersApi.Application.Addresses.Commands.DeleteAddress;
using UsersApi.Application.Addresses.Commands.UpdateAddress;
using UsersApi.Application.Addresses.DTOs;
using UsersApi.Application.Addresses.Queries.GetAddressById;
using UsersApi.Application.Addresses.Queries.GetAddresses;
using UsersApi.Application.Addresses.Queries.GetAddressesByUser;

namespace UsersApi.API.Endpoints;

public static class AddressEndpoints
{
    public static void MapAddressEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/addresses")
            .WithTags("Addresses");

        group.MapPost("/", async (
            CreateAddressRequest request,
            CreateAddressHandler handler,
            CreateAddressValidator validator) =>
        {
            var command = new CreateAddressCommand(request);

            var validationResult =
                await validator.ValidateAsync(command);

            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(
                    validationResult.ToDictionary());
            }

            var result = await handler.HandleAsync(command);

            if (result is null)
            {
                return Results.NotFound(new
                {
                    message = "El usuario indicado no existe."
                });
            }

            return Results.Created(
                $"/addresses/{result.Id}",
                result);
        });

        group.MapGet("/", async (
            GetAddressesHandler handler) =>
        {
            var result = await handler.HandleAsync(
                new GetAddressesQuery());

            return Results.Ok(result);
        });

        group.MapGet("/{id:int}", async (
            int id,
            GetAddressByIdHandler handler) =>
        {
            var result = await handler.HandleAsync(
                new GetAddressByIdQuery(id));

            return result is null
                ? Results.NotFound(new
                {
                    message = "Dirección no encontrada."
                })
                : Results.Ok(result);
        });

        group.MapGet("/user/{userId:int}", async (
            int userId,
            GetAddressesByUserHandler handler) =>
        {
            var result = await handler.HandleAsync(
                new GetAddressesByUserQuery(userId));

            return Results.Ok(result);
        });

        group.MapPut("/{id:int}", async (
            int id,
            UpdateAddressRequest request,
            UpdateAddressHandler handler,
            UpdateAddressValidator validator) =>
        {
            var command = new UpdateAddressCommand(
                id,
                request);

            var validationResult =
                await validator.ValidateAsync(command);

            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(
                    validationResult.ToDictionary());
            }

            var result = await handler.HandleAsync(command);

            return result is null
                ? Results.NotFound(new
                {
                    message = "Dirección no encontrada."
                })
                : Results.Ok(result);
        });

        group.MapDelete("/{id:int}", async (
            int id,
            DeleteAddressHandler handler) =>
        {
            var deleted = await handler.HandleAsync(
                new DeleteAddressCommand(id));

            return deleted
                ? Results.NoContent()
                : Results.NotFound(new
                {
                    message = "Dirección no encontrada."
                });
        });
    }
}