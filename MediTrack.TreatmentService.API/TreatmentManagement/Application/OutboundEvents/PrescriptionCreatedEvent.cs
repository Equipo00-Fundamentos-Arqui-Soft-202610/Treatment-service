namespace MediTrack.TreatmentService.API.TreatmentManagement.Application.OutboundEvents;

public record PrescriptionCreatedEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;
    public int PrescriptionId { get; init; }
    public int PatientId { get; init; }
    public List<MedicationCreatedDto> Medications { get; init; } = new();
}
