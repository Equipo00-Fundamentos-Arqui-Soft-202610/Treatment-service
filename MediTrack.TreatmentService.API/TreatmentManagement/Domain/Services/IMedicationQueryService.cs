using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Model.Entities;

namespace MediTrack.TreatmentService.API.TreatmentManagement.Domain.Services;

public interface IMedicationQueryService
{
    Task<IEnumerable<Medication>> GetMedicationsByPatientIdAsync(int patientId);
}