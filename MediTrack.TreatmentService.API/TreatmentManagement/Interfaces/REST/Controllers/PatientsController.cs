using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Services;

namespace MediTrack.TreatmentService.API.TreatmentManagement.Interfaces.REST.Controllers;

[ApiController]
[Route("api/v1/patients")]
[Authorize]
public class PatientsController : ControllerBase
{
    private readonly IPatientQueryService _patientQueryService;

    public PatientsController(IPatientQueryService patientQueryService)
    {
        _patientQueryService = patientQueryService;
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchPatients([FromQuery] string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return BadRequest(new { message = "Query parameter is required" });

        try
        {
            var patients = await _patientQueryService.SearchAsync(query);

            if (!patients.Any())
            {
                return NotFound(new
                {
                    message = "No se encontraron pacientes"
                });
            }

            return Ok(patients);
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