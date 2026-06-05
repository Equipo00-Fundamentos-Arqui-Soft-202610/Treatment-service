namespace MediTrack.TreatmentService.API.TreatmentManagement.Interfaces.REST.Resources;

public record CreateMedicationResource(
    int CatalogId,
    string Dose,
    int FrequencyHours,
    DateTime StartDate,
    DateTime? EndDate,
    int StockCount,
    int StockAlertThreshold,
    List<CreateDoseScheduleResource> DoseSchedules
);