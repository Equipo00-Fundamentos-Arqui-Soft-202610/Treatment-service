using MediTrack.TreatmentService.API.TreatmentManagement.Application.InboundEvents;
using MediTrack.TreatmentService.API.TreatmentManagement.Infrastructure.Persistence.EFC.Configuration;
using Microsoft.EntityFrameworkCore;

namespace MediTrack.TreatmentService.API.TreatmentManagement.Application.Internal.EventHandlers;

/// <summary>
/// Aplica <see cref="PerfilActualizadoEvent"/> a la proyección local de pacientes,
/// manteniéndola sincronizada con ediciones de perfil posteriores al registro.
/// </summary>
public sealed class PerfilActualizadoEventHandler
{
    private readonly AppDbContext _context;
    private readonly ILogger<PerfilActualizadoEventHandler> _logger;

    public PerfilActualizadoEventHandler(AppDbContext context, ILogger<PerfilActualizadoEventHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task HandleAsync(PerfilActualizadoEvent @event, CancellationToken cancellationToken = default)
    {
        var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Id == @event.PatientId, cancellationToken);
        if (patient is null)
        {
            _logger.LogWarning(
                "PerfilActualizado recibido para PatientId {PatientId}, que aún no existe en la proyección local; se omite.",
                @event.PatientId);
            return;
        }

        patient.UpdateProfile(@event.FullName, @event.Email, @event.Dni, @event.DateOfBirth);

        _logger.LogInformation(
            "Paciente {PatientId} actualizado en la proyección local de Treatment-service.", @event.PatientId);
    }
}
