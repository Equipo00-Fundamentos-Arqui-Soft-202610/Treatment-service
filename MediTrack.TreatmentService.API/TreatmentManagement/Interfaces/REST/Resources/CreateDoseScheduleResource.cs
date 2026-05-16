namespace MediTrack.TreatmentService.API.TreatmentManagement.Interfaces.REST.Resources;

public record CreateDoseScheduleResource(
    TimeOnly ScheduledTime
);