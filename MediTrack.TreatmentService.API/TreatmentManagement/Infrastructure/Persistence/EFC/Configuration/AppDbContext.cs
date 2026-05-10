using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Model.Aggregates;
using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediTrack.TreatmentService.API.TreatmentManagement.Infrastructure.Persistence.EFC.Configuration;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Prescription> Prescriptions => Set<Prescription>();
    public DbSet<Medication> Medications => Set<Medication>();
    public DbSet<DoseSchedule> DoseSchedules => Set<DoseSchedule>();
    public DbSet<MedicationCatalog> MedicationCatalog => Set<MedicationCatalog>();
    public DbSet<ClinicalRecord> ClinicalRecords => Set<ClinicalRecord>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Prescription>(entity =>
        {
            entity.ToTable("prescriptions");

            entity.HasKey(p => p.Id);

            entity.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            entity.Property(p => p.PatientId)
                .IsRequired();

            entity.Property(p => p.TechnicalId)
                .IsRequired();

            entity.Property(p => p.Status)
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(p => p.Notes)
                .HasMaxLength(500);

            entity.Property(p => p.CreatedAt)
                .IsRequired();

            entity.HasMany(p => p.Medications)
                .WithOne(m => m.Prescription)
                .HasForeignKey(m => m.PrescriptionId);
        });

        builder.Entity<Medication>(entity =>
        {
            entity.ToTable("medications");

            entity.HasKey(m => m.Id);

            entity.Property(m => m.Id)
                .ValueGeneratedOnAdd();

            entity.Property(m => m.Dose)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(m => m.FrequencyHour)
                .IsRequired();

            entity.Property(m => m.StartDate)
                .IsRequired();

            entity.Property(m => m.StockCount)
                .IsRequired();

            entity.Property(m => m.StockAlertThre)
                .IsRequired();
            
            entity.Property(m => m.IsActive)
                .IsRequired();

            entity.HasOne(m => m.MedicationCatalog)
                .WithMany(c => c.Medications)
                .HasForeignKey(m => m.CatalogId);

            entity.HasMany(m => m.DoseSchedules)
                .WithOne(ds => ds.Medication)
                .HasForeignKey(ds => ds.MedicationId);
        });

        builder.Entity<DoseSchedule>(entity =>
        {
            entity.ToTable("dose_schedules");

            entity.HasKey(ds => ds.Id);

            entity.Property(ds => ds.Id)
                .ValueGeneratedOnAdd();

            entity.Property(ds => ds.ScheduledTime)
                .IsRequired();

            entity.Property(ds => ds.IsActive)
                .IsRequired();
        });

        builder.Entity<MedicationCatalog>(entity =>
        {
            entity.ToTable("medication_catalog");

            entity.HasKey(mc => mc.Id);

            entity.Property(mc => mc.Id)
                .ValueGeneratedOnAdd();

            entity.Property(mc => mc.OfficialName)
                .HasMaxLength(150)
                .IsRequired();

            entity.Property(mc => mc.Synonyms)
                .HasMaxLength(500);

            entity.Property(mc => mc.Category)
                .HasMaxLength(100);
        });
        
        builder.Entity<ClinicalRecord>(entity =>
        {
            entity.ToTable("clinical_records");

            entity.HasKey(cr => cr.Id);

            entity.Property(cr => cr.Id)
                .ValueGeneratedOnAdd();

            entity.Property(cr => cr.PatientId)
                .IsRequired();

            entity.Property(cr => cr.UploadedBy)
                .IsRequired();

            entity.Property(cr => cr.DatasetSource)
                .HasMaxLength(255);

            entity.Property(cr => cr.FileUrl)
                .HasMaxLength(500);

            entity.Property(cr => cr.ProcessingStatus)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(cr => cr.UploadedAt)
                .IsRequired();
        });
    }
}