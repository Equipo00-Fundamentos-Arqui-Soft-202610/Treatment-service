namespace MediTrack.TreatmentService.API.TreatmentManagement.Interfaces.REST.Resources;

public record PatientSearchResultResource(
    int PatientId,
    string FullName,
    string Email,
    string? Dni,
    int? Age
);