namespace MediTrack.TreatmentService.API.TreatmentManagement.Application.Internal.OutboundServices;

public interface IPatientValidationClient
{
    Task<bool> ExistsByIdAsync(int patientId);
}