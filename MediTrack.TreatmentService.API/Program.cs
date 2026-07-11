using System.Text;
using MediTrack.TreatmentService.API.TreatmentManagement.Application.Internal.CommandServices;
using MediTrack.TreatmentService.API.TreatmentManagement.Application.Internal.EventHandlers;
using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Repositories;
using MediTrack.TreatmentService.API.TreatmentManagement.Domain.Services;
using MediTrack.TreatmentService.API.TreatmentManagement.Infrastructure.Messaging;
using MediTrack.TreatmentService.API.TreatmentManagement.Infrastructure.Persistence.EFC.Configuration;
using MediTrack.TreatmentService.API.TreatmentManagement.Infrastructure.Persistence.EFC.Repositories;
using MediTrack.TreatmentService.API.TreatmentManagement.Application.Internal.OutboundServices;
using MediTrack.TreatmentService.API.TreatmentManagement.Application.Internal.QueryServices;
using MediTrack.TreatmentService.API.TreatmentManagement.Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// JWT authentication -- valores reales vía user-secrets en desarrollo, vía
// variables de entorno en producción. Nunca en appsettings.json (ver README).
builder.Services.AddOptions<JwtOptions>()
    .Bind(builder.Configuration.GetSection(JwtOptions.SectionName))
    .Validate(o => !string.IsNullOrWhiteSpace(o.Key), "Jwt:Key es obligatorio")
    .Validate(o => !string.IsNullOrWhiteSpace(o.Issuer), "Jwt:Issuer es obligatorio")
    .Validate(o => !string.IsNullOrWhiteSpace(o.Audience), "Jwt:Audience es obligatorio")
    .ValidateOnStart();

var jwtOptions = builder.Configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>()
    ?? throw new InvalidOperationException("Falta la sección 'Jwt' en la configuración.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtOptions.Audience,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
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


builder.Services.AddScoped<IPatientValidationClient, PatientValidationClient>();

builder.Services.AddScoped<IPatientSearchClient, PatientSearchClient>();
builder.Services.AddScoped<IPatientQueryService, PatientQueryService>();

builder.Services.AddScoped<IMedicationRepository, MedicationRepository>();
builder.Services.AddScoped<IMedicationCommandService, MedicationCommandService>();

builder.Services.AddScoped<IMedicationQueryService, MedicationQueryService>();

// Messaging -- patrón Outbox: los eventos se persisten en la misma BD que el
// cambio de dominio y se entregan a RabbitMQ en background (no se pierden si
// el broker está caído justo al publicar).
builder.Services.AddScoped<IEventPublisher, OutboxEventPublisher>();
builder.Services.AddHostedService<OutboxDispatcherHostedService>();
builder.Services.AddScoped<PacienteRegistradoEventHandler>();
builder.Services.AddHostedService<PacienteRegistradoConsumerHostedService>();
builder.Services.AddScoped<PerfilActualizadoEventHandler>();
builder.Services.AddHostedService<PerfilActualizadoConsumerHostedService>();

var app = builder.Build();

// Auto-migrate: crea la base de datos y aplica migraciones pendientes al arrancar.
// No debe tumbar todo el proceso si la BD no está disponible al arrancar (p. ej.
// durante un despliegue en el que el contenedor de MySQL todavía no respondió).
try
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
}
catch (Exception ex)
{
    app.Logger.LogCritical(ex, "No se pudieron aplicar las migraciones de base de datos al arrancar.");
}

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
