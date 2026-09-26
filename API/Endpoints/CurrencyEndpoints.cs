using UsersApi.Application.Currencies;
using UsersApi.Application.Currencies.Commands.CreateCurrency;
using UsersApi.Application.Currencies.Commands.DeleteCurrency;
using UsersApi.Application.Currencies.Commands.UpdateCurrency;
using UsersApi.Application.Currencies.DTOs;
using UsersApi.Application.Currencies.Queries.GetCurrencies;
using UsersApi.Application.Currencies.Queries.GetCurrencyById;

namespace UsersApi.API.Endpoints;

public static class CurrencyEndpoints
{
    public static void MapCurrencyEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/currencies")
            .WithTags("Currencies");

        group.MapPost("/", async (
            CreateCurrencyRequest request,
            CreateCurrencyHandler handler,
            CreateCurrencyValidator validator) =>
        {
            var command = new CreateCurrencyCommand(request);

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
                    $"/currencies/{result.Id}",
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
            GetCurrenciesHandler handler) =>
        {
            var result = await handler.HandleAsync(
                new GetCurrenciesQuery());

            return Results.Ok(result);
        });

        group.MapGet("/{id:int}", async (
            int id,
            GetCurrencyByIdHandler handler) =>
        {
            var result = await handler.HandleAsync(
                new GetCurrencyByIdQuery(id));

            return result is null
                ? Results.NotFound(new
                {
                    message = "Moneda no encontrada."
                })
                : Results.Ok(result);
        });

        group.MapPut("/{id:int}", async (
            int id,
            UpdateCurrencyRequest request,
            UpdateCurrencyHandler handler,
            UpdateCurrencyValidator validator) =>
        {
            var command = new UpdateCurrencyCommand(
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
                        message = "Moneda no encontrada."
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
            DeleteCurrencyHandler handler) =>
        {
            var deleted = await handler.HandleAsync(
                new DeleteCurrencyCommand(id));

            return deleted
                ? Results.NoContent()
                : Results.NotFound(new
                {
                    message = "Moneda no encontrada."
                });
        });

        group.MapPost("/convert", async (
            ConvertCurrencyRequest request,
            ConvertCurrencyHandler handler,
            ConvertCurrencyValidator validator) =>
        {
            var query = new ConvertCurrencyQuery(request);

            var validationResult =
                await validator.ValidateAsync(query);

            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(
                    validationResult.ToDictionary());
            }

            var result = await handler.HandleAsync(query);

            return result is null
                ? Results.NotFound(new
                {
                    message = "Una o ambas monedas no existen."
                })
                : Results.Ok(result);
        });
    }
}