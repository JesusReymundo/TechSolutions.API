# TechSolutions.API – Caso práctico Patrones de Diseño

Proyecto backend desarrollado para el caso **TechSolutions S.A.**  
Curso: **Patrones de Diseño de Software**  
Integrante(s): _[colocar nombres]_  

El objetivo es construir una **API REST** que aplique patrones de diseño de forma explícita para resolver los requerimientos de la plataforma de gestión empresarial para pymes.

---

## 🧱 Arquitectura general

- **TechSolutions.API**  
  - Capa de presentación / endpoints REST (`Controllers`).
  - Configuración de inyección de dependencias (`Program.cs`).

- **TechSolutions.Core**  
  - Capa de dominio y servicios de negocio.
  - Módulos:
    - `Payments` (Adapter)
    - `Reports` (Proxy)
    - `Inventory` (Observer)
    - `Pricing` (Strategy)
    - `Orders` (Command + Memento)
    - `Catalog` (Iterator)
    - `Security`, `Entities`

- **TechSolutions.Tests**  
  - Pruebas unitarias con xUnit (`PricingServiceTests`, `OrderServiceTests`, …).

Esta separación permite aplicar principios **GRASP**:  
Controlador, Baja dependencia, Alta cohesión y Polimorfismo.

---

## 🧩 Patrones de diseño aplicados

### 1. Adapter – Integración de pasarelas de pago

- **Clases clave**
  - `IPaymentProcessor`
  - `PayPalAdapter`, `YapeAdapter`, `PlinAdapter`
  - `PayPalService`, `YapeService`, `PlinService`
- **Endpoint**
  - `POST /api/Payments`
- **Idea**: Unificar en una misma interfaz el uso de diferentes pasarelas (PayPal, Yape, Plin) para que el controlador no dependa de SDKs concretos.

---

### 2. Proxy – Acceso a reportes financieros

- **Clases clave**
  - `IReportService`
  - `RealReportService`
  - `ReportServiceProxy`
  - `ICurrentUserContext`, `HttpCurrentUserContext`, `UserContext`, `UserRole`
- **Endpoint**
  - `GET /api/Reports/monthly`
- **Idea**: El proxy controla que solo usuarios con rol **Manager** o **Accountant** accedan a los reportes.

---

### 3. Observer – Alertas de stock bajo

- **Clases clave**
  - `InventoryItem`, `InventoryService`
  - `IStockObserver`
  - `ManagerStockObserver`, `PurchasingStockObserver`
  - `StockNotification`
- **Endpoints**
  - `GET /api/Inventory`
  - `POST /api/Inventory/adjust`
- **Idea**: Cuando el stock cruza por debajo del mínimo, se notifica a los observadores (Gerencia y Compras).

---

### 4. Strategy – Políticas de precios

- **Clases clave**
  - `Product` (en `Entities`)
  - `PricingService`
  - `IPriceStrategy`
  - `StandardPriceStrategy`, `DiscountPriceStrategy`, `DynamicPriceStrategy`
  - `PriceContext`, `PriceStrategyType`
- **Endpoints**
  - `GET /api/Pricing/products`
  - `POST /api/Pricing/apply`
- **Idea**: Calcular el precio final de un producto según la estrategia seleccionada (estándar, descuento, precio dinámico).

---

### 5. Command + Memento – Procesamiento de pedidos y deshacer

- **Clases clave**
  - `Order`, `OrderStatus`
  - `IOrderCommand`
  - `ProcessOrderCommand`, `CancelOrderCommand`, `ApplyDiscountCommand`
  - `OrderHistory`, `OrderMemento`
  - `OrderService`
- **Endpoints**
  - `GET /api/Orders`
  - `GET /api/Orders/{id}`
  - `POST /api/Orders` (crear)
  - `POST /api/Orders/{id}/process`
  - `POST /api/Orders/{id}/discount`
  - `POST /api/Orders/{id}/cancel`
  - `POST /api/Orders/{id}/undo`
- **Idea**: Encapsular operaciones sobre pedidos como comandos, registrar estados anteriores y permitir **deshacer** la última acción mediante Memento.

---

### 6. Iterator – Catálogo de productos

- **Clases clave**
  - `ProductCatalog`, `CatalogService`
  - `IProductCollection`, `IProductIterator`
  - `ProductIterator`
- **Endpoint**
  - `GET /api/Catalog?pageNumber=&pageSize=&nameFilter=`
- **Idea**: Recorrer el catálogo de forma secuencial con paginación y filtro, sin exponer la estructura interna de la colección.

---

## 🧠 GRASP en la solución

- **Controller**:  
  - `PaymentsController`, `OrdersController`, `InventoryController`, `PricingController`, `ReportsController`, `CatalogController`.
- **Low Coupling**:  
  - Uso de inyección de dependencias y de interfaces (`IPaymentProcessor`, `IPriceStrategy`, `IStockObserver`, `IReportService`).
- **High Cohesion**:  
  - Cada servicio tiene una responsabilidad clara (`OrderService` maneja pedidos, `InventoryService` inventario, etc.).
- **Polymorphism**:  
  - Variaciones de comportamiento a través de clases concretas que implementan interfaces de estrategia, comando, observer, etc.

---

## ▶️ Cómo ejecutar el proyecto

### Requisitos

- .NET SDK 8.0 instalado

### Pasos

```bash
# Clonar el repositorio
git clone https://github.com/usuario/TechSolutions.API.git
cd TechSolutions.API

# Ejecutar la API
dotnet run --project TechSolutions.API/TechSolutions.API.csproj
La API expone Swagger en:

text
Copiar código
http://localhost:5121/swagger
✅ Pruebas unitarias
Las pruebas se encuentran en el proyecto TechSolutions.Tests.

Para ejecutarlas:

bash
Copiar código
cd TechSolutions.API
dotnet test
Pruebas incluidas:

PricingServiceTests → valida el cálculo de precios (Strategy).

OrderServiceTests → valida el uso de Memento para deshacer cambios en pedidos.

