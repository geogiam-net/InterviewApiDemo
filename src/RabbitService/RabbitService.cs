using Demo.Domain.Interfaces;
using Demo.Domain.Models;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Demo.Infrastructure.RabbitMQ;

// https://www.rabbitmq.com/tutorials/tutorial-one-dotnet
public class RabbitService : IMessageBroker
{
    private readonly string ConnectionString = string.Empty;

    public RabbitService(string connection)
    {
        ConnectionString = connection;
    }

    public async Task SendUserCreatedAsync(User user)
    {
        // https://www.rabbitmq.com/docs/download
        // var factory = new ConnectionFactory { HostName = "localhost" };
        var factory = new ConnectionFactory { Uri = new Uri(ConnectionString) };

        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(queue: "hello", durable: true, exclusive: false, autoDelete: false,
            arguments: new Dictionary<string, object?> { { "x-queue-type", "quorum" } });

        string message = JsonSerializer.Serialize(user);
        var body = Encoding.UTF8.GetBytes(message);
        await channel.BasicPublishAsync(exchange: string.Empty, routingKey: "hello", body: body);
    }
}
