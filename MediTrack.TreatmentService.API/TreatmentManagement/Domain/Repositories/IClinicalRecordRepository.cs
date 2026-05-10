using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Model.Entities;

namespace MediTrack.TreatmentService.API.TreatmentManagement.Domain.Repositories;

public interface IClinicalRecordRepository
{
    Task AddAsync(ClinicalRecord clinicalRecord);
}