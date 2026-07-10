namespace MediTrack.TreatmentService.API.TreatmentManagement.Domain.Model.Entities;

/// <summary>
/// Proyección local de solo lectura de un paciente, alimentada por el evento
/// <c>PacienteRegistrado</c> del Identity Service. Evita que Treatment-service
/// dependa de una llamada síncrona a otro bounded context para validar o buscar
/// pacientes (CON-05: comunicación asíncrona entre microservicios).
/// </summary>
public class Patient
{
    public int Id { get; private set; }
    public string FullName { get; private set; }
    public string Email { get; private set; }
    public string? Dni { get; private set; }
    public DateTime? DateOfBirth { get; private set; }
    public DateTime RegisteredAtUtc { get; private set; }

    protected Patient()
    {
        FullName = string.Empty;
        Email = string.Empty;
    }

    public Patient(int id, string fullName, string email, DateTime registeredAtUtc, string? dni = null, DateTime? dateOfBirth = null)
    {
        Id = id;
        FullName = fullName;
        Email = email;
        RegisteredAtUtc = registeredAtUtc;
        Dni = dni;
        DateOfBirth = dateOfBirth;
    }

    /// <summary>Aplica un <c>PerfilActualizado</c>: mantiene la proyección al día con ediciones posteriores al registro.</summary>
    public void UpdateProfile(string fullName, string email, string? dni, DateTime? dateOfBirth)
    {
        FullName = fullName;
        Email = email;

        if (dni is not null)
            Dni = dni;

        if (dateOfBirth is not null)
            DateOfBirth = dateOfBirth;
    }
}
