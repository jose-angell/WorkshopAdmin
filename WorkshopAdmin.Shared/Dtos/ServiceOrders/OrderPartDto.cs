using System;
using System.Collections.Generic;
using System.Text;

namespace WorkshopAdmin.Shared.Dtos.ServiceOrders;
public class OrderPartDto
{
    public Guid ServiceOrderId { get; set; }
    public Guid PartId { get; set; }
    public string PartName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
    public decimal Subtotal { get; set; }
    
}

