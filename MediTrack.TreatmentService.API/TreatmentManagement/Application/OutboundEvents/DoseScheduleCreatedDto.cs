namespace MediTrack.TreatmentService.API.TreatmentManagement.Application.OutboundEvents;

public record DoseScheduleCreatedDto(
    int DoseScheduleId,
    TimeSpan ScheduledTime,
    bool IsActive
);
