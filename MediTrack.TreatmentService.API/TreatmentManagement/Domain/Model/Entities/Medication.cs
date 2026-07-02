using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Model.Aggregates;

namespace MediTrack.TreatmentService.API.TreatmentManagement.Domain.Model.Entities;

public class Medication
{
    public int Id { get; private set; }
    public int PrescriptionId { get; private set; }
    public int CatalogId { get; private set; }
    public string Dose { get; private set; }
    public int FrequencyHours { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public int StockCount { get; private set; }
    public int StockAlertThreshold { get; private set; }
    public bool IsActive { get; private set; } = true;

    public Prescription Prescription { get; private set; }
    public MedicationCatalog MedicationCatalog { get; private set; }
    public ICollection<DoseSchedule> DoseSchedules { get; private set; }

    protected Medication()
    {
        Dose = string.Empty;
        IsActive = true;
        Prescription = null!;
        MedicationCatalog = null!;
        DoseSchedules = new List<DoseSchedule>();
    }

    public Medication(
        int catalogId,
        string dose,
        int frequencyHours,
        DateTime startDate,
        DateTime? endDate,
        int stockCount,
        int stockAlertThreshold)
    {
        CatalogId = catalogId;
        Dose = dose;
        FrequencyHours = frequencyHours;
        StartDate = startDate;
        EndDate = endDate;
        StockCount = stockCount;
        StockAlertThreshold = stockAlertThreshold;
        DoseSchedules = new List<DoseSchedule>();
    }

    public void Cancel()
    {
        IsActive = false;
    }

    public void Update(
        string dose,
        int frequencyHours,
        DateTime startDate,
        DateTime? endDate,
        int stockCount,
        int stockAlertThreshold)
    {
        Dose = dose;
        FrequencyHours = frequencyHours;
        StartDate = startDate;
        EndDate = endDate;
        StockCount = stockCount;
        StockAlertThreshold = stockAlertThreshold;
    }
}