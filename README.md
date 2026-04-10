# 🛠️ WorkshopAdmin
WorkshopAdmin es una plataforma integral diseñada para gestionar el ciclo de vida completo de las órdenes de servicio en talleres técnicos. El sistema centraliza la recepción de equipos, el diagnóstico, la asignación de refacciones y el control de costos para mejorar la eficiencia operativa y la transparencia con el cliente.

---

# 🎯 Objetivo del Proyecto
Optimizar la gestión de servicios técnicos mediante un sistema escalable que garantice la trazabilidad de cada reparación, desde el ingreso del equipo hasta su entrega final.

---


# 🚀 Funcionalidades Principales (MVP)
- Gestión de Clientes y Equipos: Registro detallado de clientes y equipos (laptops, TVs, etc.) asociados para mantener un historial histórico.

- Órdenes de Servicio: Creación y seguimiento de órdenes con descripciones de falla y estados en tiempo real.

- Control de Inventario: Gestión de refacciones con descuento automático de stock al ser asignadas a una reparación.

- Cálculo de Costos Automatizado: Suma dinámica de mano de obra y costos históricos de refacciones.

- Flujo de Trabajo Estructurado: Sistema de estados obligatorio: Received → Diagnosing → Repairing → Completed → Delivered.

---


# 🏗️ Arquitectura Técnica
El proyecto implementa una Clean Architecture (Light) para asegurar la separación de responsabilidades y facilitar el mantenimiento a largo plazo:

- Domain: Contiene las entidades principales (Customer, Equipment, ServiceOrder, Part) y las reglas de negocio, sin dependencias externas.

- Application: Orquestación de casos de uso y lógica de aplicación mediante servicios y DTOs.

- Infrastructure: Implementación del acceso a datos mediante Entity Framework Core y persistencia en base de datos.

- WebAPI: Exposición de servicios mediante una API RESTful.

- UI (Blazor): Interfaz de usuario moderna desarrollada en Blazor (C#) para una experiencia web fluida.

---


# 💻 Stack Tecnológico
- Backend: .NET 10

- Frontend: Blazor

- Base de Datos: PostgreSQL (o SQL Server)

- ORM: Entity Framework Core

---


# 📊 Modelo de Datos
El diseño de la base de datos está normalizado y preparado para la integridad de la información:

- Uso de UUID para claves primarias.

- Almacenamiento de precios históricos en las refacciones utilizadas (unit_price) para auditoría.

- Relaciones robustas entre Clientes, Equipos, Órdenes y Partes.

---


# 🛠️ Reglas de Negocio Clave
- Integridad de Flujo: No es posible completar una orden sin un diagnóstico registrado ni entregarla si no está marcada como completada.

- Inmutabilidad: Las órdenes en estado Delivered no permiten modificaciones para proteger la integridad histórica de los datos.

- Validación de Stock: El sistema impide la asignación de refacciones si el inventario es insuficiente.

---

# 📈 Roadmap
- [ ] Implementación de autenticación y roles (Admin/Técnico).

- [ ] Integración de IA para diagnósticos automáticos sugeridos.

- [ ] Sistema de notificaciones automáticas para clientes.

- [ ] Generación de reportes avanzados y exportación a PDF.
