using WorkshopAdmin.Shared.Enums;

namespace WorkshopAdmin.Shared.Dtos.ServiceOrders;

public class ServiceOrderDto
{
    public Guid Id { get; set; }

    // Datos del Cliente (Aplanados)
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;

    // Datos del Equipo (Aplanados)
    public Guid EquipmentId { get; set; }
    public string EquipmentType { get; set; } = string.Empty;
    public string EquipmentBrand { get; set; } = string.Empty;
    public string EquipmentModel { get; set; } = string.Empty;

    // Información de la Orden
    public string FailureDescription { get; set; } = string.Empty;
    public ServiceOrderStatus Status { get; set; } // Enum (0-4)
    public decimal LaborCost { get; set; }
    public decimal TotalCost { get; set; } // Calculado: Σ refacciones + mano de obra

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public IEnumerable<OrderPartDto?> OrderPart { get; set; } = Enumerable.Empty<OrderPartDto?>();
}