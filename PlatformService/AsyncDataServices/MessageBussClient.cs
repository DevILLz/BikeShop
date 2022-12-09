using PlatformService.DTOs;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace PlatformService.AsyncDataServices;

public class MessageBussClient : IMessageBussClient, IDisposable
{
    private readonly IConfiguration config;
    private readonly IConnection connection;
    private readonly IModel channel;

    public MessageBussClient(IConfiguration config)
    {
        this.config = config;

        var factory = new ConnectionFactory
        {
            HostName = config["RabbitMQHost"],
            Port = int.Parse(config["RabbitMQPort"]),
        };

        try
        {
            connection = factory.CreateConnection();
            channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);

            connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"--> Could ont connect to the Message Bus: {ex.Message}");
        }
    }

    private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
    {
        Console.WriteLine($"--> RabbitMQ connection Shutdown");
    }

    public void PublishNewPlatform(PlatformPublishDto dto)
    {
        var message = JsonSerializer.Serialize(dto);

        if (connection.IsOpen is false)
        {
            Console.WriteLine("--> RabbitMQ connection is closed, can't publish message");
            return;
        }

        Console.WriteLine($"--> RabbitMQ connection is open, publishing message {message}");
        SendMessage(message);
    }

    private void SendMessage(string message)
    {
        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(exchange: "trigger", routingKey: "", basicProperties: null, body: body);
    }

    public void Dispose()
    {
        if (channel.IsClosed) return;
        channel.Close();
        connection.Close();
    }
}