using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace MediTrack.TreatmentService.API.TreatmentManagement.Infrastructure.Messaging;

public class RabbitMqPublisher : IEventPublisher, IDisposable
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly string _exchangeName;
    private readonly ILogger<RabbitMqPublisher> _logger;

    public RabbitMqPublisher(IConfiguration configuration, ILogger<RabbitMqPublisher> logger)
    {
        _logger = logger;

        var factory = new ConnectionFactory
        {
            HostName = configuration["RabbitMq:Host"] ?? "localhost",
            Port = int.Parse(configuration["RabbitMq:Port"] ?? "5672"),
            UserName = configuration["RabbitMq:UserName"] ?? "guest",
            Password = configuration["RabbitMq:Password"] ?? "guest",
            VirtualHost = configuration["RabbitMq:VirtualHost"] ?? "/",
            DispatchConsumersAsync = true
        };

        _exchangeName = configuration["RabbitMq:ExchangeName"] ?? "meditrack.events";

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.ExchangeDeclare(
            exchange: _exchangeName,
            type: "topic",
            durable: true,
            autoDelete: false
        );

        _logger.LogInformation("RabbitMQ publisher initialized for exchange {ExchangeName}", _exchangeName);
    }

    public Task PublishAsync(string routingKey, object payload)
    {
        try
        {
            var json = JsonSerializer.Serialize(payload);
            var body = Encoding.UTF8.GetBytes(json);

            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true;
            properties.ContentType = "application/json";
            properties.Type = routingKey;

            _channel.BasicPublish(
                exchange: _exchangeName,
                routingKey: routingKey,
                mandatory: true,
                basicProperties: properties,
                body: body
            );

            _logger.LogInformation("Published event {RoutingKey} to exchange {ExchangeName}",
                routingKey, _exchangeName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to publish event {RoutingKey} to exchange {ExchangeName}",
                routingKey, _exchangeName);
            throw;
        }

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _channel?.Close();
        _channel?.Dispose();
        _connection?.Close();
        _connection?.Dispose();
    }
}
