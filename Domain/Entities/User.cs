namespace UsersApi.Domain.Entities;

public class User
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public bool IsActive { get; set; } = true;

    public string? PasswordHash { get; set; }

    public ICollection<Address> Addresses { get; set; } = new List<Address>();
}