# TechSolutions – Plataforma Web de Gestión (Pagos, Órdenes, Inventario, Precios y Reportes)

Proyecto desarrollado como evaluación final del curso **Patrones de Diseño de Software**, basado en el caso **TechSolutions**.

La solución implementa una **API REST en .NET** y un **frontend en Angular** que resuelven los requerimientos del caso aplicando distintos patrones de diseño: Adapter, Strategy, Observer, Command, Memento, Proxy e Iterator.

---

## 1. Arquitectura general

La solución está dividida en tres proyectos principales:

- **TechSolutions.Core**  
  Biblioteca de clases con la **lógica de negocio** y los **patrones de diseño**:
  - Pagos (`Payments`)
  - Órdenes (`Orders`)
  - Inventario (`Inventory`)
  - Precios (`Pricing`)
  - Catálogo (`Catalog`)
  - Seguridad y usuarios (`Security`)
  - Reportes (`Reports`)

- **TechSolutions.API**  
  Proyecto **ASP.NET Core Web API** que expone los casos de uso del negocio:
  - Controladores REST (`Controllers`)
  - Configuración de inyección de dependencias (`Program.cs`)
  - Servicios específicos de infraestructura (adapters concretos, contexto de usuario HTTP, etc.)
  - Documentación con **Swagger**.

- **techsolutions-web**  
  Frontend **Angular** que consume la API:
  - Módulos: Dashboard, Pagos, Órdenes, Inventario, Precios, Catálogo, Reportes.
  - Navegación SPA y consumo de endpoints vía `HttpClient`.
  - Estilo similar a panel administrativo.

Comunicación:

```text
Angular (techsolutions-web)  -->  TechSolutions.API  -->  TechSolutions.Core
2. Requisitos previos
.NET SDK 8.x o superior

Node.js 18+

Angular CLI (global):

bash
Copiar código
npm install -g @angular/cli
Navegador web moderno (Chrome, Edge, etc.)

3. Cómo ejecutar el backend (TechSolutions.API)
Abrir una terminal (PowerShell).

Ir a la carpeta del proyecto:

bash
Copiar código
cd "C:\Patrones de Diseño de Software\TechSolutions.API"
Ejecutar la API:

bash
Copiar código
dotnet run --project TechSolutions.API/TechSolutions.API.csproj
Cuando compile, verás un mensaje similar a:

Now listening on: http://localhost:5121

Application started. Press Ctrl+C to shut down.

Abrir la documentación Swagger en el navegador:

text
Copiar código
http://localhost:5121/swagger/index.html
Desde Swagger se pueden probar todos los endpoints de la API.

4. Cómo ejecutar el frontend (techsolutions-web)
Abrir otra terminal (sin cerrar la del backend).

Ir a la carpeta del frontend:

bash
Copiar código
cd "C:\Patrones de Diseño de Software\TechSolutions.API\techsolutions-web"
Instalar dependencias (solo la primera vez):

bash
Copiar código
npm install
Levantar la aplicación Angular:

bash
Copiar código
ng serve -o
Esto abrirá automáticamente:

text
Copiar código
http://localhost:4200
La SPA mostrará el Dashboard con acceso a los módulos: Pagos, Órdenes, Inventario, Precios, Catálogo y Reportes.

5. Módulos funcionales y patrones de diseño
5.1. Pagos – Patrón Adapter
Objetivo del caso:
Integrar distintos proveedores de pago (PayPal, Yape, Plin) detrás de una interfaz común para el sistema.

Clases principales (TechSolutions.Core.Payments):

PaymentRequest, PaymentResult, PaymentMethod

IPaymentProcessor (interfaz común)

Adapters concretos:

PayPalAdapter

YapeAdapter

PlinAdapter

Cada adapter adapta la firma del proveedor real a la interfaz IPaymentProcessor.

Infraestructura (TechSolutions.API.Services):

PayPalService

YapeService

PlinService

Controller:

PaymentsController (/api/Payments)

Funcionalidad:

Registrar un pago con cualquier método.

Habilitar / deshabilitar métodos desde PaymentConfiguration.

Exponer configuración y operaciones vía API.

5.2. Inventario – Patrón Observer
Objetivo del caso:
Detectar productos con stock crítico y notificar a distintos actores (jefe, compras, etc.).

Clases principales (TechSolutions.Core.Inventory):

InventoryItem

InventoryService (sujeto/subject del patrón)

Observers:

IStockObserver (interfaz)

ManagerStockObserver

PurchasingStockObserver

Cuando el stock de un producto baja del mínimo, InventoryService notifica a todos los observadores registrados.

Controller:

InventoryController (/api/Inventory)

Endpoints:

GET /api/Inventory – consulta del stock actual.

POST /api/Inventory/adjust – ajuste de stock.

PUT /api/Inventory/minimumStock – actualización de stock mínimo.

5.3. Precios – Patrón Strategy
Objetivo del caso:
Aplicar distintas estrategias de precio según configuración (estándar, con descuento, dinámica).

Clases principales (TechSolutions.Core.Pricing):

PricingConfiguration

IPriceStrategy (interfaz)

Implementaciones:

StandardPriceStrategy

DiscountPriceStrategy

DynamicPriceStrategy

PricingService – selecciona la estrategia apropiada usando PricingConfiguration.

Controller:

PricingController (/api/Pricing)

Endpoints:

GET /api/Pricing/products – lista de productos con precio base.

GET /api/Pricing/config – configuración actual.

PUT /api/Pricing/config – actualizar la estrategia y parámetros.

POST /api/Pricing/apply – calcular el precio final de un producto.

5.4. Órdenes – Patrones Command + Memento
Objetivo del caso:
Gestionar el ciclo de vida de las órdenes y permitir deshacer la última operación.

Clases principales (TechSolutions.Core.Orders):

Entidad Order y OrderStatus.

IOrderRepository / InMemoryOrderRepository.

IOrderCommand (interfaz base de comandos).

Comandos concretos:

CreateOrderCommand

ProcessOrderCommand

CancelOrderCommand

ApplyDiscountOrderCommand

Memento / Historial:

OrderMemento – guarda el estado de la orden.

OrderCommandHistory – almacena el historial de comandos y snapshots.

Servicio:

OrderService – orquesta comandos, mementos y repositorio.

Controller:

OrdersController (/api/Orders)

Endpoints típicos:

GET /api/Orders – listar órdenes.

POST /api/Orders – crear orden.

GET /api/Orders/{id} – obtener detalle.

POST /api/Orders/{id}/process – procesar.

POST /api/Orders/{id}/cancel – cancelar.

POST /api/Orders/{id}/discount – aplicar descuento.

POST /api/Orders/undo – deshacer último comando.

De esta forma se cumple el requerimiento de apostar por un diseño extensible, donde cada nueva operación sobre órdenes puede modelarse como un nuevo comando.

5.5. Catálogo – Patrón Iterator
Objetivo del caso:
Recorrer el catálogo de productos y devolver páginas de resultados al frontend.

Clases (TechSolutions.Core.Catalog):

Product

ProductCatalog – contiene la colección interna y ofrece un iterador.

CatalogService – expone iteraciones y paginación.

Controller:

CatalogController (/api/Catalog)

Permite listar el catálogo de forma paginada para el módulo de Catálogo en Angular.

5.6. Reportes – Patrón Proxy
Objetivo del caso:
Proteger la generación de reportes financieros, permitiendo solo a usuarios con rol autorizado.

Clases (TechSolutions.Core.Reports y Security):

IReportService

RealReportService – acceso real a la generación de reportes.

ReportServiceProxy – controla acceso según el usuario.

ICurrentUserContext / UserContext / UserRole.

Infraestructura (TechSolutions.API.Services):

HttpCurrentUserContext – obtiene el usuario a partir de encabezados HTTP (X-User-Name, X-User-Role).

Controller:

ReportsController (/api/Reports/monthly)

Ejemplo de uso:

Enviar encabezado X-User-Role: Administrator → acceso permitido.

Enviar otro rol no autorizado → el proxy genera respuesta de acceso denegado.

6. Seguridad y simulación de usuarios
Para no implementar autenticación completa, el contexto de usuario se simula con headers HTTP:

X-User-Name

X-User-Role (Administrator, Manager, Viewer, etc.)

HttpCurrentUserContext lee estos valores y construye un UserContext, que luego utiliza ReportServiceProxy para verificar si el usuario tiene permisos para ver reportes.

7. CORS y consumo desde Angular
En Program.cs se habilita CORS para permitir que el frontend Angular se comunique con la API:

csharp
Copiar código
policy.WithOrigins("http://localhost:4200")
      .AllowAnyHeader()
      .AllowAnyMethod();
Esto permite que las llamadas AJAX desde techsolutions-web funcionen sin errores de CORS durante el desarrollo local.

8. Flujo de uso recomendado (demo)
Catálogo

Consultar el listado de productos en /api/Catalog o desde el módulo “Catálogo” en Angular.

Precios

Revisar la configuración actual en /api/Pricing/config.

Cambiar la estrategia a “Descuento” y definir un porcentaje.

Aplicar la estrategia a un producto y ver el nuevo precio.

Inventario

Consultar inventario actual.

Ajustar stock de un producto por debajo del mínimo y observar las notificaciones generadas por los observers.

Órdenes

Crear una orden.

Procesarla.

Aplicar un descuento.

Cancelarla.

Probar POST /api/Orders/undo para deshacer la última operación mediante Memento.

Pagos

Configurar métodos de pago habilitados/deshabilitados.

Registrar un pago de prueba utilizando PayPal, Yape o Plin.

Reportes

Llamar a /api/Reports/monthly:

Con X-User-Role: Viewer → acceso denegado (Proxy bloquea).

Con X-User-Role: Administrator → acceso permitido y reporte generado.

9. Buenas prácticas aplicadas
Separación clara entre dominio (Core) y capa de presentación (API/Angular).

Uso intensivo de Inversión de Dependencias e Inyección de Dependencias en Program.cs.

Aplicación de varios patrones de diseño de software alineados con el caso TechSolutions.

API documentada con Swagger para facilitar pruebas y mantenimiento.

Frontend SPA que consume todos los endpoints definidos y refleja los flujos de negocio del caso.

makefile
Copiar código
::contentReference[oaicite:0]{index=0}




