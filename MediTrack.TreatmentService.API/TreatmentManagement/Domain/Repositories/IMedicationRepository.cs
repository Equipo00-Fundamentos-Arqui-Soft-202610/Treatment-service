using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Model.Entities;

namespace MediTrack.TreatmentService.API.TreatmentManagement.Domain.Repositories;

public interface IMedicationRepository
{
    Task<Medication?> FindByIdAsync(int id);
    Task<IEnumerable<Medication>> FindByPatientIdAsync(int patientId);
    Task UpdateAsync(Medication medication);
}