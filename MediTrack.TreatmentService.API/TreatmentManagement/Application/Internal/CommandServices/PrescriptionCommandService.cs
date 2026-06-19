using MediTrack.TreatmentService.API.TreatmentManagement.Application.Internal.OutboundServices;
using MediTrack.TreatmentService.API.TreatmentManagement.Application.OutboundEvents;
using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Model.Aggregates;
using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Model.Commands;
using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Model.Entities;
using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Repositories;
using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Services;
using MediTrack.TreatmentService.API.TreatmentManagement.Infrastructure.Messaging;

namespace MediTrack.TreatmentService.API.TreatmentManagement.Application.Internal.CommandServices;

public class PrescriptionCommandService : IPrescriptionCommandService
{
    private readonly IPrescriptionRepository _prescriptionRepository;
    private readonly IMedicationCatalogRepository _medicationCatalogRepository;
    private readonly IPatientValidationClient _patientValidationClient;
    private readonly IEventPublisher _eventPublisher;
    private readonly ILogger<PrescriptionCommandService> _logger;

    public PrescriptionCommandService(
        IPrescriptionRepository prescriptionRepository,
        IMedicationCatalogRepository medicationCatalogRepository,
        IPatientValidationClient patientValidationClient,
        IEventPublisher eventPublisher,
        ILogger<PrescriptionCommandService> logger)
    {
        _prescriptionRepository = prescriptionRepository;
        _medicationCatalogRepository = medicationCatalogRepository;
        _patientValidationClient = patientValidationClient;
        _eventPublisher = eventPublisher;
        _logger = logger;
    }

    public async Task<Prescription?> Handle(CreatePrescriptionCommand command)
    {
        await ValidateCommand(command);

        var prescription = new Prescription(
            command.PatientId,
            command.TechnicalStaffId,
            command.Notes
        );

        foreach (var medicationCommand in command.Medications)
        {
            var medication = new Medication(
                medicationCommand.CatalogId,
                medicationCommand.Dose,
                medicationCommand.FrequencyHours,
                medicationCommand.StartDate,
                medicationCommand.EndDate,
                medicationCommand.StockCount,
                medicationCommand.StockAlertThreshold
            );

            foreach (var scheduleCommand in medicationCommand.DoseSchedules)
            {
                var schedule = new DoseSchedule(scheduleCommand.ScheduledTime);
                medication.DoseSchedules.Add(schedule);
            }

            prescription.Medications.Add(medication);
        }

        await _prescriptionRepository.AddAsync(prescription);

        await PublishPrescriptionCreatedEventAsync(prescription);

        return prescription;
    }

    private async Task PublishPrescriptionCreatedEventAsync(Prescription prescription)
    {
        try
        {
            var catalogs = await _medicationCatalogRepository.FindAllAsync();
            var catalogNames = catalogs.ToDictionary(c => c.Id, c => c.OfficialName);

            var medications = prescription.Medications.Select(med => new MedicationCreatedDto(
                med.Id,
                prescription.PatientId,
                prescription.Id,
                med.CatalogId,
                catalogNames.GetValueOrDefault(med.CatalogId, "Unknown"),
                med.Dose,
                med.FrequencyHours,
                med.StartDate,
                med.EndDate,
                med.StockCount,
                med.StockAlertThreshold,
                med.DoseSchedules.Select(schedule => new DoseScheduleCreatedDto(
                    schedule.Id,
                    schedule.ScheduledTime,
                    schedule.IsActive
                )).ToList()
            )).ToList();

            var integrationEvent = new PrescriptionCreatedEvent
            {
                PrescriptionId = prescription.Id,
                PatientId = prescription.PatientId,
                Medications = medications
            };

            await _eventPublisher.PublishAsync("PrescriptionCreated", integrationEvent);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to publish PrescriptionCreated event for PrescriptionId {PrescriptionId}. Prescription was saved successfully.",
                prescription.Id);
        }
    }

    private async Task ValidateCommand(CreatePrescriptionCommand command)
    {
        if (command.PatientId <= 0)
            throw new Exception("PatientId is required");

        var patientExists = await _patientValidationClient.ExistsByIdAsync(command.PatientId);

        if (!patientExists)
            throw new Exception("Paciente no encontrado");

        if (command.TechnicalStaffId <= 0)
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

            if (medication.FrequencyHours <= 0)
                throw new Exception("Frequency hour must be greater than zero");

            if (medication.StartDate == default)
                throw new Exception("Start date is required");

            if (medication.EndDate.HasValue && medication.EndDate.Value < medication.StartDate)
                throw new Exception("End date cannot be earlier than start date");

            if (medication.StockCount < 0)
                throw new Exception("Stock count cannot be negative");

            if (medication.StockAlertThreshold < 0)
                throw new Exception("Stock alert threshold cannot be negative");

            if (medication.DoseSchedules == null || !medication.DoseSchedules.Any())
                throw new Exception("Completa todos los horarios");
        }
    }
}