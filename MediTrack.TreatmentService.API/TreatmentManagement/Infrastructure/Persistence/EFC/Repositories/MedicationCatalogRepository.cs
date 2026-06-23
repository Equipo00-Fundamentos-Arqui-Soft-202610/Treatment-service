using Microsoft.EntityFrameworkCore;
using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Model.Entities;
using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Repositories;
using MediTrack.TreatmentService.API.TreatmentManagement.Infrastructure.Persistence.EFC.Configuration;

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

    public async Task<MedicationCatalog?> FindByNameAsync(string officialName)
    {
        return await _context.MedicationCatalog
            .FirstOrDefaultAsync(catalog => catalog.OfficialName == officialName);
    }

    public async Task<IEnumerable<MedicationCatalog>> FindAllAsync()
    {
        return await _context.MedicationCatalog
            .OrderBy(catalog => catalog.OfficialName)
            .ToListAsync();
    }

    public async Task<IEnumerable<MedicationCatalog>> SearchAsync(string searchTerm)
    {
        var lower = searchTerm.ToLower();
        return await _context.MedicationCatalog
            .Where(catalog =>
                catalog.OfficialName.ToLower().Contains(lower) ||
                (catalog.Synonyms != null && catalog.Synonyms.ToLower().Contains(lower)))
            .OrderBy(catalog => catalog.OfficialName)
            .ToListAsync();
    }

    public async Task AddAsync(MedicationCatalog medicationCatalog)
    {
        await _context.MedicationCatalog.AddAsync(medicationCatalog);
        await _context.SaveChangesAsync();
    }
}
