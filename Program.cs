using InventorySystem.Components;
using InventorySystem.Data;
using InventorySystem.Services;
using Microsoft.EntityFrameworkCore;
using Radzen;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Radzen services
builder.Services.AddRadzenComponents();

// Database
builder.Services.AddDbContext<InventoryDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
    
// Repositories
builder.Services.AddScoped(typeof(InventorySystem.Repositories.IRepository<>), typeof(InventorySystem.Repositories.Repository<>));
builder.Services.AddScoped<InventorySystem.Repositories.ICategoryRepository, InventorySystem.Repositories.CategoryRepository>();
builder.Services.AddScoped<InventorySystem.Repositories.IProductRepository, InventorySystem.Repositories.ProductRepository>();
builder.Services.AddScoped<InventorySystem.Repositories.IStockTransactionRepository, InventorySystem.Repositories.StockTransactionRepository>();

// Application services
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IStockTransactionService, StockTransactionService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IAiAnalysisService, AiAnalysisService>();

var app = builder.Build();

// Auto-migrate database
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<InventoryDbContext>();
    db.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
