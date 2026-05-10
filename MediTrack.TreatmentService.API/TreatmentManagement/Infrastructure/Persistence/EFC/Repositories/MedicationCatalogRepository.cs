using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Repositories;
using MediTrack.TreatmentService.API.TreatmentManagement.Infrastructure.Persistence.EFC.Configuration;
using Microsoft.EntityFrameworkCore;

namespace MediTrack.TreatmentService.API.TreatmentManagement.Infrastructure.Persistence.EFC.Repositories;

public class MedicationCatalogRepository : IMedicationCatalogRepository
{
    private readonly AppDbContext _context;

    public MedicationCatalogRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ExistsByIdAsync(int id)
    {
        return await _context.MedicationCatalog.AnyAsync(catalog => catalog.Id == id);
    }
}