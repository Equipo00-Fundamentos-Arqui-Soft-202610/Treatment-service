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

            entity.Property(p => p.TechnicalStaffId)
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

            entity.Property(m => m.FrequencyHours)
                .IsRequired();

            entity.Property(m => m.StartDate)
                .IsRequired();

            entity.Property(m => m.StockCount)
                .IsRequired();

            entity.Property(m => m.StockAlertThreshold)
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
        
        
    }
}