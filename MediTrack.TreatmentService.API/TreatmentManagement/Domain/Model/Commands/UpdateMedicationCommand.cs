namespace MediTrack.TreatmentService.API.TreatmentManagement.Domain.Model.Commands;

public record UpdateMedicationCommand(
    int MedicationId,
    string Dose,
    int FrequencyHours,
    DateTime StartDate,
    DateTime? EndDate,
    int StockCount,
    int StockAlertThreshold,
    bool AuthorizedByTechnicalStaff
);