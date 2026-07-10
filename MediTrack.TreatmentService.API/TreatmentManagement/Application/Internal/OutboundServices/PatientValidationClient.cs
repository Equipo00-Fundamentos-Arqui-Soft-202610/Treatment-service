using MediTrack.TreatmentService.API.TreatmentManagement.Infrastructure.Persistence.EFC.Configuration;
using Microsoft.EntityFrameworkCore;

namespace MediTrack.TreatmentService.API.TreatmentManagement.Application.Internal.OutboundServices;

/// <summary>
/// Valida pacientes contra la proyección local alimentada por el evento
/// PacienteRegistrado del Identity Service (ver PacienteRegistradoConsumerHostedService).
/// </summary>
public class PatientValidationClient : IPatientValidationClient
{
    private readonly AppDbContext _context;

    public PatientValidationClient(AppDbContext context)
    {
        _context = context;
    }

    public Task<bool> ExistsByIdAsync(int patientId)
    {
        return _context.Patients.AnyAsync(p => p.Id == patientId);
    }
}
