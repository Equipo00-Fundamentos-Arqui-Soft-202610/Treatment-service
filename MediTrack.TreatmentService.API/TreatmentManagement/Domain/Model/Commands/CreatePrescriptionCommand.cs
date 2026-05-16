namespace MediTrack.TreatmentService.API.TreatmentManagement.Domain.Model.Commands;

public record CreatePrescriptionCommand(
    int PatientId,
    int TechnicalId,
    string? Notes,
    List<CreateMedicationCommand> Medications
);

public record CreateMedicationCommand(
    int CatalogId,
    string Dose,
    int FrequencyHour,
    DateTime StartDate,
    DateTime? EndDate,
    int StockCount,
    int StockAlertThre,
    List<CreateDoseScheduleCommand> DoseSchedules
);

public record CreateDoseScheduleCommand(
    TimeOnly ScheduledTime
);