using Demo.Infrastructure.SqlStorage.Data;
using Microsoft.EntityFrameworkCore;

namespace Demo.Api.Startup;

internal static class DatabaseMigrations
{
    public static void RunDatabaseMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        dbContext.Database.Migrate();
    }
}
