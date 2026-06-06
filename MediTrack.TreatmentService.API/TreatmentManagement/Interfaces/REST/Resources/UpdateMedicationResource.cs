namespace MediTrack.TreatmentService.API.TreatmentManagement.Interfaces.REST.Resources;

public record UpdateMedicationResource(
    string Dose,
    int FrequencyHours,
    DateTime StartDate,
    DateTime? EndDate,
    int StockCount,
    int StockAlertThreshold,
    bool AuthorizedByTechnicalStaff
);