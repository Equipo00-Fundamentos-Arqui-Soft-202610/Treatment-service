namespace MediTrack.TreatmentService.API.TreatmentManagement.Interfaces.REST.Resources;

public record MedicationCatalogResource(
    int Id,
    string OfficialName,
    string? Synonyms,
    string? Category
);
