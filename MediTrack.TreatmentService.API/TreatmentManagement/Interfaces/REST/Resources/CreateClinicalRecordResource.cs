namespace MediTrack.TreatmentService.API.TreatmentManagement.Interfaces.REST.Resources;

public record CreateClinicalRecordResource(
    int PatientId,
    int UploadedBy,
    string? DatasetSource,
    string? FileUrl
);