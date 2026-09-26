namespace UsersApi.Infrastructure.Security;

public class ApiKeyMiddleware
{
    private const string ApiKeyHeaderName = "X-API-KEY";

    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;

    public ApiKeyMiddleware(
        RequestDelegate next,
        IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue(
                ApiKeyHeaderName,
                out var providedApiKey))
        {
            await WriteUnauthorizedResponseAsync(
                context,
                "API Key requerida.");

            return;
        }

        var configuredApiKey =
            _configuration["ApiKey"];

        if (string.IsNullOrWhiteSpace(configuredApiKey) ||
            !string.Equals(
                providedApiKey.ToString(),
                configuredApiKey,
                StringComparison.Ordinal))
        {
            await WriteUnauthorizedResponseAsync(
                context,
                "API Key inválida.");

            return;
        }

        await _next(context);
    }

    private static async Task WriteUnauthorizedResponseAsync(
        HttpContext context,
        string message)
    {
        context.Response.StatusCode =
            StatusCodes.Status401Unauthorized;

        context.Response.ContentType =
            "application/json";

        await context.Response.WriteAsJsonAsync(new
        {
            message
        });
    }
}