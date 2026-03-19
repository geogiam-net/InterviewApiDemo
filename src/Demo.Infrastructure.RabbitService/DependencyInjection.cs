using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Demo.Domain.Interfaces;

namespace Demo.Infrastructure.RabbitMQ;

public static class DependencyInjection
{
    public const string ApiDemoRabbitConnection = "ApiDemoRabbit";

    public static void AddRabbit(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(ApiDemoRabbitConnection)
            ?? throw new InvalidOperationException($"Connection string with name '{ApiDemoRabbitConnection}' not found");

        services.AddSingleton<IMessageBroker>(new RabbitService(connectionString));
    }
}