using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Model.Commands;
using MediTrack.TreatmentService.API.TreatmentManagement.Interfaces.REST.Resources;

namespace MediTrack.TreatmentService.API.TreatmentManagement.Interfaces.REST.Transform;

public static class CreateClinicalRecordCommandFromResourceAssembler
{
    public static CreateClinicalRecordCommand ToCommandFromResource(CreateClinicalRecordResource resource)
    {
        return new CreateClinicalRecordCommand(
            resource.PatientId,
            resource.UploadedBy,
            resource.DatasetSource,
            resource.FileUrl
        );
    }
}