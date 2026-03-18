using InterviewApiDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace InterviewApiDemo.Data;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options)
{
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Constants.DbSchema);

        base.OnModelCreating(modelBuilder);
    }
}
