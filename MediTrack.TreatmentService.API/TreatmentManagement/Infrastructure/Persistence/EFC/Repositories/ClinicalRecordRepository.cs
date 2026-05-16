using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Model.Entities;
using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Repositories;
using MediTrack.TreatmentService.API.TreatmentManagement.Infrastructure.Persistence.EFC.Configuration;

namespace MediTrack.TreatmentService.API.TreatmentManagement.Infrastructure.Persistence.EFC.Repositories;

public class ClinicalRecordRepository : IClinicalRecordRepository
{
    private readonly AppDbContext _context;

    public ClinicalRecordRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(ClinicalRecord clinicalRecord)
    {
        await _context.ClinicalRecords.AddAsync(clinicalRecord);
        await _context.SaveChangesAsync();
    }
}