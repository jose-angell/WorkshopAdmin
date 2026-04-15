using System;
using System.Collections.Generic;
using System.Text;

namespace WorkshopAdmin.Domain.Entities;

public class OrderPart
{
    public Guid ServiceOrderId { get; set; } // PK, FK 
    public Guid PartId { get; set; } // PK, FK 
    public int Quantity { get; set; } // Cantidad utilizada 
    public decimal UnitPrice { get; set; } // Precio histórico al momento de la asignación 

    // Propiedades de navegación
    public virtual ServiceOrder ServiceOrder { get; set; } = null!;
    public virtual Part Part { get; set; } = null!;
}
