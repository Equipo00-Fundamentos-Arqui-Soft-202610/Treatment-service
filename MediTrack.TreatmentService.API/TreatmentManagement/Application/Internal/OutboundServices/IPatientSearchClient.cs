using MediTrack.TreatmentService.API.TreatmentManagement.Interfaces.REST.Resources;

namespace MediTrack.TreatmentService.API.TreatmentManagement.Application.Internal.OutboundServices;

public interface IPatientSearchClient
{
    Task<IEnumerable<PatientSearchResultResource>> SearchAsync(string query);
}