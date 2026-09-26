namespace UsersApi.Application.Addresses.DTOs;

public record CreateAddressRequest(
    int UserId,
    string Street,
    string City,
    string Country,
    string? ZipCode
);