FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5162

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["MediTrack.TreatmentService.API/MediTrack.TreatmentService.API.csproj", "MediTrack.TreatmentService.API/"]
RUN dotnet restore "MediTrack.TreatmentService.API/MediTrack.TreatmentService.API.csproj"
COPY . .
WORKDIR "/src/MediTrack.TreatmentService.API"
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MediTrack.TreatmentService.API.dll"]
