namespace MediTrack.TreatmentService.API.TreatmentManagement.Application.Internal.OutboundServices;

public class MockPatientValidationClient : IPatientValidationClient
{
    public Task<bool> ExistsByIdAsync(int patientId)
    {
        // TODO: Replace this mock validation with an HTTP client call
        // to the Identity/Profile microservice when it is available.
        // Temporary rule for local development:
        // patientId = 1 exists, any other patientId does not exist.

        return Task.FromResult(patientId == 1);
    }
}