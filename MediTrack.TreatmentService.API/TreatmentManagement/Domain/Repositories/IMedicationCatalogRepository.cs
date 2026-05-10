namespace MediTrack.TreatmentService.API.TreatmentManagement.Domain.Repositories;

public interface IMedicationCatalogRepository
{
    Task<bool> ExistsByIdAsync(int id);
}