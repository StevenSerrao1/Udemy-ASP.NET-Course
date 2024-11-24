using ServiceContracts;
using Services;
using Microsoft.EntityFrameworkCore;
using Entities;
using RepositoryContracts;
using Repositories;

// Create builder 
var builder = WebApplication.CreateBuilder(args);

Console.WriteLine($"Current Environment: {builder.Environment.EnvironmentName}");

// Enable SERVICE that allows using controllers with views
builder.Services.AddControllersWithViews();

//// Configure logging provider / Old way of doing it (pre .NET 7)
//builder.Host.ConfigureLogging(logging => 
//{
//    logging.ClearProviders();
//    logging.AddConsole();
//});

// Configure logging provider
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services into the IoC container to use them
builder.Services.AddScoped<IPersonsRepository, PersonsRepository>();
builder.Services.AddScoped<ICountriesRepository, CountriesRepository>();
builder.Services.AddScoped<ICountriesService, CountriesService>();
builder.Services.AddScoped<IPersonService, PersonService>();

if (!builder.Environment.IsEnvironment("Test"))
{
    var connectionString = builder.Configuration.GetConnectionString("DbConnection");
    Console.WriteLine($"Connection String: {connectionString}");

    builder.Services.AddDbContext<ApplicationDbContext>(options =>
    {
        options.UseSqlServer(connectionString);
    });

    // Register Rotativa pdf services for development environment
    Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");
}

// Build app
var app = builder.Build();

// Enable developer-specific exception page
if(builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.Logger.LogDebug("Debug log message");
app.Logger.LogInformation("Information log message");
app.Logger.LogWarning("Warning log message");
app.Logger.LogError("Error log message");
app.Logger.LogCritical("Critical log message");

// Enable features such as static file use, routing and controller mapping
app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

// LET'S GOOOO
app.Run();

// ** CREATE PARTIAL CLASS CALLED PROGRAM IN ORDER TO ACCESS AUTO-GENERATED PROGRAM CLASS PROGRAMATICALLY
public partial class Program { }
