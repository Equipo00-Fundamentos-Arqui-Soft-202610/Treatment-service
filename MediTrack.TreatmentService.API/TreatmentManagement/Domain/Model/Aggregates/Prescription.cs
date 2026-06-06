using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Model.Entities;

namespace MediTrack.TreatmentService.API.TreatmentManagement.Domain.Model.Aggregates;

public class Prescription
{
    public int Id { get; private set; }
    public int PatientId { get; private set; }
    public int TechnicalStaffId { get; private set; }
    public string Status { get; private set; }
    public string? Notes { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public ICollection<Medication> Medications { get; private set; }

    protected Prescription()
    {
        Status = string.Empty;
        Medications = new List<Medication>();
    }

    public Prescription(int patientId, int technicalStaffId, string? notes)
    {
        PatientId = patientId;
        TechnicalStaffId = technicalStaffId;
        Status = "active";
        Notes = notes;
        CreatedAt = DateTime.UtcNow;
        Medications = new List<Medication>();
    }
}