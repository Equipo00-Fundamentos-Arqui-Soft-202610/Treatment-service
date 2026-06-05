using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Model.Commands;
using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Model.Entities;
using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Repositories;
using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Services;

namespace MediTrack.TreatmentService.API.TreatmentManagement.Application.Internal.CommandServices;

public class MedicationCommandService : IMedicationCommandService
{
    private readonly IMedicationRepository _medicationRepository;

    public MedicationCommandService(IMedicationRepository medicationRepository)
    {
        _medicationRepository = medicationRepository;
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

        return medication;
    }

    public async Task<Medication?> Handle(CancelMedicationCommand command)
    {
        if (!command.AuthorizedByTechnicalStaff)
            throw new Exception("Solo el personal técnico puede cancelar");

        var medication = await _medicationRepository.FindByIdAsync(command.MedicationId);

        if (medication is null)
            throw new Exception("Medication not found");
        

        await _medicationRepository.UpdateAsync(medication);

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