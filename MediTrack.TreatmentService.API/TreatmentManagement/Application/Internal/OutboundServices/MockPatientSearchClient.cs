using MediTrack.TreatmentService.API.TreatmentManagement.Interfaces.REST.Resources;

namespace MediTrack.TreatmentService.API.TreatmentManagement.Application.Internal.OutboundServices;

public class MockPatientSearchClient : IPatientSearchClient
{
    private readonly List<PatientSearchResultResource> _patients =
    [
        new(1, "Juan Perez", "12345678", 45, "Active"),
        new(2, "Maria Lopez", "87654321", 38, "Active"),
        new(3, "Carlos Ramirez", "45678912", 52, "Inactive")
    ];

    public Task<IEnumerable<PatientSearchResultResource>> SearchAsync(string query)
    {
        // TODO: Replace this mock search with an HTTP client call
        // to the Identity/Profile microservice when it is available.

        var normalizedQuery = query.Trim().ToLower();

        var results = _patients.Where(patient =>
            patient.Dni.Contains(normalizedQuery) ||
            patient.FullName.ToLower().Contains(normalizedQuery)
        );

        return Task.FromResult(results);
    }
}