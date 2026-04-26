namespace WorkshopAdmin.Shared.Enums;

public enum ServiceType : short
{
    Preventive = 0,   // Mantenimiento programado (cambios de aceite, filtros)
    Corrective = 1,   // Reparación de fallas o averías detectadas
    Emergency = 2,    // Atención inmediata por falla crítica (aplica recargo)
    Warranty = 3,     // Servicio sin costo por reclamo de trabajo previo
    Installation = 4  // Montaje de componentes nuevos o accesorios
}

public static class ServiceTypeExtensions
{
    public static string ToFriendlyName(this ServiceType type) => type switch
    {
        ServiceType.Preventive => "Mantenimiento Preventivo",
        ServiceType.Corrective => "Mantenimiento Correctivo / Reparación",
        ServiceType.Emergency => "Atención de Emergencia",
        ServiceType.Warranty => "Garantía de Servicio",
        ServiceType.Installation => "Instalación y Montaje",
        _ => type.ToString()
    };
}