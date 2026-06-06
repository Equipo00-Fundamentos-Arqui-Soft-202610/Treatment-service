namespace MediTrack.TreatmentService.API.TreatmentManagement.Interfaces.REST.Resources;

public record MedicationResource(
    int Id,
    int PrescriptionId,
    int CatalogId,
    string OfficialName,
    string? Category,
    string Dose,
    int FrequencyHours,
    DateTime StartDate,
    DateTime? EndDate,
    int StockCount,
    int StockAlertThreshold,
    List<string> ScheduledTimes
);