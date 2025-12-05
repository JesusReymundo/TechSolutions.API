# TechSolutions.API – Caso práctico Patrones de Diseño de Software

Proyecto backend desarrollado para el caso **TechSolutions S.A.**, orientado a pymes que necesitan gestionar pagos, pedidos, inventario, catálogo de productos y reportes financieros.

Curso: **Patrones de Diseño de Software**  
Estudiante: **Reymundo Jesús Roman**  
Repositorio: https://github.com/JesusReymundo/TechSolutions.API  

---

## 🎯 Objetivo del proyecto

Aplicar **patrones de diseño (GOF y GRASP)** en una API REST construida con **.NET 8** para resolver los requerimientos RF1–RF12 del caso TechSolutions:

- Integración con varias pasarelas de pago.
- Control de acceso a reportes.
- Gestión de inventario con alertas de stock.
- Procesamiento de pedidos con historial y deshacer.
- Políticas de precios configurables.
- Catálogo de productos con recorrido eficiente.

---

## 🧱 Arquitectura general

La solución está dividida en tres proyectos:

### 1. TechSolutions.API
Capa de presentación (Web API).

- `Controllers/`
  - `PaymentsController`  
  - `ReportsController`  
  - `InventoryController`  
  - `OrdersController`  
  - `PricingController`  
  - `CatalogController`
- `Dtos/`
  - Modelos para requests/responses (por ejemplo `PaymentRequest`, `ApplyPriceRequest`, etc.).
- `Program.cs`
  - Configuración de DI (Dependency Injection), Swagger y mapeo de endpoints.

### 2. TechSolutions.Core
Capa de dominio y lógica de negocio.

- `Payments/` → Adapter + configuración de pasarelas  
- `Reports/` → Proxy de reportes  
- `Inventory/` → Observer de stock  
- `Pricing/` → Strategy de precios + configuración global  
- `Orders/` → Command + Memento para pedidos  
- `Catalog/` → Iterator de productos  
- `Security/` → Contexto de usuario y roles  
- `Entities/` → Entidades compartidas (`Product`, etc.)

### 3. TechSolutions.Tests
Proyecto de pruebas unitarias con **xUnit**:

- `PricingServiceTests` → valida lógicas de precios (Strategy).
- `OrderServiceTests` → valida historial y deshacer (Command + Memento).

Esta separación permite aplicar principios **GRASP**:  
*Controller, Alta Cohesión, Bajo Acoplamiento y Polimorfismo.*

---

## 🧩 Patrones de diseño aplicados

### Adapter – Integración de pasarelas de pago

- **Clases clave**
  - `IPaymentProcessor`
  - `PayPalAdapter`, `YapeAdapter`, `PlinAdapter`
  - `PayPalService`, `YapeService`, `PlinService`
  - `PaymentConfiguration`
- **Endpoints**
  - `POST /api/Payments` → procesa un pago con la pasarela elegida.
  - `GET /api/Payments/config` → lista pasarelas habilitadas.
  - `POST /api/Payments/config/enable` → habilita una pasarela.
  - `POST /api/Payments/config/disable` → deshabilita una pasarela.
- **Intención**  
  Unificar bajo una única interfaz el uso de diferentes pasarelas (PayPal, Yape, Plin) sin que el controlador conozca los detalles de cada SDK. La configuración permite cumplir RF2 (habilitar/deshabilitar).

---

### Proxy – Acceso a reportes financieros

- **Clases clave**
  - `IReportService`
  - `RealReportService`
  - `ReportServiceProxy`
  - `ICurrentUserContext`, `HttpCurrentUserContext`
  - `UserContext`, `UserRole`
- **Endpoint**
  - `GET /api/Reports/monthly`
- **Intención**  
  `ReportServiceProxy` actúa como sustituto de `RealReportService`, verificando el rol del usuario (Manager/Accountant) antes de permitir el acceso al reporte mensual (RF3 y RF4).

---

### Observer – Alertas de stock bajo

- **Clases clave**
  - `InventoryItem`, `InventoryService`
  - `IStockObserver`
  - `ManagerStockObserver`, `PurchasingStockObserver`
- **Endpoints**
  - `GET /api/Inventory` → lista de productos en inventario.
  - `POST /api/Inventory/adjust` → ajusta stock (incrementa/decrementa).
  - `PUT /api/Inventory/minimumStock` → configura el stock mínimo por producto.
- **Intención**  
  Cuando el stock de un producto baja por debajo del mínimo configurado, `InventoryService` notifica a los observadores (Gerencia y Compras) generando mensajes de alerta (RF5 y RF6).

---

### Strategy – Políticas de precios

- **Clases clave**
  - `Product` (en `Entities`)
  - `PricingService`
  - `IPriceStrategy`
  - `StandardPriceStrategy`, `DiscountPriceStrategy`, `DynamicPriceStrategy`
  - `PriceContext`
  - `PricingConfiguration`
- **Endpoints**
  - `GET /api/Pricing/products` → catálogo básico de productos con precio base.
  - `GET /api/Pricing/config` → estrategia de precios por defecto y parámetros.
  - `PUT /api/Pricing/config` → admin configura estrategia global (RF10).
  - `POST /api/Pricing/apply`  
    - Permite:
      - Elegir explícitamente una estrategia, o  
      - Usar la estrategia configurada globalmente (`UseConfiguredStrategy = true`).
- **Intención**  
  Cambiar la forma de calcular el precio final (estándar, descuento porcentual, precio dinámico según demanda) sin modificar el código cliente, solo agregando nuevas estrategias.

---

### Command + Memento – Procesamiento de pedidos y deshacer

- **Clases clave**
  - `Order`, `OrderStatus`
  - `IOrderCommand`
  - `ProcessOrderCommand`, `CancelOrderCommand`, `ApplyDiscountCommand`
  - `OrderHistory`, `OrderMemento`
  - `OrderService`
- **Endpoints**
  - `GET /api/Orders` → lista de pedidos.
  - `GET /api/Orders/{id}` → detalle de pedido.
  - `POST /api/Orders` → crea pedido.
  - `POST /api/Orders/{id}/process` → procesa pedido.
  - `POST /api/Orders/{id}/discount` → aplica descuento.
  - `POST /api/Orders/{id}/cancel` → cancela pedido.
  - `POST /api/Orders/{id}/undo` → deshace la última operación sobre el pedido.
- **Intención**  
  Encapsular operaciones sobre pedidos como comandos, registrar el estado previo en `OrderHistory` (Memento) y permitir **deshacer** (RF7 y RF8).

---

### Iterator – Catálogo de productos

- **Clases clave**
  - `ProductCatalog`, `CatalogService`
  - `IProductCollection`, `IProductIterator`
  - `ProductIterator`
- **Endpoint**
  - `GET /api/Catalog?pageNumber=&pageSize=&nameFilter=`
- **Intención**  
  Recorrer el catálogo con paginación y filtro de nombre sin exponer la estructura interna de la colección de productos (RF11 y RF12).

---

## 🧠 Patrones GRASP en la solución

- **Controller**  
  - Los controladores Web API (`PaymentsController`, `OrdersController`, etc.) reciben la petición, validan y delegan en servicios de dominio.
- **Low Coupling (bajo acoplamiento)**  
  - Uso extensivo de **interfaces** (`IPaymentProcessor`, `IPriceStrategy`, `IStockObserver`, `IReportService`, `IOrderCommand`) y **Dependency Injection**.
- **High Cohesion (alta cohesión)**  
  - Cada servicio tiene una responsabilidad clara:
    - `OrderService` se ocupa solo de pedidos.  
    - `PricingService` solo de cálculo de precios.  
    - `InventoryService` de inventario, etc.
- **Polymorphism (polimorfismo)**  
  - Variaciones de comportamiento se resuelven con implementaciones concretas de interfaces:
    - Estrategias de precio, comandos de pedido, observers de stock, adapters de pago, etc.

---

## ▶️ Cómo ejecutar el proyecto

### Requisitos

- **.NET SDK 8.0** o superior instalado.

### Ejecutar la API

```bash
# Clonar el repositorio
git clone https://github.com/JesusReymundo/TechSolutions.API.git
cd TechSolutions.API

# Ejecutar la Web API
dotnet run --project TechSolutions.API/TechSolutions.API.csproj
La API quedará expuesta en:

text
Copiar código
http://localhost:5121/swagger
✅ Pruebas unitarias
Las pruebas están en el proyecto TechSolutions.Tests.

Para ejecutarlas:

bash
Copiar código
cd TechSolutions.API
dotnet test
Resultados esperados: todas las pruebas Correctas (verde).

🔗 Enlaces adicionales (para el informe)
Completar cuando estén listos.

Prototipo Figma (UI): [enlace pendiente]

Documento técnico (PDF): [enlace pendiente]

📌 Trabajo futuro / mejoras
Implementar un frontend (Angular/React) que consuma esta API.

Persistir información en una base de datos (actualmente los datos son en memoria).

Agregar más pruebas unitarias y de integración.

Extender el catálogo y las estrategias de pricing con reglas más avanzadas.

r
Copiar código

Cuando lo pegues:

1. Guarda el archivo (`Ctrl+S`).
2. En PowerShell:

```powershell
cd "C:\Patrones de Diseño de Software\TechSolutions.API"
git add README.md
git commit -m "Actualizar README con descripción de patrones y endpoints"
git push






