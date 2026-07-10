namespace MediTrack.TreatmentService.API.TreatmentManagement.Application.OutboundEvents;

public record MedicationCancelledEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;
    public int MedicationId { get; init; }
    public int PatientId { get; init; }
}
