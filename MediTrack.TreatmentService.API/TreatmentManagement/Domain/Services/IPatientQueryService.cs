using MediTrack.TreatmentService.API.TreatmentManagement.Interfaces.REST.Resources;

namespace MediTrack.TreatmentService.API.TreatmentManagement.Domain.Services;

public interface IPatientQueryService
{
    Task<IEnumerable<PatientSearchResultResource>> SearchAsync(string query);
}