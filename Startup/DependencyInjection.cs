using Microsoft.EntityFrameworkCore;
using InterviewApiDemo.Services;
using InterviewApiDemo.Data;

namespace InterviewApiDemo.Startup;

internal static class DependencyInjection
{
    public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<UsersService>();

        services.AddDatabase(configuration);
    }

    // One has to add the connection string as a secret with the name same as Constants.DbName.
    // https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-10.0&tabs=windows
    private static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(Constants.DbName)
            ?? throw new InvalidOperationException($"Connection string with name '{Constants.DbName}' not found");

        // Application db context
        services.AddDbContextFactory<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString,
                sql => sql.MigrationsHistoryTable("__EFMigrationsHistory", Constants.DbSchema)));
        services.AddScoped<ApplicationDbContext>();
    }

    public static void RunDatabaseMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        dbContext.Database.Migrate();
    }
}
