namespace MediTrack.TreatmentService.API.TreatmentManagement.Domain.Model.Entities;

public class MedicationCatalog
{
    public int Id { get; private set; }
    public string OfficialName { get; private set; }
    public string? Synonyms { get; private set; }
    public string? Category { get; private set; }

    public ICollection<Medication> Medications { get; private set; }

    protected MedicationCatalog()
    {
        OfficialName = string.Empty;
        Medications = new List<Medication>();
    }

    public MedicationCatalog(string officialName, string? synonyms, string? category)
    {
        OfficialName = officialName;
        Synonyms = synonyms;
        Category = category;
        Medications = new List<Medication>();
    }
}