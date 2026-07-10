namespace MediTrack.TreatmentService.API.TreatmentManagement.Application.InboundEvents;

/// <summary>
/// Contrato del evento publicado por el Identity Service cuando un paciente
/// completa el registro. Debe mantenerse en sincronía con
/// meditrack-identity-service/.../PacienteRegistradoEvent.cs.
/// </summary>
public sealed record PacienteRegistradoEvent(
    Guid EventId,
    DateTime OccurredAtUtc,
    int PatientId,
    string FullName,
    string Email,
    string? Dni = null,
    DateTime? DateOfBirth = null
);
