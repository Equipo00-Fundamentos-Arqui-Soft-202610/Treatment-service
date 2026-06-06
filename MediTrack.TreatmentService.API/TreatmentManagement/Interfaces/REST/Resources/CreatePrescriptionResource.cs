namespace MediTrack.TreatmentService.API.TreatmentManagement.Interfaces.REST.Resources;

public record CreatePrescriptionResource(
    int PatientId,
    int TechnicalStaffId,
    string? Notes,
    List<CreateMedicationResource> Medications
);