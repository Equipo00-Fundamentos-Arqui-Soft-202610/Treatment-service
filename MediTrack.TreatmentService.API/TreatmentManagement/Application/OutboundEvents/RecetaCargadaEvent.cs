namespace MediTrack.TreatmentService.API.TreatmentManagement.Application.OutboundEvents;

public record DoseOccurrenceDto(
    int MedicationId,
    string Name,
    string Dose,
    DateTime DoseTimeUtc
);

public record RecetaCargadaEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;
    public int PatientId { get; init; }
    public int PrescriptionId { get; init; }
    public List<DoseOccurrenceDto> Medications { get; init; } = new();
}
