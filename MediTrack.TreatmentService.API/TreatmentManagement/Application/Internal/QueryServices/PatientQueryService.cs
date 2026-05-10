using MediTrack.TreatmentService.API.TreatmentManagement.Application.Internal.OutboundServices;
using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Services;
using MediTrack.TreatmentService.API.TreatmentManagement.Interfaces.REST.Resources;

namespace MediTrack.TreatmentService.API.TreatmentManagement.Application.Internal.QueryServices;

public class PatientQueryService : IPatientQueryService
{
    private readonly IPatientSearchClient _patientSearchClient;

    public PatientQueryService(IPatientSearchClient patientSearchClient)
    {
        _patientSearchClient = patientSearchClient;
    }

    public async Task<IEnumerable<PatientSearchResultResource>> SearchAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            throw new Exception("Search query is required");

        return await _patientSearchClient.SearchAsync(query);
    }
}