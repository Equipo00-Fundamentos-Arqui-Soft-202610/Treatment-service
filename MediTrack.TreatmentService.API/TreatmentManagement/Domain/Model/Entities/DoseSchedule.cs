namespace MediTrack.TreatmentService.API.TreatmentManagement.Domain.Model.Entities;

public class DoseSchedule
{
    public int Id { get; private set; }
    public int MedicationId { get; private set; }
    public TimeOnly ScheduledTime { get; private set; }
    public bool IsActive { get; private set; }

    public Medication Medication { get; private set; }

    protected DoseSchedule()
    {
        Medication = null!;
    }

    public DoseSchedule(TimeOnly scheduledTime)
    {
        ScheduledTime = scheduledTime;
        IsActive = true;
    }
}