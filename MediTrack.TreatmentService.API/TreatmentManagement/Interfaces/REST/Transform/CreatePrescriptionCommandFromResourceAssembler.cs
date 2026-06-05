using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Model.Commands;
using MediTrack.TreatmentService.API.TreatmentManagement.Interfaces.REST.Resources;

namespace MediTrack.TreatmentService.API.TreatmentManagement.Interfaces.REST.Transform;

public static class CreatePrescriptionCommandFromResourceAssembler
{
    public static CreatePrescriptionCommand ToCommandFromResource(CreatePrescriptionResource resource)
    {
        return new CreatePrescriptionCommand(
            resource.PatientId,
            resource.TechnicalStaffId,
            resource.Notes,
            resource.Medications.Select(medication =>
                new CreateMedicationCommand(
                    medication.CatalogId,
                    medication.Dose,
                    medication.FrequencyHours,
                    medication.StartDate,
                    medication.EndDate,
                    medication.StockCount,
                    medication.StockAlertThreshold,
                    medication.DoseSchedules.Select(schedule =>
                        new CreateDoseScheduleCommand(
                            schedule.ScheduledTime
                        )
                    ).ToList()
                )
            ).ToList()
        );
    }
}