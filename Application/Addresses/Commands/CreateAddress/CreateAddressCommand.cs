using UsersApi.Application.Addresses.DTOs;

namespace UsersApi.Application.Addresses.Commands.CreateAddress;

public record CreateAddressCommand(
    CreateAddressRequest Request
);