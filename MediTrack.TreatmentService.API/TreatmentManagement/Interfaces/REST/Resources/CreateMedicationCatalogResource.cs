namespace MediTrack.TreatmentService.API.TreatmentManagement.Interfaces.REST.Resources;

public record CreateMedicationCatalogResource(
    string OfficialName,
    string? Synonyms,
    string? Category
);
