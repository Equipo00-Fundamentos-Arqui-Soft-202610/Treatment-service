using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Model.Entities;

namespace MediTrack.TreatmentService.API.TreatmentManagement.Domain.Repositories;

public interface IMedicationCatalogRepository
{
    Task<bool> ExistsByIdAsync(int id);
    Task<MedicationCatalog?> FindByNameAsync(string officialName);
    Task<IEnumerable<MedicationCatalog>> FindAllAsync();
    Task<IEnumerable<MedicationCatalog>> SearchAsync(string searchTerm);
    Task AddAsync(MedicationCatalog medicationCatalog);
}