using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Model.Commands;
using MediTrack.TreatmentService.API.TreatmentManagement.Interfaces.REST.Resources;

namespace MediTrack.TreatmentService.API.TreatmentManagement.Interfaces.REST.Transform;

public static class CancelMedicationCommandFromResourceAssembler
{
    public static CancelMedicationCommand ToCommandFromResource(int medicationId, CancelMedicationResource resource)
    {
        return new CancelMedicationCommand(
            medicationId,
            resource.AuthorizedByTechnicalStaff
        );
    }
}