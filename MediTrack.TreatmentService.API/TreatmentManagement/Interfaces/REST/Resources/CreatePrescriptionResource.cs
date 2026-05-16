namespace MediTrack.TreatmentService.API.TreatmentManagement.Interfaces.REST.Resources;

public record CreatePrescriptionResource(
    int PatientId,
    int TechnicalId,
    string? Notes,
    List<CreateMedicationResource> Medications
);