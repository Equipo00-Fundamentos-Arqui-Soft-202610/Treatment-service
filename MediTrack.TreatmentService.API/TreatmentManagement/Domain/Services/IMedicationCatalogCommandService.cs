using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Model.Entities;
using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Model.Commands;

namespace MediTrack.TreatmentService.API.TreatmentManagement.Domain.Services;

public interface IMedicationCatalogCommandService
{
    Task<MedicationCatalog?> Handle(CreateMedicationCatalogCommand command);
}
