namespace UsersApi.Application.Addresses.DTOs;

public record UpdateAddressRequest(
    string Street,
    string City,
    string Country,
    string? ZipCode
);