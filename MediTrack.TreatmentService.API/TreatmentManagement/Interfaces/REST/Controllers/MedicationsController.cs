using Microsoft.AspNetCore.Mvc;
using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Services;
using MediTrack.TreatmentService.API.TreatmentManagement.Interfaces.REST.Resources;
using MediTrack.TreatmentService.API.TreatmentManagement.Interfaces.REST.Transform;

namespace MediTrack.TreatmentService.API.TreatmentManagement.Interfaces.REST.Controllers;

[ApiController]
[Route("api/v1/medications")]
public class MedicationsController : ControllerBase
{
    private readonly IMedicationCommandService _medicationCommandService;

    public MedicationsController(IMedicationCommandService medicationCommandService)
    {
        _medicationCommandService = medicationCommandService;
    }

    [HttpPut("{medicationId:int}")]
    public async Task<IActionResult> UpdateMedication(
        int medicationId,
        [FromBody] UpdateMedicationResource resource)
    {
        try
        {
            var command = UpdateMedicationCommandFromResourceAssembler.ToCommandFromResource(medicationId, resource);

            var medication = await _medicationCommandService.Handle(command);

            if (medication is null)
                return BadRequest();

            return Ok(new
            {
                medication.Id,
                medication.PrescriptionId,
                medication.CatalogId,
                medication.Dose,
                medication.FrequencyHour,
                medication.StartDate,
                medication.EndDate,
                medication.StockCount,
                medication.StockAlertThre,
                medication.IsActive
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }

    [HttpPatch("{medicationId:int}/cancel")]
    public async Task<IActionResult> CancelMedication(
        int medicationId,
        [FromBody] CancelMedicationResource resource)
    {
        try
        {
            var command = CancelMedicationCommandFromResourceAssembler.ToCommandFromResource(medicationId, resource);

            var medication = await _medicationCommandService.Handle(command);

            if (medication is null)
                return BadRequest();

            return Ok(new
            {
                medication.Id,
                medication.PrescriptionId,
                medication.CatalogId,
                medication.Dose,
                medication.FrequencyHour,
                medication.StartDate,
                medication.EndDate,
                medication.StockCount,
                medication.StockAlertThre,
                medication.IsActive
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }
}