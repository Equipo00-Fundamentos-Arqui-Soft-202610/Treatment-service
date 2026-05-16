using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Model.Aggregates;
using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Repositories;
using MediTrack.TreatmentService.API.TreatmentManagement.Infrastructure.Persistence.EFC.Configuration;

namespace MediTrack.TreatmentService.API.TreatmentManagement.Infrastructure.Persistence.EFC.Repositories;

public class PrescriptionRepository : IPrescriptionRepository
{
    private readonly AppDbContext _context;

    public PrescriptionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Prescription prescription)
    {
        await _context.Prescriptions.AddAsync(prescription);
        await _context.SaveChangesAsync();
    }
}