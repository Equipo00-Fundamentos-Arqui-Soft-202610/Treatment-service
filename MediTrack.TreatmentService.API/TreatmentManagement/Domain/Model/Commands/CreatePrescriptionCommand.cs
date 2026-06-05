namespace MediTrack.TreatmentService.API.TreatmentManagement.Domain.Model.Commands;

public record CreatePrescriptionCommand(
    int PatientId,
    int TechnicalStaffId,
    string? Notes,
    List<CreateMedicationCommand> Medications
);

public record CreateMedicationCommand(
    int CatalogId,
    string Dose,
    int FrequencyHours,
    DateTime StartDate,
    DateTime? EndDate,
    int StockCount,
    int StockAlertThreshold,
    List<CreateDoseScheduleCommand> DoseSchedules
);

public record CreateDoseScheduleCommand(
    TimeOnly ScheduledTime
);