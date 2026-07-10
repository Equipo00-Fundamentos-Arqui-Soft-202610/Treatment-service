# MediTrack - Treatment Service

Microservicio de tratamientos (recetas, medicamentos, catálogo) de MediTrack.

## Secretos en desarrollo local

`Jwt:Key` está vacío en `appsettings.json` a propósito -- es compartido con el
Gateway, Identity Service, Reminder-Service y FollowUp-Service. Cada dev lo
configura una vez en su máquina:

```bash
dotnet user-secrets set "Jwt:Key" "<pedile la clave al equipo>" --project MediTrack.TreatmentService.API
```

En producción esa misma variable se setea como `Jwt__Key` en el entorno del
proveedor de deploy (Render, etc.) -- nunca en un archivo del repo.

## Ejecución local

```bash
dotnet run --project MediTrack.TreatmentService.API
```
