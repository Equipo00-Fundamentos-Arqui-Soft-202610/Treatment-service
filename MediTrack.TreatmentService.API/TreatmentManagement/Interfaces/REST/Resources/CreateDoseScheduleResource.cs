namespace MediTrack.TreatmentService.API.TreatmentManagement.Interfaces.REST.Resources;

public record CreateDoseScheduleResource(
    TimeSpan ScheduledTime
);