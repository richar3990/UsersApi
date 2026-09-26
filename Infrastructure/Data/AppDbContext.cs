using Microsoft.EntityFrameworkCore;
using UsersApi.Domain.Entities;

namespace UsersApi.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    public DbSet<Address> Addresses => Set<Address>();

    public DbSet<Currency> Currencies => Set<Currency>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureUser(modelBuilder);
        ConfigureAddress(modelBuilder);
        ConfigureCurrency(modelBuilder);
    }

    private static void ConfigureUser(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            entity.Property(x => x.PasswordHash)
                .HasMaxLength(500);

            entity.HasIndex(x => x.Email)
                .IsUnique();
        });
    }

    private static void ConfigureAddress(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.ToTable("Addresses");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Street)
                .IsRequired()
                .HasMaxLength(250);

            entity.Property(x => x.City)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(x => x.Country)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(x => x.ZipCode)
                .HasMaxLength(20);

            entity.HasOne(x => x.User)
                .WithMany(x => x.Addresses)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(x => x.UserId);
        });
    }

    private static void ConfigureCurrency(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Currency>(entity =>
        {
            entity.ToTable("Currencies");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Code)
                .IsRequired()
                .HasMaxLength(3);

            entity.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(x => x.RateToBase)
                .IsRequired()
                .HasPrecision(19, 6);

            entity.HasIndex(x => x.Code)
                .IsUnique();
        });
    }
}