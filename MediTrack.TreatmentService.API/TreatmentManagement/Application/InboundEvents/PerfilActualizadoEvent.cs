namespace MediTrack.TreatmentService.API.TreatmentManagement.Application.InboundEvents;

/// <summary>
/// Contrato del evento publicado por el Identity Service cuando un paciente edita
/// su perfil después del registro. Debe mantenerse en sincronía con
/// meditrack-identity-service/.../PerfilActualizadoEvent.cs.
/// </summary>
public sealed record PerfilActualizadoEvent(
    Guid EventId,
    DateTime OccurredAtUtc,
    int PatientId,
    string FullName,
    string Email,
    string? Dni,
    DateTime? DateOfBirth
);
