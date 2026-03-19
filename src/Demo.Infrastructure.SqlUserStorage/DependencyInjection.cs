using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Demo.Infrastructure.SqlStorage.Data;
using Demo.Domain.Interfaces;

namespace Demo.Infrastructure.SqlStorage;

public static class DependencyInjection
{
    private const string ApiDemoDbConnection = "ApiDemoDb";

    public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUserRepository, UsersService>();

        var connectionString = configuration.GetConnectionString(ApiDemoDbConnection)
            ?? throw new InvalidOperationException($"Connection string with name '{ApiDemoDbConnection}' not found");

        services.AddDbContextFactory<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString,
                sql => sql.MigrationsHistoryTable("__EFMigrationsHistory", ApplicationDbContext.DbSchema)));

        services.AddScoped<ApplicationDbContext>();
    }
}