namespace WorkshopAdmin.Shared.Enums;

public enum PartCategory : short
{
    General = 0,      // Insumos misceláneos y consumibles básicos
    Engine = 1,       // Componentes del motor y tren motriz
    Suspension = 2,   // Amortiguadores, dirección y ejes
    Electrical = 3,   // Sensores, baterías y cableado
    Fluids = 4,       // Aceites, refrigerantes y lubricantes
    Brakes = 5,       // Discos, balatas y sistemas de frenado
    Transmission = 6  // Cajas de cambio, embragues y diferenciales
}

public static class PartCategoryExtensions
{
    public static string ToFriendlyName(this PartCategory category) => category switch
    {
        PartCategory.General => "General / Consumibles",
        PartCategory.Engine => "Motor y Tren Motriz",
        PartCategory.Suspension => "Suspensión y Dirección",
        PartCategory.Electrical => "Sistema Eléctrico / Sensores",
        PartCategory.Fluids => "Líquidos y Lubricantes",
        PartCategory.Brakes => "Sistema de Frenado",
        PartCategory.Transmission => "Transmisión y Embrague",
        _ => category.ToString()
    };
}