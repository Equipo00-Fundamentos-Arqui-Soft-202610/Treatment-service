using System.Text;
using MediTrack.TreatmentService.API.TreatmentManagement.Infrastructure.Persistence.EFC.Configuration;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;

namespace MediTrack.TreatmentService.API.TreatmentManagement.Infrastructure.Messaging;

/// <summary>
/// Lee periódicamente los mensajes pendientes del Outbox y los entrega a
/// RabbitMQ usando el EventType como routing key. Marca cada mensaje como
/// procesado solo tras confirmar la publicación (entrega "al menos una vez").
/// </summary>
public sealed class OutboxDispatcherHostedService : BackgroundService
{
    private const int BatchSize = 50;
    private static readonly TimeSpan PollingInterval = TimeSpan.FromSeconds(10);

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<OutboxDispatcherHostedService> _logger;

    public OutboxDispatcherHostedService(
        IServiceScopeFactory scopeFactory, IConfiguration configuration, ILogger<OutboxDispatcherHostedService> logger)
    {
        _scopeFactory = scopeFactory;
        _configuration = configuration;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await DispatchPendingAsync(stoppingToken);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogError(ex, "Error en el ciclo del publicador de Outbox.");
            }

            await Task.Delay(PollingInterval, stoppingToken);
        }
    }

    private async Task DispatchPendingAsync(CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var pending = await context.OutboxMessages
            .Where(m => m.ProcessedAtUtc == null)
            .OrderBy(m => m.OccurredAtUtc)
            .Take(BatchSize)
            .ToListAsync(cancellationToken);

        if (pending.Count == 0)
            return;

        var factory = new ConnectionFactory
        {
            HostName = _configuration["RabbitMq:Host"] ?? "localhost",
            Port = int.Parse(_configuration["RabbitMq:Port"] ?? "5672"),
            UserName = _configuration["RabbitMq:UserName"] ?? "guest",
            Password = _configuration["RabbitMq:Password"] ?? "guest",
            VirtualHost = _configuration["RabbitMq:VirtualHost"] ?? "/"
        };
        var exchangeName = _configuration["RabbitMq:ExchangeName"] ?? "meditrack.events";

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        channel.ExchangeDeclare(exchange: exchangeName, type: "topic", durable: true, autoDelete: false);
        channel.ConfirmSelect();

        var unroutableMessageIds = new HashSet<string>();
        channel.BasicReturn += (_, args) =>
        {
            unroutableMessageIds.Add(args.BasicProperties.MessageId);
            _logger.LogError(
                "Mensaje de Outbox {MessageId} no pudo ser ruteado (sin binding activo): {ReplyText}",
                args.BasicProperties.MessageId, args.ReplyText);
        };

        foreach (var message in pending)
        {
            try
            {
                var body = Encoding.UTF8.GetBytes(message.Payload);
                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;
                properties.MessageId = message.Id.ToString();
                properties.Type = message.EventType;
                properties.ContentType = "application/json";

                channel.BasicPublish(exchangeName, message.EventType, mandatory: true, properties, body);
                channel.WaitForConfirmsOrDie(TimeSpan.FromSeconds(5));

                if (unroutableMessageIds.Contains(properties.MessageId))
                {
                    message.Attempts++;
                    message.LastError = "Sin cola/binding activo en el momento del publish; se reintentará.";
                }
                else
                {
                    message.ProcessedAtUtc = DateTime.UtcNow;
                }
            }
            catch (Exception ex)
            {
                message.Attempts++;
                message.LastError = ex.Message;
                _logger.LogError(ex, "No se pudo publicar el mensaje de Outbox {MessageId}.", message.Id);
            }
        }

        await context.SaveChangesAsync(cancellationToken);
        _logger.LogInformation(
            "Publicados {Count} mensajes del Outbox.", pending.Count(m => m.ProcessedAtUtc != null));
    }
}
