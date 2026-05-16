using MediTrack.TreatmentService.API.TreatmentManagement.Application.Internal.OutboundServices;
using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Model.Commands;
using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Model.Entities;
using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Repositories;
using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Services;

namespace MediTrack.TreatmentService.API.TreatmentManagement.Application.Internal.CommandServices;

public class ClinicalRecordCommandService : IClinicalRecordCommandService
{
    private readonly IClinicalRecordRepository _clinicalRecordRepository;
    private readonly IPatientValidationClient _patientValidationClient;

    public ClinicalRecordCommandService(
        IClinicalRecordRepository clinicalRecordRepository,
        IPatientValidationClient patientValidationClient)
    {
        _clinicalRecordRepository = clinicalRecordRepository;
        _patientValidationClient = patientValidationClient;
    }

    public async Task<ClinicalRecord?> Handle(CreateClinicalRecordCommand command)
    {
        await ValidateCommand(command);

        var clinicalRecord = new ClinicalRecord(
            command.PatientId,
            command.UploadedBy,
            command.DatasetSource,
            command.FileUrl
        );

        await _clinicalRecordRepository.AddAsync(clinicalRecord);

        return clinicalRecord;
    }

    private async Task ValidateCommand(CreateClinicalRecordCommand command)
    {
        if (command.PatientId <= 0)
            throw new Exception("PatientId is required");

        var patientExists = await _patientValidationClient.ExistsByIdAsync(command.PatientId);

        if (!patientExists)
            throw new Exception("Paciente no encontrado");

        if (command.UploadedBy <= 0)
            throw new Exception("UploadedBy is required");

        if (string.IsNullOrWhiteSpace(command.DatasetSource) && string.IsNullOrWhiteSpace(command.FileUrl))
            throw new Exception("Dataset source or file URL is required");
    }
}