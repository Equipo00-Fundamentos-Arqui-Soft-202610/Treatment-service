using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Model.Entities;
using MediTrack.TreatmentService.API.TreatmentManagement.Interfaces.REST.Resources;

namespace MediTrack.TreatmentService.API.TreatmentManagement.Interfaces.REST.Transform;

public static class MedicationResourceFromEntityAssembler
{
    public static MedicationResource ToResourceFromEntity(Medication entity)
    {
        return new MedicationResource(
            entity.Id,
            entity.PrescriptionId,
            entity.CatalogId,
            entity.MedicationCatalog.OfficialName,
            entity.MedicationCatalog.Category,
            entity.Dose,
            entity.FrequencyHours,
            entity.StartDate,
            entity.EndDate,
            entity.StockCount,
            entity.StockAlertThreshold,
            entity.DoseSchedules
                .Where(schedule => schedule.IsActive)
                .Select(schedule => schedule.ScheduledTime.ToString(@"hh\:mm"))
                .ToList()
        );
    }
}