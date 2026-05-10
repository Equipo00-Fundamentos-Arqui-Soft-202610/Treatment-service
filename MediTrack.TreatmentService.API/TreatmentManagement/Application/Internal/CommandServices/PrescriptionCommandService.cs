using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Model.Aggregates;
using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Model.Commands;
using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Model.Entities;
using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Repositories;
using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Services;

namespace MediTrack.TreatmentService.API.TreatmentManagement.Application.Internal.CommandServices;

public class PrescriptionCommandService : IPrescriptionCommandService
{
    private readonly IPrescriptionRepository _prescriptionRepository;
    private readonly IMedicationCatalogRepository _medicationCatalogRepository;

    public PrescriptionCommandService(
        IPrescriptionRepository prescriptionRepository,
        IMedicationCatalogRepository medicationCatalogRepository)
    {
        _prescriptionRepository = prescriptionRepository;
        _medicationCatalogRepository = medicationCatalogRepository;
    }

    public async Task<Prescription?> Handle(CreatePrescriptionCommand command)
    {
        await ValidateCommand(command);

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

    private async Task ValidateCommand(CreatePrescriptionCommand command)
    {
        if (command.PatientId <= 0)
            throw new Exception("PatientId is required");

        if (command.TechnicalId <= 0)
            throw new Exception("TechnicalId is required");

        if (command.Medications == null || !command.Medications.Any())
            throw new Exception("The prescription must contain medications");

        foreach (var medication in command.Medications)
        {
            if (medication.CatalogId <= 0)
                throw new Exception("Medication catalog is required");

            var medicationExists = await _medicationCatalogRepository.ExistsByIdAsync(medication.CatalogId);

            if (!medicationExists)
                throw new Exception("Medication catalog not found");

            if (string.IsNullOrWhiteSpace(medication.Dose))
                throw new Exception("Dose is required");

            if (medication.FrequencyHour <= 0)
                throw new Exception("Frequency hour must be greater than zero");

            if (medication.StartDate == default)
                throw new Exception("Start date is required");

            if (medication.EndDate.HasValue && medication.EndDate.Value < medication.StartDate)
                throw new Exception("End date cannot be earlier than start date");

            if (medication.StockCount < 0)
                throw new Exception("Stock count cannot be negative");

            if (medication.StockAlertThre < 0)
                throw new Exception("Stock alert threshold cannot be negative");

            if (medication.DoseSchedules == null || !medication.DoseSchedules.Any())
                throw new Exception("Completa todos los horarios");
        }
    }
}