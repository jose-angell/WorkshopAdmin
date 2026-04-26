namespace WorkshopAdmin.Shared.Enums;

public enum EquipmentType : short
{
    LightVehicle = 0, // Automóviles sedán, compactos y SUVs
    HeavyDuty = 1,    // Camiones, remolques y transporte de carga
    Industrial = 2,   // Maquinaria fija, prensas o motores estacionarios
    PowerTool = 3,    // Herramientas eléctricas o neumáticas industriales
    Agricultural = 4,  // Tractores y maquinaria de campo
    Unknown = -1   // Valor por defecto para casos no especificados
}

public static class EquipmentTypeExtensions
{
    public static string ToFriendlyName(this EquipmentType type) => type switch
    {
        EquipmentType.LightVehicle => "Vehículo Ligero (Auto/SUV)",
        EquipmentType.HeavyDuty => "Equipo Pesado / Carga",
        EquipmentType.Industrial => "Maquinaria Industrial / Fija",
        EquipmentType.PowerTool => "Herramienta Eléctrica / Neumática",
        EquipmentType.Agricultural => "Maquinaria Agrícola",
        EquipmentType.Unknown => "Desconocido / Sin especificar",
        _ => type.ToString()
    };
}