using MediTrack.TreatmentService.API.TreatmentManagement.Application.Internal.CommandServices;
using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Repositories;
using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Services;
using MediTrack.TreatmentService.API.TreatmentManagement.Infrastructure.Persistence.EFC.Configuration;
using MediTrack.TreatmentService.API.TreatmentManagement.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database

builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseMySQL(connectionString);
});

// Dependency Injection

builder.Services.AddScoped<IPrescriptionRepository, PrescriptionRepository>();
builder.Services.AddScoped<IPrescriptionCommandService, PrescriptionCommandService>();

builder.Services.AddScoped<IMedicationCatalogRepository, MedicationCatalogRepository>();

builder.Services.AddScoped<IClinicalRecordRepository, ClinicalRecordRepository>();
builder.Services.AddScoped<IClinicalRecordCommandService, ClinicalRecordCommandService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();