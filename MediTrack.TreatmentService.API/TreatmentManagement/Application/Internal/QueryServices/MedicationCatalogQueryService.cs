using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Model.Entities;
using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Model.Queries;
using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Repositories;
using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Services;

namespace MediTrack.TreatmentService.API.TreatmentManagement.Application.Internal.QueryServices;

public class MedicationCatalogQueryService : IMedicationCatalogQueryService
{
    private readonly IMedicationCatalogRepository _medicationCatalogRepository;

    public MedicationCatalogQueryService(IMedicationCatalogRepository medicationCatalogRepository)
    {
        _medicationCatalogRepository = medicationCatalogRepository;
    }

    public async Task<IEnumerable<MedicationCatalog>> Handle(GetAllMedicationCatalogQuery query)
    {
        return await _medicationCatalogRepository.FindAllAsync();
    }

    public async Task<IEnumerable<MedicationCatalog>> Handle(SearchMedicationCatalogQuery query)
    {
        return await _medicationCatalogRepository.SearchAsync(query.SearchTerm);
    }
}
