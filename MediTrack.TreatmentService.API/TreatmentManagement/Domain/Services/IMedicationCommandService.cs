using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Model.Commands;
using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Model.Entities;

namespace MediTrack.TreatmentService.API.TreatmentManagement.Domain.Services;

public interface IMedicationCommandService
{
    Task<Medication?> Handle(UpdateMedicationCommand command);
    Task<Medication?> Handle(CancelMedicationCommand command);
}