using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Model.Commands;
using MediTrack.TreatmentService.API.TreatmentManagement.Interfaces.REST.Resources;

namespace MediTrack.TreatmentService.API.TreatmentManagement.Interfaces.REST.Transform;

public static class CreateMedicationCatalogCommandFromResourceAssembler
{
    public static CreateMedicationCatalogCommand ToCommandFromResource(CreateMedicationCatalogResource resource) =>
        new(resource.OfficialName, resource.Synonyms, resource.Category);
}
