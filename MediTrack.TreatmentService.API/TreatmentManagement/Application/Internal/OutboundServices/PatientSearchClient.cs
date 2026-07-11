using MediTrack.TreatmentService.API.TreatmentManagement.Infrastructure.Persistence.EFC.Configuration;
using MediTrack.TreatmentService.API.TreatmentManagement.Interfaces.REST.Resources;
using Microsoft.EntityFrameworkCore;

namespace MediTrack.TreatmentService.API.TreatmentManagement.Application.Internal.OutboundServices;

/// <summary>
/// Busca pacientes en la proyección local alimentada por el evento
/// PacienteRegistrado del Identity Service (ver PacienteRegistradoConsumerHostedService).
/// </summary>
public class PatientSearchClient : IPatientSearchClient
{
    private readonly AppDbContext _context;

    public PatientSearchClient(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PatientSearchResultResource>> SearchAsync(string query)
    {
        var q = query.Trim().ToLower();
        var patients = await _context.Patients
            .Where(p =>
                p.FullName.ToLower().Contains(q)
                || p.Email.ToLower().Contains(q)
                || (p.Dni != null && p.Dni.ToLower().Contains(q)))
            .ToListAsync();

        return patients.Select(p =>
        {
            int? age = p.DateOfBirth.HasValue
                ? (int)((DateTime.UtcNow - p.DateOfBirth.Value).TotalDays / 365.25)
                : null;
            return new PatientSearchResultResource(p.Id, p.FullName, p.Email, p.Dni, age);
        });
    }
}
