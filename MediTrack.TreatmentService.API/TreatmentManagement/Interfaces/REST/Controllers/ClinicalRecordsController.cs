using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Services;
using MediTrack.TreatmentService.API.TreatmentManagement.Interfaces.REST.Resources;
using MediTrack.TreatmentService.API.TreatmentManagement.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace MediTrack.TreatmentService.API.TreatmentManagement.Interfaces.REST.Controllers;

[ApiController]
[Route("api/v1/clinical-records")]
public class ClinicalRecordsController : ControllerBase
{
    private readonly IClinicalRecordCommandService _clinicalRecordCommandService;

    public ClinicalRecordsController(IClinicalRecordCommandService clinicalRecordCommandService)
    {
        _clinicalRecordCommandService = clinicalRecordCommandService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateClinicalRecord([FromBody] CreateClinicalRecordResource resource)
    {
        try
        {
            var command = CreateClinicalRecordCommandFromResourceAssembler.ToCommandFromResource(resource);

            var clinicalRecord = await _clinicalRecordCommandService.Handle(command);

            if (clinicalRecord is null)
                return BadRequest();

            return Created(string.Empty, new
            {
                clinicalRecord.Id,
                clinicalRecord.PatientId,
                clinicalRecord.UploadedBy,
                clinicalRecord.DatasetSource,
                clinicalRecord.FileUrl,
                clinicalRecord.ProcessingStatus,
                clinicalRecord.UploadedAt
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