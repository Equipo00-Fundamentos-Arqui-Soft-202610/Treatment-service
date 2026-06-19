using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Model.Entities;
using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Model.Queries;

namespace MediTrack.TreatmentService.API.TreatmentManagement.Domain.Services;

public interface IMedicationCatalogQueryService
{
    Task<IEnumerable<MedicationCatalog>> Handle(GetAllMedicationCatalogQuery query);
    Task<IEnumerable<MedicationCatalog>> Handle(SearchMedicationCatalogQuery query);
}
