using Microsoft.EntityFrameworkCore;
using Demo.Domain.Models;

namespace Demo.Infrastructure.SqlStorage.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public const string DbSchema = "app";

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(DbSchema);

        modelBuilder.Entity<User>().HasKey(x => x.Id);

        modelBuilder.Entity<User>().Property(x => x.Username).HasMaxLength(50).IsRequired();
        modelBuilder.Entity<User>().Property(x => x.Name).HasMaxLength(80).IsRequired();
        modelBuilder.Entity<User>().Property(x => x.DateOfBirth).IsRequired();

        modelBuilder.Entity<User>().HasIndex(x => x.Username);

        base.OnModelCreating(modelBuilder);
    }
}
