using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Model.Commands;
using MediTrack.TreatmentService.API.TreatmentManagement.Interfaces.REST.Resources;

namespace MediTrack.TreatmentService.API.TreatmentManagement.Interfaces.REST.Transform;

public static class UpdateMedicationCommandFromResourceAssembler
{
    public static UpdateMedicationCommand ToCommandFromResource(
        int medicationId, UpdateMedicationResource resource, bool isAuthorizedByTechnicalStaff)
    {
        return new UpdateMedicationCommand(
            medicationId,
            resource.Dose,
            resource.FrequencyHours,
            resource.StartDate,
            resource.EndDate,
            resource.StockCount,
            resource.StockAlertThreshold,
            isAuthorizedByTechnicalStaff
        );
    }
}