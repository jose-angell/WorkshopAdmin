namespace WorkshopAdmin.Shared.Enums;

/// <summary>
/// Define los estados permitidos para una Orden de Servicio.
/// Estos valores están alineados con el tipo smallint en la base de datos PostgreSQL.
/// </summary>
public enum ServiceOrderStatus : short
{
    /// <summary>
    /// Estado inicial: El equipo ha sido recibido en el taller.
    /// </summary>
    Received = 0,

    /// <summary>
    /// El técnico está realizando la revisión inicial del equipo.
    /// </summary>
    Diagnosing = 1,

    /// <summary>
    /// El equipo se encuentra en proceso de reparación.
    /// </summary>
    Repairing = 2,

    /// <summary>
    /// La reparación ha finalizado y el diagnóstico ha sido registrado.
    /// </summary>
    Completed = 3,

    /// <summary>
    /// El equipo ha sido entregado al cliente (estado final no editable).
    /// </summary>
    Delivered = 4
}