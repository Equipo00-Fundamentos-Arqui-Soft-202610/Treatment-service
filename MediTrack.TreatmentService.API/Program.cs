using System.Text;
using MediTrack.TreatmentService.API.TreatmentManagement.Application.Internal.CommandServices;
using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Repositories;
using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Services;
using MediTrack.TreatmentService.API.TreatmentManagement.Infrastructure.Messaging;
using MediTrack.TreatmentService.API.TreatmentManagement.Infrastructure.Persistence.EFC.Configuration;
using MediTrack.TreatmentService.API.TreatmentManagement.Infrastructure.Persistence.EFC.Repositories;
using MediTrack.TreatmentService.API.TreatmentManagement.Application.Internal.OutboundServices;
using MediTrack.TreatmentService.API.TreatmentManagement.Application.Internal.QueryServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// JWT authentication
var jwtSection = builder.Configuration.GetSection("Jwt");
var signingKey = jwtSection["Key"]
    ?? throw new InvalidOperationException("Falta la clave de firma JWT en 'Jwt:Key'.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSection["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwtSection["Audience"],
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)),
            ClockSkew = TimeSpan.FromSeconds(30)
        };
    });

builder.Services.AddAuthorization();

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
builder.Services.AddScoped<IMedicationCatalogCommandService, MedicationCatalogCommandService>();
builder.Services.AddScoped<IMedicationCatalogQueryService, MedicationCatalogQueryService>();


builder.Services.AddScoped<IPatientValidationClient, MockPatientValidationClient>();

builder.Services.AddScoped<IPatientSearchClient, MockPatientSearchClient>();
builder.Services.AddScoped<IPatientQueryService, PatientQueryService>();

builder.Services.AddScoped<IMedicationRepository, MedicationRepository>();
builder.Services.AddScoped<IMedicationCommandService, MedicationCommandService>();

builder.Services.AddScoped<IMedicationQueryService, MedicationQueryService>();

// Messaging
builder.Services.AddSingleton<IEventPublisher, RabbitMqPublisher>();

var app = builder.Build();

// Auto-migrate: crea la base de datos y aplica migraciones pendientes al arrancar
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
}

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();