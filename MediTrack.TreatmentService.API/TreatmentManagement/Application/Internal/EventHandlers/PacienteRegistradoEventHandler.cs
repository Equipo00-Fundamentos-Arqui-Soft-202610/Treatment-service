using MediTrack.TreatmentService.API.TreatmentManagement.Application.InboundEvents;
using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Model.Entities;
using MediTrack.TreatmentService.API.TreatmentManagement.Infrastructure.Persistence.EFC.Configuration;
using Microsoft.EntityFrameworkCore;

namespace MediTrack.TreatmentService.API.TreatmentManagement.Application.Internal.EventHandlers;

/// <summary>
/// Aplica <see cref="PacienteRegistradoEvent"/> a la proyección local de pacientes.
/// Idempotente por diseño del llamador (ver <c>ProcessedEvent</c> / patrón Inbox):
/// además, un mismo PatientId nunca se inserta dos veces gracias al chequeo previo.
/// </summary>
public sealed class PacienteRegistradoEventHandler
{
    private readonly AppDbContext _context;
    private readonly ILogger<PacienteRegistradoEventHandler> _logger;

    public PacienteRegistradoEventHandler(AppDbContext context, ILogger<PacienteRegistradoEventHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task HandleAsync(PacienteRegistradoEvent @event, CancellationToken cancellationToken = default)
    {
        var exists = await _context.Patients.AnyAsync(p => p.Id == @event.PatientId, cancellationToken);
        if (exists)
        {
            _logger.LogDebug("Paciente {PatientId} ya existe en la proyección local; se omite.", @event.PatientId);
            return;
        }

        var patient = new Patient(@event.PatientId, @event.FullName, @event.Email, @event.OccurredAtUtc);
        await _context.Patients.AddAsync(patient, cancellationToken);

        _logger.LogInformation(
            "Paciente {PatientId} ({FullName}) agregado a la proyección local de Treatment-service.",
            @event.PatientId, @event.FullName);
    }
}
