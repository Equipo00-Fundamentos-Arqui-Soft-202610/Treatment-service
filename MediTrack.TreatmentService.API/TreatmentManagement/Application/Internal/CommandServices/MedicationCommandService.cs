using MediTrack.TreatmentService.API.TreatmentManagement.Application.OutboundEvents;
using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Model.Commands;
using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Model.Entities;
using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Repositories;
using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Services;
using MediTrack.TreatmentService.API.TreatmentManagement.Infrastructure.Messaging;

namespace MediTrack.TreatmentService.API.TreatmentManagement.Application.Internal.CommandServices;

public class MedicationCommandService : IMedicationCommandService
{
    private readonly IMedicationRepository _medicationRepository;
    private readonly IEventPublisher _eventPublisher;
    private readonly ILogger<MedicationCommandService> _logger;

    public MedicationCommandService(
        IMedicationRepository medicationRepository,
        IEventPublisher eventPublisher,
        ILogger<MedicationCommandService> logger)
    {
        _medicationRepository = medicationRepository;
        _eventPublisher = eventPublisher;
        _logger = logger;
    }

    public async Task<Medication?> Handle(UpdateMedicationCommand command)
    {
        if (!command.AuthorizedByTechnicalStaff)
            throw new Exception("Solo el personal técnico puede editar");

        ValidateUpdateCommand(command);

        var medication = await _medicationRepository.FindByIdAsync(command.MedicationId);

        if (medication is null)
            throw new Exception("Medication not found");

        medication.Update(
            command.Dose,
            command.FrequencyHours,
            command.StartDate,
            command.EndDate,
            command.StockCount,
            command.StockAlertThreshold
        );

        await _medicationRepository.UpdateAsync(medication);

        await PublishMedicationUpdatedEventAsync(medication);

        return medication;
    }

    private async Task PublishMedicationUpdatedEventAsync(Medication medication)
    {
        try
        {
            await _eventPublisher.PublishAsync("MedicationUpdated", new MedicationUpdatedEvent
            {
                MedicationId = medication.Id,
                PatientId = medication.Prescription.PatientId,
                Dose = medication.Dose,
                FrequencyHours = medication.FrequencyHours,
                StartDate = medication.StartDate,
                EndDate = medication.EndDate,
                StockCount = medication.StockCount,
                StockAlertThreshold = medication.StockAlertThreshold
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to publish MedicationUpdated event for MedicationId {MedicationId}", medication.Id);
        }
    }

    private async Task PublishMedicationCancelledEventAsync(Medication medication)
    {
        try
        {
            await _eventPublisher.PublishAsync("MedicationCancelled", new MedicationCancelledEvent
            {
                MedicationId = medication.Id,
                PatientId = medication.Prescription.PatientId
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to publish MedicationCancelled event for MedicationId {MedicationId}", medication.Id);
        }
    }

    public async Task<Medication?> Handle(CancelMedicationCommand command)
    {
        if (!command.AuthorizedByTechnicalStaff)
            throw new Exception("Solo el personal técnico puede cancelar");

        var medication = await _medicationRepository.FindByIdAsync(command.MedicationId);

        if (medication is null)
            throw new Exception("Medication not found");

        medication.Cancel();

        await _medicationRepository.UpdateAsync(medication);

        await PublishMedicationCancelledEventAsync(medication);

        return medication;
    }

    private static void ValidateUpdateCommand(UpdateMedicationCommand command)
    {
        if (command.MedicationId <= 0)
            throw new Exception("MedicationId is required");

        if (string.IsNullOrWhiteSpace(command.Dose))
            throw new Exception("Dose is required");

        if (command.FrequencyHours <= 0)
            throw new Exception("Frequency hour must be greater than zero");

        if (command.StartDate == default)
            throw new Exception("Start date is required");

        if (command.EndDate.HasValue && command.EndDate.Value < command.StartDate)
            throw new Exception("End date cannot be earlier than start date");

        if (command.StockCount < 0)
            throw new Exception("Stock count cannot be negative");

        if (command.StockAlertThreshold < 0)
            throw new Exception("Stock alert threshold cannot be negative");
    }
}