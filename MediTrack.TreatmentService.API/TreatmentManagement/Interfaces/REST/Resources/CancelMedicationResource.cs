namespace MediTrack.TreatmentService.API.TreatmentManagement.Interfaces.REST.Resources;

public record CancelMedicationResource(
    bool AuthorizedByTechnicalStaff
);