namespace MediTrack.TreatmentService.API.TreatmentManagement.Domain.Model.Commands;

public record CreateClinicalRecordCommand(
    int PatientId,
    int UploadedBy,
    string? DatasetSource,
    string? FileUrl
);