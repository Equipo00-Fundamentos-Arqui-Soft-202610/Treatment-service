using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Model.Commands;
using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Model.Entities;
using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Repositories;
using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Services;

namespace MediTrack.TreatmentService.API.TreatmentManagement.Application.Internal.CommandServices;

public class MedicationCatalogCommandService : IMedicationCatalogCommandService
{
    private readonly IMedicationCatalogRepository _medicationCatalogRepository;

    public MedicationCatalogCommandService(IMedicationCatalogRepository medicationCatalogRepository)
    {
        _medicationCatalogRepository = medicationCatalogRepository;
    }

    public async Task<MedicationCatalog?> Handle(CreateMedicationCatalogCommand command)
    {
        if (string.IsNullOrWhiteSpace(command.OfficialName))
            throw new ArgumentException("OfficialName is required");

        var existing = await _medicationCatalogRepository.FindByNameAsync(command.OfficialName);
        if (existing is not null)
            throw new InvalidOperationException("A medication with this name already exists in the catalog");

        var entity = new MedicationCatalog(command.OfficialName, command.Synonyms, command.Category);
        await _medicationCatalogRepository.AddAsync(entity);
        return entity;
    }
}
