using InterviewApiDemo.Models;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace InterviewApiDemo.Services;

public class RabbitService()
{
    // CloudAMQP URL in format amqp://user:pass@hostName:port/vhost
    private static readonly string _url = "amqp://guest:guest@localhost/%2f";

    public async Task SendUserCreatedAsync(User user)
    {
		try
		{
            // Create a ConnectionFactory and set the Uri to the CloudAMQP url
            // the connectionfactory is stateless and can safetly be a static resource in your app
            var factory = new ConnectionFactory
            {
                Uri = new Uri(_url)
            };

            // create a connection and open a channel, dispose them when done
            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            // ensure that the queue exists before we publish to it
            var queueName = "queue1";
            bool durable = false;
            bool exclusive = false;
            bool autoDelete = true;

            await channel.QueueDeclareAsync(queueName, durable, exclusive, autoDelete, null);

            // the data put on the queue must be a byte array
            var data = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(user));

            // publish to the "default exchange", with the queue name as the routing key
            var exchangeName = "";
            var routingKey = queueName;
            // await channel.BasicPublishAsync(exchangeName, routingKey, null, data);
		}
		catch (Exception)
		{
            // log the exception
        }
    }
}
