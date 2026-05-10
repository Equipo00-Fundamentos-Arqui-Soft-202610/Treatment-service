namespace MediTrack.TreatmentService.API.TreatmentManagement.Domain.Model.Entities;

public class ClinicalRecord
{
    public int Id { get; private set; }
    public int PatientId { get; private set; }
    public int UploadedBy { get; private set; }
    public string? DatasetSource { get; private set; }
    public string? FileUrl { get; private set; }
    public string ProcessingStatus { get; private set; }
    public DateTime UploadedAt { get; private set; }

    protected ClinicalRecord()
    {
        ProcessingStatus = string.Empty;
    }

    public ClinicalRecord(
        int patientId,
        int uploadedBy,
        string? datasetSource,
        string? fileUrl)
    {
        PatientId = patientId;
        UploadedBy = uploadedBy;
        DatasetSource = datasetSource;
        FileUrl = fileUrl;
        ProcessingStatus = "Processed";
        UploadedAt = DateTime.UtcNow;
    }
}