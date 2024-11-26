using ServiceContracts;
using Services;
using Microsoft.EntityFrameworkCore;
using Entities;
using RepositoryContracts;
using Repositories;
using Microsoft.AspNetCore.HttpLogging;
using Serilog;

// Create builder 
var builder = WebApplication.CreateBuilder(args);

// SERILOG // HOW TO ENABLE AND IMPLEMENT (see 'appsettings.json' and 'appsettings.development.json')
builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider services, LoggerConfiguration loggerconf) =>
{
    loggerconf.ReadFrom.Configuration(context.Configuration);
    loggerconf.ReadFrom.Services(services);
});

Console.WriteLine($"Current Environment: {builder.Environment.EnvironmentName}");

// Enable SERVICE that allows using controllers with views
builder.Services.AddControllersWithViews();
builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = HttpLoggingFields.RequestPropertiesAndHeaders | HttpLoggingFields.ResponsePropertiesAndHeaders;
});


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


// Enable features such as HTTPLOGGING, static file use, routing and controller mapping
app.UseStaticFiles();
app.UseHttpLogging();
app.UseRouting();
app.MapControllers();

// LET'S GOOOO
app.Run();

// ** CREATE PARTIAL CLASS CALLED PROGRAM IN ORDER TO ACCESS AUTO-GENERATED PROGRAM CLASS PROGRAMATICALLY
public partial class Program { }
