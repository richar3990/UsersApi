namespace UsersApi.Domain.Entities;

public class Address
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Street { get; set; } = null!;

    public string City { get; set; } = null!;

    public string Country { get; set; } = null!;

    public string? ZipCode { get; set; }

    public User User { get; set; } = null!;
}