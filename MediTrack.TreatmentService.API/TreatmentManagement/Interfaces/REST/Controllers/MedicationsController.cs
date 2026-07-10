using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Services;
using MediTrack.TreatmentService.API.TreatmentManagement.Interfaces.REST.Resources;
using MediTrack.TreatmentService.API.TreatmentManagement.Interfaces.REST.Transform;

namespace MediTrack.TreatmentService.API.TreatmentManagement.Interfaces.REST.Controllers;

[ApiController]
[Route("api/v1/medications")]
[Authorize]
public class MedicationsController : ControllerBase
{
    private readonly IMedicationCommandService _medicationCommandService;
    private readonly IMedicationQueryService _medicationQueryService;

    public MedicationsController(
        IMedicationCommandService medicationCommandService,
        IMedicationQueryService medicationQueryService)
    {
        _medicationCommandService = medicationCommandService;
        _medicationQueryService = medicationQueryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetMedicationsByPatientId([FromQuery] int patientId)
    {
        var medications = await _medicationQueryService.GetMedicationsByPatientIdAsync(patientId);

        var resources = medications
            .Select(MedicationResourceFromEntityAssembler.ToResourceFromEntity)
            .ToList();

        return Ok(resources);
    }

    [HttpPut("{medicationId:int}")]
    [Authorize(Roles = "TechnicalStaff")]
    public async Task<IActionResult> UpdateMedication(
        int medicationId,
        [FromBody] UpdateMedicationResource resource)
    {
        try
        {
            var command = UpdateMedicationCommandFromResourceAssembler.ToCommandFromResource(
                medicationId, resource, User.IsInRole("TechnicalStaff"));

            var medication = await _medicationCommandService.Handle(command);

            if (medication is null)
                return BadRequest();

            return Ok(new
            {
                medication.Id,
                medication.PrescriptionId,
                medication.CatalogId,
                medication.Dose,
                medication.FrequencyHours,
                medication.StartDate,
                medication.EndDate,
                medication.StockCount,
                medication.StockAlertThreshold,
                medication.IsActive,
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
    [Authorize(Roles = "TechnicalStaff")]
    public async Task<IActionResult> CancelMedication(int medicationId)
    {
        try
        {
            var command = CancelMedicationCommandFromResourceAssembler.ToCommandFromResource(
                medicationId, User.IsInRole("TechnicalStaff"));

            var medication = await _medicationCommandService.Handle(command);

            if (medication is null)
                return BadRequest();

            return Ok(new
            {
                medication.Id,
                medication.PrescriptionId,
                medication.CatalogId,
                medication.Dose,
                medication.FrequencyHours,
                medication.StartDate,
                medication.EndDate,
                medication.StockCount,
                medication.StockAlertThreshold,
                medication.IsActive,
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