using Microsoft.EntityFrameworkCore;
using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Model.Entities;
using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Repositories;
using MediTrack.TreatmentService.API.TreatmentManagement.Infrastructure.Persistence.EFC.Configuration;

namespace MediTrack.TreatmentService.API.TreatmentManagement.Infrastructure.Persistence.EFC.Repositories;

public class MedicationRepository : IMedicationRepository
{
    private readonly AppDbContext _context;

    public MedicationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Medication?> FindByIdAsync(int id)
    {
        return await _context.Medications
            .FirstOrDefaultAsync(medication => medication.Id == id);
    }
    
    public async Task<IEnumerable<Medication>> FindByPatientIdAsync(int patientId)
    {
        return await _context.Medications
            .Include(medication => medication.Prescription)
            .Include(medication => medication.MedicationCatalog)
            .Include(medication => medication.DoseSchedules)
            .Where(medication => medication.Prescription.PatientId == patientId)
            .ToListAsync();
    }

    public async Task UpdateAsync(Medication medication)
    {
        _context.Medications.Update(medication);
        await _context.SaveChangesAsync();
    }
}