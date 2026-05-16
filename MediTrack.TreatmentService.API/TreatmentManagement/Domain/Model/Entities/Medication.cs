using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Model.Aggregates;

namespace MediTrack.TreatmentService.API.TreatmentManagement.Domain.Model.Entities;

public class Medication
{
    public int Id { get; private set; }
    public int PrescriptionId { get; private set; }
    public int CatalogId { get; private set; }
    public string Dose { get; private set; }
    public int FrequencyHour { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public int StockCount { get; private set; }
    public int StockAlertThre { get; private set; }
    public bool IsActive { get; private set; }

    public Prescription Prescription { get; private set; }
    public MedicationCatalog MedicationCatalog { get; private set; }
    public ICollection<DoseSchedule> DoseSchedules { get; private set; }

    protected Medication()
    {
        Dose = string.Empty;
        Prescription = null!;
        MedicationCatalog = null!;
        DoseSchedules = new List<DoseSchedule>();
        IsActive = true;
    }

    public Medication(
        int catalogId,
        string dose,
        int frequencyHour,
        DateTime startDate,
        DateTime? endDate,
        int stockCount,
        int stockAlertThre)
    {
        CatalogId = catalogId;
        Dose = dose;
        FrequencyHour = frequencyHour;
        StartDate = startDate;
        EndDate = endDate;
        StockCount = stockCount;
        StockAlertThre = stockAlertThre;
        IsActive = true;
        DoseSchedules = new List<DoseSchedule>();
    }

    public void Update(
        string dose,
        int frequencyHour,
        DateTime startDate,
        DateTime? endDate,
        int stockCount,
        int stockAlertThre)
    {
        Dose = dose;
        FrequencyHour = frequencyHour;
        StartDate = startDate;
        EndDate = endDate;
        StockCount = stockCount;
        StockAlertThre = stockAlertThre;
    }

    public void Cancel()
    {
        IsActive = false;
    }
}