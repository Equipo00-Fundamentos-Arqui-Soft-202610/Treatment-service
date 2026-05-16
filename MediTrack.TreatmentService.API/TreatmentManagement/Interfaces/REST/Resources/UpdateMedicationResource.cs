namespace MediTrack.TreatmentService.API.TreatmentManagement.Interfaces.REST.Resources;

public record UpdateMedicationResource(
    string Dose,
    int FrequencyHour,
    DateTime StartDate,
    DateTime? EndDate,
    int StockCount,
    int StockAlertThre,
    bool AuthorizedByTechnicalStaff
);