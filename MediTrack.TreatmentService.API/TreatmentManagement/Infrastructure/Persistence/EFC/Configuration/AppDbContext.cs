using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Model.Aggregates;
using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Model.Entities;
using MediTrack.TreatmentService.API.TreatmentManagement.Infrastructure.Persistence.EFC;
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
    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<ProcessedEvent> ProcessedEvents => Set<ProcessedEvent>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();


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
                .HasColumnType("time")
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

        builder.Entity<Patient>(entity =>
        {
            entity.ToTable("patients");

            entity.HasKey(p => p.Id);

            entity.Property(p => p.Id)
                .ValueGeneratedNever();

            entity.Property(p => p.FullName)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(p => p.Email)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(p => p.RegisteredAtUtc)
                .IsRequired();
        });

        builder.Entity<ProcessedEvent>(entity =>
        {
            entity.ToTable("processed_events");

            entity.HasKey(e => e.EventId);

            entity.Property(e => e.EventId)
                .HasConversion(g => g.ToByteArray(), b => new Guid(b))
                .HasColumnType("binary(16)");

            entity.Property(e => e.EventType)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(e => e.ProcessedAtUtc)
                .IsRequired();
        });

        builder.Entity<OutboxMessage>(entity =>
        {
            entity.ToTable("outbox_message");

            entity.HasKey(m => m.Id);

            entity.Property(m => m.Id)
                .HasConversion(g => g.ToByteArray(), b => new Guid(b))
                .HasColumnType("binary(16)");

            entity.Property(m => m.EventType)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(m => m.Payload)
                .HasColumnType("json")
                .IsRequired();

            entity.Property(m => m.OccurredAtUtc)
                .IsRequired();

            entity.Property(m => m.LastError)
                .HasMaxLength(500);

            entity.HasIndex(m => m.ProcessedAtUtc);
        });
    }
}
