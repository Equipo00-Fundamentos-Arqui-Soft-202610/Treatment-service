namespace MediTrack.TreatmentService.API.TreatmentManagement.Domain.Model.Commands;

public record CancelMedicationCommand(
    int MedicationId,
    bool AuthorizedByTechnicalStaff
);