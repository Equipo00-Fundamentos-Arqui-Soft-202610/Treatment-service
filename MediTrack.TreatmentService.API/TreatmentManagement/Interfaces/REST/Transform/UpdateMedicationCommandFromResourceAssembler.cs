using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Model.Commands;
using MediTrack.TreatmentService.API.TreatmentManagement.Interfaces.REST.Resources;

namespace MediTrack.TreatmentService.API.TreatmentManagement.Interfaces.REST.Transform;

public static class UpdateMedicationCommandFromResourceAssembler
{
    public static UpdateMedicationCommand ToCommandFromResource(int medicationId, UpdateMedicationResource resource)
    {
        return new UpdateMedicationCommand(
            medicationId,
            resource.Dose,
            resource.FrequencyHour,
            resource.StartDate,
            resource.EndDate,
            resource.StockCount,
            resource.StockAlertThre,
            resource.AuthorizedByTechnicalStaff
        );
    }
}