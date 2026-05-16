using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Model.Commands;
using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Model.Entities;

namespace MediTrack.TreatmentService.API.TreatmentManagement.Domain.Services;

public interface IClinicalRecordCommandService
{
    Task<ClinicalRecord?> Handle(CreateClinicalRecordCommand command);
}