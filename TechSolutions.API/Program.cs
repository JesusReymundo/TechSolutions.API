using TechSolutions.Core.Payments;
using TechSolutions.Core.Reports;
using TechSolutions.Core.Security;
using TechSolutions.API.Services;
using TechSolutions.Core.Inventory;
using TechSolutions.Core.Pricing;
using TechSolutions.Core.Orders;
using TechSolutions.Core.Catalog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// ---------- PAGOS (Adapter) ----------
builder.Services.AddScoped<PayPalService>();
builder.Services.AddScoped<YapeService>();
builder.Services.AddScoped<PlinService>();

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
builder.Services.AddSingleton<OrderService>();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
