using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Services;
using MediTrack.TreatmentService.API.TreatmentManagement.Interfaces.REST.Resources;
using MediTrack.TreatmentService.API.TreatmentManagement.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediTrack.TreatmentService.API.TreatmentManagement.Interfaces.REST.Controllers;

[ApiController]
[Route("api/v1/prescriptions")]
[Authorize]
public class PrescriptionsController : ControllerBase
{
    private readonly IPrescriptionCommandService _prescriptionCommandService;

    public PrescriptionsController(IPrescriptionCommandService prescriptionCommandService)
    {
        _prescriptionCommandService = prescriptionCommandService;
    }

    [HttpPost]
    [Authorize(Roles = "TechnicalStaff")]
    public async Task<IActionResult> CreatePrescription([FromBody] CreatePrescriptionResource resource)
    {
        try
        {
            var command = CreatePrescriptionCommandFromResourceAssembler.ToCommandFromResource(resource);

            var prescription = await _prescriptionCommandService.Handle(command);

            if (prescription is null)
                return BadRequest();

            return Created(string.Empty, new
            {
                prescription.Id,
                prescription.PatientId,
                prescription.TechnicalStaffId,
                prescription.Status,
                prescription.Notes,
                prescription.CreatedAt
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