using UsersApi.Application.Addresses.DTOs;

namespace UsersApi.Application.Addresses.Commands.UpdateAddress;

public record UpdateAddressCommand(
    int Id,
    UpdateAddressRequest Request
);