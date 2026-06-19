namespace MediTrack.TreatmentService.API.TreatmentManagement.Domain.Model.Commands;

public record CreateMedicationCatalogCommand(
    string OfficialName,
    string? Synonyms,
    string? Category
);
