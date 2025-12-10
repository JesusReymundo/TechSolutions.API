using TechSolutions.Core.Payments;
using TechSolutions.Core.Reports;
using TechSolutions.Core.Security;
using TechSolutions.Core.Inventory;
using TechSolutions.Core.Pricing;
using TechSolutions.Core.Orders;
using TechSolutions.Core.Catalog;
using TechSolutions.API.Services; // <- aquí vive HttpCurrentUserContext

var builder = WebApplication.CreateBuilder(args);

// ---------- CORS: permitir frontend Angular ----------
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins, policy =>
    {
        policy.WithOrigins("http://localhost:4200") // frontend
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// ---------- Controllers ----------
builder.Services.AddControllers();

// ---------- PAGOS (Adapter) ----------
builder.Services.AddScoped<PayPalService>();
builder.Services.AddScoped<YapeService>();
builder.Services.AddScoped<PlinService>();

// Usamos los adapters del Core
builder.Services.AddScoped<IPaymentProcessor, PayPalAdapter>();
builder.Services.AddScoped<IPaymentProcessor, YapeAdapter>();
builder.Services.AddScoped<IPaymentProcessor, PlinAdapter>();

builder.Services.AddSingleton<PaymentConfiguration>();

// ---------- REPORTES (Proxy) ----------
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserContext, HttpCurrentUserContext>();
builder.Services.AddScoped<RealReportService>();
builder.Services.AddScoped<IReportService, ReportServiceProxy>();

// ---------- INVENTARIO (Observer) ----------
builder.Services.AddScoped<InventoryService>();
builder.Services.AddScoped<IStockObserver, ManagerStockObserver>();
builder.Services.AddScoped<IStockObserver, PurchasingStockObserver>();

// ---------- PRECIOS (Strategy) ----------
builder.Services.AddSingleton<PricingConfiguration>();
builder.Services.AddScoped<PricingService>();
builder.Services.AddScoped<IPriceStrategy, StandardPriceStrategy>();
builder.Services.AddScoped<IPriceStrategy, DiscountPriceStrategy>();
builder.Services.AddScoped<IPriceStrategy, DynamicPriceStrategy>();

// ---------- PEDIDOS (Command + Memento) ----------
builder.Services.AddSingleton<IOrderRepository, InMemoryOrderRepository>();
builder.Services.AddSingleton<OrderCommandHistory>();
builder.Services.AddScoped<OrderService>();

// ---------- CATÁLOGO (Iterator) ----------
builder.Services.AddSingleton<ProductCatalog>();
builder.Services.AddScoped<CatalogService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// aplicar CORS antes de Authorization
app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
