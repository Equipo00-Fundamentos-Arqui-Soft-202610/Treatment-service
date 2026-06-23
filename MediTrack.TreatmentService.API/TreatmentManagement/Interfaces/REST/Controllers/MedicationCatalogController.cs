using Microsoft.AspNetCore.Mvc;
using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Model.Queries;
using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Services;
using MediTrack.TreatmentService.API.TreatmentManagement.Interfaces.REST.Resources;
using MediTrack.TreatmentService.API.TreatmentManagement.Interfaces.REST.Transform;

namespace MediTrack.TreatmentService.API.TreatmentManagement.Interfaces.REST.Controllers;

[ApiController]
[Route("api/v1/medication-catalog")]
public class MedicationCatalogController : ControllerBase
{
    private readonly IMedicationCatalogCommandService _commandService;
    private readonly IMedicationCatalogQueryService _queryService;

    public MedicationCatalogController(
        IMedicationCatalogCommandService commandService,
        IMedicationCatalogQueryService queryService)
    {
        _commandService = commandService;
        _queryService = queryService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateMedicationCatalog([FromBody] CreateMedicationCatalogResource resource)
    {
        try
        {
            var command = CreateMedicationCatalogCommandFromResourceAssembler
                .ToCommandFromResource(resource);
            var entity = await _commandService.Handle(command);
            if (entity is null) return BadRequest();
            var result = MedicationCatalogResourceFromEntityAssembler
                .ToResourceFromEntity(entity);
            return CreatedAtAction(nameof(GetAll), new { id = result.Id }, result);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var entities = await _queryService.Handle(new GetAllMedicationCatalogQuery());
        var resources = entities.Select(MedicationCatalogResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return BadRequest(new { message = "Query parameter is required" });

        var entities = await _queryService.Handle(new SearchMedicationCatalogQuery(query));
        var resources = entities.Select(MedicationCatalogResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }
}
