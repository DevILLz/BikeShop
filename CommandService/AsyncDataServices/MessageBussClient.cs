using CommandService.EventProcessing;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace CommandService.AsyncDataServices;

public class MessageBussSubscriber : BackgroundService
{
    private readonly IConfiguration config;
    private readonly IEventProcessor eventProcessor;
    private IConnection connection;
    private IModel channel;

    public MessageBussSubscriber(IConfiguration config, IEventProcessor eventProcessor)
    {
        this.config = config;
        this.eventProcessor = eventProcessor;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var factory = new ConnectionFactory
        {
            HostName = config["RabbitMQHost"],
            Port = int.Parse(config["RabbitMQPort"]),
        };

        try
        {
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            var queueName = channel.QueueDeclare().QueueName;

            channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
            channel.QueueBind(queueName, "trigger", "");
            connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

            Console.WriteLine($"--> Listening on the Message Bus...");

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += PlatformRecieved;

            channel.BasicConsume(queueName, true, consumer);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"--> Could ont connect to the Message Bus: {ex.Message}");
        }

        return Task.CompletedTask;
    }

    private void PlatformRecieved(object sender, BasicDeliverEventArgs e)
    {
        Console.WriteLine("--> Event bus msg Recieved!");

        var notificationMessage = Encoding.UTF8.GetString(e.Body.ToArray());

        eventProcessor.ProcessEvent(notificationMessage);
    }

    private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
    {
        Console.WriteLine($"--> RabbitMQ connection Shutdown");
    }

    public override void Dispose()
    {
        if (channel.IsClosed) return;
        channel.Close();
        connection.Close();
        connection.ConnectionShutdown -= RabbitMQ_ConnectionShutdown;
    }
}