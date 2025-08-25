using Microsoft.EntityFrameworkCore;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);

// Load environment variables (the platform sets .env into environment)
// Expected variables (optional):
// - DB_PROVIDER: "sqlite" (default) or "inmemory"
// - DB_CONNECTION_STRING: for sqlite (e.g. Data Source=./App_Data/products.db)
// - ASPNETCORE_URLS: for port binding
// - ALLOWED_ORIGINS: comma-separated list, optional

// Add services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();

// NSwag + Swagger configuration for OpenAPI
builder.Services.AddOpenApiDocument(settings =>
{
    settings.Title = "Product Management API";
    settings.Version = "v1";
    settings.Description = "A simple CRUD API for managing products with name and price.";
    settings.DocumentName = "v1";
    settings.PostProcess = document =>
    {
        document.Info.Contact = new NSwag.OpenApiContact
        {
            Name = "API Support",
            Email = "support@example.com"
        };
        document.Tags = new[]
        {
            new NSwag.OpenApiTag { Name = "Products", Description = "CRUD operations for products" }
        }.ToList();
    };
});

// Also enable Swashbuckle (optional, for compatibility with some tools)
builder.Services.AddSwaggerGen();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.SetIsOriginAllowed(_ => true)
              .AllowCredentials()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Configure EF Core database
var dbProvider = Environment.GetEnvironmentVariable("DB_PROVIDER")?.ToLowerInvariant() ?? "sqlite";
var dbConnectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

// Default to local sqlite file if not provided
if (string.IsNullOrWhiteSpace(dbConnectionString) && dbProvider == "sqlite")
{
    var dataDir = Path.Combine(AppContext.BaseDirectory, "App_Data");
    Directory.CreateDirectory(dataDir);
    var dbPath = Path.Combine(dataDir, "products.db");
    dbConnectionString = $"Data Source={dbPath}";
}

builder.Services.AddDbContext<AppDbContext>(options =>
{
    if (dbProvider == "inmemory")
    {
        options.UseInMemoryDatabase("ProductsDb");
    }
    else // sqlite default
    {
        options.UseSqlite(dbConnectionString);
    }
});

// Register repository
builder.Services.AddScoped<IProductRepository, ProductRepository>();

var app = builder.Build();

// Ensure database created/migrated for sqlite / seed for in-memory
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (db.Database.IsSqlite())
    {
        db.Database.EnsureCreated();
    }
    if (!db.Products.Any())
    {
        db.Products.Add(new Product { Name = "Sample Product", Price = 9.99m });
        db.SaveChanges();
    }
}

// Middlewares
app.UseCors("AllowAll");

// OpenAPI (NSwag)
app.UseOpenApi();
app.UseSwaggerUi(config =>
{
    config.Path = "/docs";
});

// Swashbuckle UI (optional, available at /swagger)
app.UseSwagger();
app.UseSwaggerUI();

// Health endpoint
// PUBLIC_INTERFACE
app.MapGet("/", () => new { message = "Healthy" })
   .WithName("HealthCheck")
   .WithSummary("Health check")
   .WithDescription("Returns a basic health message to indicate the service is running.")
   .Produces(200);

// Map controllers
app.MapControllers();

app.Run();

// Domain model, DbContext, repository are in separate files under the project.
// Placed minimal definitions here to ensure compile if files are moved; actual implementations are in dedicated files.
