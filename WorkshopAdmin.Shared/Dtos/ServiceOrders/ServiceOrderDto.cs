using WorkshopAdmin.Shared.Dtos.Customers;
using WorkshopAdmin.Shared.Dtos.Equipments;
using WorkshopAdmin.Shared.Enums;

namespace WorkshopAdmin.Shared.Dtos.ServiceOrders;

public class ServiceOrderDto
{
    public Guid Id { get; set; }

    public string FriendlyId { get; set; } = string.Empty; // Código legible (ej: ORD-2605-00001)

    // Datos del Cliente (Aplanados)
    public CustomerDto Customer { get; set; } = new CustomerDto();

    // Datos del Equipo (Aplanados)
    public EquipmentDto Equipment { get; set; } = new EquipmentDto();

    // Información de la Orden
    public string FailureDescription { get; set; } = string.Empty;
    public string? Diagnosis { get; set; }
    public ServiceOrderStatus Status { get; set; } // Enum (0-4)
    public decimal LaborCost { get; set; }
    public decimal TotalCost { get; set; } // Calculado: Σ refacciones + mano de obra
    public TimeSpan? EstimatedTime { get; set; }

    // Propiedad calculada útil para la UI
    public string EstimatedTimeDisplay => EstimatedTime.HasValue ? $"{(int)EstimatedTime.Value.TotalHours}h {EstimatedTime.Value.Minutes}m" : "No definido";
    public DateTimeOffset? RepairStartedAt { get; set; }
    public DateTimeOffset? RepairFinishedAt { get; set; }
    public DateTimeOffset? ExpectedFinishAt { get; set; }
    public TimeSpan? RemainingBusinessTime { get; set; }
    public string RemainingBusinessTimeDisplay => RemainingBusinessTime.HasValue ? $"{(int)RemainingBusinessTime.Value.TotalHours}h {RemainingBusinessTime.Value.Minutes}m" : "--";
    public string ExpectedFinishDisplay => ExpectedFinishAt?.ToLocalTime().ToString("dd/MM/yy hh:mm tt") ?? "--";
    public bool IsDelayed { get; set; }
    public ServiceType ServiceTypeId { get; set; }
    public Guid TechnicianId { get; set; }
    public string ServiceTypeDescription { get; set; } = string.Empty;
    public Guid CreatedByUserId { get; set; }
    public Guid? UpdatedByUserId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public IEnumerable<OrderPartDto?> OrderPart { get; set; } = Enumerable.Empty<OrderPartDto?>();
}