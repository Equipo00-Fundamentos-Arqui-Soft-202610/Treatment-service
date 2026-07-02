namespace MediTrack.TreatmentService.API.TreatmentManagement.Application.OutboundEvents;

public record MedicationUpdatedEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;
    public int MedicationId { get; init; }
    public int PatientId { get; init; }
    public string Dose { get; init; } = string.Empty;
    public int FrequencyHours { get; init; }
    public DateTime StartDate { get; init; }
    public DateTime? EndDate { get; init; }
    public int StockCount { get; init; }
    public int StockAlertThreshold { get; init; }
}
