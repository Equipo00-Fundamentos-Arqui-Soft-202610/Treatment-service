namespace MediTrack.TreatmentService.API.TreatmentManagement.Application.OutboundEvents;

public record MedicationCreatedDto(
    int MedicationId,
    int PatientId,
    int PrescriptionId,
    int CatalogId,
    string MedicationName,
    string Dose,
    int FrequencyHours,
    DateTime StartDate,
    DateTime? EndDate,
    int StockCount,
    int StockAlertThreshold,
    List<DoseScheduleCreatedDto> DoseSchedules
);
