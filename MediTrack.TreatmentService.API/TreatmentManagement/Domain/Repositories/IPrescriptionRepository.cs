using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Model.Aggregates;

namespace MediTrack.TreatmentService.API.TreatmentManagement.Domain.Repositories;

public interface IPrescriptionRepository
{
    Task AddAsync(Prescription prescription);
}