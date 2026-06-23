namespace MediTrack.TreatmentService.API.TreatmentManagement.Infrastructure.Messaging;

public interface IEventPublisher
{
    Task PublishAsync(string routingKey, object payload);
}
