using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Model.Entities;
using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Repositories;
using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Services;

namespace MediTrack.TreatmentService.API.TreatmentManagement.Application.Internal.QueryServices;

public class MedicationQueryService : IMedicationQueryService
{
    private readonly IMedicationRepository _medicationRepository;

    public MedicationQueryService(IMedicationRepository medicationRepository)
    {
        _medicationRepository = medicationRepository;
    }

    public async Task<IEnumerable<Medication>> GetMedicationsByPatientIdAsync(int patientId)
    {
        return await _medicationRepository.FindByPatientIdAsync(patientId);
    }
}