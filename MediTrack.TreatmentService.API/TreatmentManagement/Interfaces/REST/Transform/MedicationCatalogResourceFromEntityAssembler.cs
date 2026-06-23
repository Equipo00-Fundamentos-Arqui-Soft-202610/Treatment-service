using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Model.Entities;
using MediTrack.TreatmentService.API.TreatmentManagement.Interfaces.REST.Resources;

namespace MediTrack.TreatmentService.API.TreatmentManagement.Interfaces.REST.Transform;

public static class MedicationCatalogResourceFromEntityAssembler
{
    public static MedicationCatalogResource ToResourceFromEntity(MedicationCatalog entity) =>
        new(entity.Id, entity.OfficialName, entity.Synonyms, entity.Category);
}
