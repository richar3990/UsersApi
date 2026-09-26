using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using UsersApi.Infrastructure.Data;
using UsersApi.Infrastructure.Security;
using UsersApi.Application.Users.Commands.CreateUser;
using UsersApi.Application.Users.Commands.UpdateUser;
using UsersApi.Application.Users.Commands.DeleteUser;
using UsersApi.Application.Users.Queries.GetUsers;
using UsersApi.Application.Users.Queries.GetUserById;
using UsersApi.Application.Addresses.Commands.CreateAddress;
using UsersApi.Application.Addresses.Commands.UpdateAddress;
using UsersApi.Application.Addresses.Commands.DeleteAddress;
using UsersApi.Application.Addresses.Queries.GetAddresses;
using UsersApi.Application.Addresses.Queries.GetAddressById;
using UsersApi.Application.Addresses.Queries.GetAddressesByUser;

using UsersApi.Application.Currencies;
using UsersApi.Application.Currencies.Commands.CreateCurrency;
using UsersApi.Application.Currencies.Commands.DeleteCurrency;
using UsersApi.Application.Currencies.Commands.UpdateCurrency;
using UsersApi.Application.Currencies.Queries.GetCurrencies;
using UsersApi.Application.Currencies.Queries.GetCurrencyById;

using UsersApi.Application.Users.Commands.BulkCreateUsers;

using UsersApi.API.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextFactory<AppDbContext>(options =>
{
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Users API",
        Version = "v1",
        Description = "API de usuarios, direcciones y divisas"
    });

    options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Name = "X-API-KEY",
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Description = "API Key requerida para acceder a la API."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddScoped<CreateUserHandler>();
builder.Services.AddScoped<UpdateUserHandler>();
builder.Services.AddScoped<DeleteUserHandler>();
builder.Services.AddScoped<GetUsersHandler>();
builder.Services.AddScoped<GetUserByIdHandler>();
builder.Services.AddScoped<CreateUserValidator>();
builder.Services.AddScoped<UpdateUserValidator>();
builder.Services.AddScoped<CreateAddressHandler>();
builder.Services.AddScoped<UpdateAddressHandler>();
builder.Services.AddScoped<DeleteAddressHandler>();

builder.Services.AddScoped<GetAddressesHandler>();
builder.Services.AddScoped<GetAddressByIdHandler>();
builder.Services.AddScoped<GetAddressesByUserHandler>();

builder.Services.AddScoped<CreateAddressValidator>();
builder.Services.AddScoped<UpdateAddressValidator>();

builder.Services.AddScoped<CreateCurrencyHandler>();
builder.Services.AddScoped<UpdateCurrencyHandler>();
builder.Services.AddScoped<DeleteCurrencyHandler>();

builder.Services.AddScoped<GetCurrenciesHandler>();
builder.Services.AddScoped<GetCurrencyByIdHandler>();

builder.Services.AddScoped<ConvertCurrencyHandler>();

builder.Services.AddScoped<CreateCurrencyValidator>();
builder.Services.AddScoped<UpdateCurrencyValidator>();
builder.Services.AddScoped<ConvertCurrencyValidator>();

builder.Services.AddScoped<BulkCreateUsersHandler>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var contextFactory =
        scope.ServiceProvider.GetRequiredService<
            IDbContextFactory<AppDbContext>>();

    await DbInitializer.InitializeAsync(contextFactory);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ApiKeyMiddleware>();

app.UseHttpsRedirection();

app.MapGet("/", () => Results.Ok(new
{
    message = "Users API is running"
}));

app.MapUserEndpoints();
app.MapAddressEndpoints();
app.MapCurrencyEndpoints();

app.Run();