namespace MediTrack.TreatmentService.API.TreatmentManagement.Domain.Model.Commands;

public record UpdateMedicationCommand(
    int MedicationId,
    string Dose,
    int FrequencyHour,
    DateTime StartDate,
    DateTime? EndDate,
    int StockCount,
    int StockAlertThre,
    bool AuthorizedByTechnicalStaff
);