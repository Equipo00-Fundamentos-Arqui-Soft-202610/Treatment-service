using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Model.Commands;

namespace MediTrack.TreatmentService.API.TreatmentManagement.Interfaces.REST.Transform;

public static class CancelMedicationCommandFromResourceAssembler
{
    public static CancelMedicationCommand ToCommandFromResource(int medicationId, bool isAuthorizedByTechnicalStaff)
    {
        return new CancelMedicationCommand(
            medicationId,
            isAuthorizedByTechnicalStaff
        );
    }
}