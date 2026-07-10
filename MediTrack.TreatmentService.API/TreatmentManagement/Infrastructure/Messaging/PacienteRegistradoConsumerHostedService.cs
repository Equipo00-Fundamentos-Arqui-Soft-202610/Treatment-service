using System.Text;
using System.Text.Json;
using MediTrack.TreatmentService.API.TreatmentManagement.Application.InboundEvents;
using MediTrack.TreatmentService.API.TreatmentManagement.Application.Internal.EventHandlers;
using MediTrack.TreatmentService.API.TreatmentManagement.Infrastructure.Persistence.EFC;
using MediTrack.TreatmentService.API.TreatmentManagement.Infrastructure.Persistence.EFC.Configuration;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MediTrack.TreatmentService.API.TreatmentManagement.Infrastructure.Messaging;

/// <summary>
/// Consume el evento <c>PacienteRegistrado</c> del Identity Service y alimenta la
/// proyección local de pacientes (patrón Inbox para idempotencia: un EventId ya
/// procesado se descarta sin reaplicarse).
///
/// A diferencia de otros consumers del sistema, este valida el payload ANTES de
/// invocar al handler: datos incompletos o inválidos se descartan (ack) en vez de
/// reencolarse — evita el loop infinito de "mensaje envenenado" ya detectado en
/// otros servicios cuando un campo requerido llega en null.
/// </summary>
public sealed class PacienteRegistradoConsumerHostedService : BackgroundService
{
    private const string RoutingKey = "PacienteRegistrado";
    private const string QueueName = "treatment-service.events";

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<PacienteRegistradoConsumerHostedService> _logger;
    private IConnection? _connection;
    private IModel? _channel;

    public PacienteRegistradoConsumerHostedService(
        IServiceScopeFactory scopeFactory,
        IConfiguration configuration,
        ILogger<PacienteRegistradoConsumerHostedService> logger)
    {
        _scopeFactory = scopeFactory;
        _configuration = configuration;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory
        {
            HostName = _configuration["RabbitMq:Host"] ?? "localhost",
            Port = int.Parse(_configuration["RabbitMq:Port"] ?? "5672"),
            UserName = _configuration["RabbitMq:UserName"] ?? "guest",
            Password = _configuration["RabbitMq:Password"] ?? "guest",
            VirtualHost = _configuration["RabbitMq:VirtualHost"] ?? "/",
            DispatchConsumersAsync = true
        };
        var exchangeName = _configuration["RabbitMq:ExchangeName"] ?? "meditrack.events";

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.ExchangeDeclare(exchange: exchangeName, type: "topic", durable: true, autoDelete: false);
        _channel.QueueDeclare(queue: QueueName, durable: true, exclusive: false, autoDelete: false);
        _channel.QueueBind(queue: QueueName, exchange: exchangeName, routingKey: RoutingKey);
        _channel.BasicQos(prefetchSize: 0, prefetchCount: 10, global: false);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.Received += OnReceivedAsync;
        _channel.BasicConsume(queue: QueueName, autoAck: false, consumer: consumer);

        _logger.LogInformation("PacienteRegistrado consumer escuchando la cola {Queue}.", QueueName);
        return Task.CompletedTask;
    }

    private async Task OnReceivedAsync(object sender, BasicDeliverEventArgs args)
    {
        var payload = Encoding.UTF8.GetString(args.Body.Span);

        PacienteRegistradoEvent? @event;
        try
        {
            @event = JsonSerializer.Deserialize<PacienteRegistradoEvent>(
                payload, new JsonSerializerOptions(JsonSerializerDefaults.Web));
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Evento PacienteRegistrado con JSON inválido; se descarta.");
            _channel!.BasicAck(args.DeliveryTag, multiple: false);
            return;
        }

        if (@event is null || @event.PatientId <= 0 || string.IsNullOrWhiteSpace(@event.FullName))
        {
            _logger.LogWarning(
                "Evento PacienteRegistrado con datos incompletos (PatientId={PatientId}); se descarta sin reintentar.",
                @event?.PatientId);
            _channel!.BasicAck(args.DeliveryTag, multiple: false);
            return;
        }

        try
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var alreadyProcessed = await context.ProcessedEvents.AnyAsync(e => e.EventId == @event.EventId);
            if (alreadyProcessed)
            {
                _logger.LogDebug("Evento {EventId} ya procesado; se omite.", @event.EventId);
                _channel!.BasicAck(args.DeliveryTag, multiple: false);
                return;
            }

            var handler = scope.ServiceProvider.GetRequiredService<PacienteRegistradoEventHandler>();
            await handler.HandleAsync(@event);

            context.ProcessedEvents.Add(new ProcessedEvent
            {
                EventId = @event.EventId,
                EventType = RoutingKey,
                ProcessedAtUtc = DateTime.UtcNow
            });
            await context.SaveChangesAsync();

            _channel!.BasicAck(args.DeliveryTag, multiple: false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error transitorio procesando PacienteRegistrado (PatientId={PatientId}). Se reencolará.",
                @event.PatientId);
            _channel!.BasicNack(args.DeliveryTag, multiple: false, requeue: true);
        }
    }

    public override void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
        base.Dispose();
    }
}
