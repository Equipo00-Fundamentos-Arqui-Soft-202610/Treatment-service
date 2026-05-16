using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Model.Aggregates;
using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Model.Commands;

namespace MediTrack.TreatmentService.API.TreatmentManagement.Domain.Services;

public interface IPrescriptionCommandService
{
    Task<Prescription?> Handle(CreatePrescriptionCommand command);
}