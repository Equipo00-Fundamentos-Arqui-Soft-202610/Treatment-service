using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Model.Aggregates;
using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Model.Commands;
using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Model.Entities;
using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Repositories;
using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Services;

namespace MediTrack.TreatmentService.API.TreatmentManagement.Application.Internal.CommandServices;

public class PrescriptionCommandService : IPrescriptionCommandService
{
    private readonly IPrescriptionRepository _prescriptionRepository;

    public PrescriptionCommandService(IPrescriptionRepository prescriptionRepository)
    {
        _prescriptionRepository = prescriptionRepository;
    }

    public async Task<Prescription?> Handle(CreatePrescriptionCommand command)
    {
        if (command.Medications == null || !command.Medications.Any())
            throw new Exception("The prescription must contain medications");

        foreach (var medication in command.Medications)
        {
            if (medication.DoseSchedules == null || !medication.DoseSchedules.Any())
                throw new Exception("Completa todos los horarios");
        }

        var prescription = new Prescription(
            command.PatientId,
            command.TechnicalId,
            command.Notes
        );

        foreach (var medicationCommand in command.Medications)
        {
            var medication = new Medication(
                medicationCommand.CatalogId,
                medicationCommand.Dose,
                medicationCommand.FrequencyHour,
                medicationCommand.StartDate,
                medicationCommand.EndDate,
                medicationCommand.StockCount,
                medicationCommand.StockAlertThre
            );

            foreach (var scheduleCommand in medicationCommand.DoseSchedules)
            {
                var schedule = new DoseSchedule(scheduleCommand.ScheduledTime);

                medication.DoseSchedules.Add(schedule);
            }

            prescription.Medications.Add(medication);
        }

        await _prescriptionRepository.AddAsync(prescription);

        return prescription;
    }
}