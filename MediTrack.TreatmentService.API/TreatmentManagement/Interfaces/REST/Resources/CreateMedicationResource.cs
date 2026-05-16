namespace MediTrack.TreatmentService.API.TreatmentManagement.Interfaces.REST.Resources;

public record CreateMedicationResource(
    int CatalogId,
    string Dose,
    int FrequencyHour,
    DateTime StartDate,
    DateTime? EndDate,
    int StockCount,
    int StockAlertThre,
    List<CreateDoseScheduleResource> DoseSchedules
);