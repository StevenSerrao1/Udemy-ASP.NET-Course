using ServiceContracts;
using Services;
using Microsoft.EntityFrameworkCore;
using Entities;
using RepositoryContracts;
using Repositories;

// Create builder 
var builder = WebApplication.CreateBuilder(args);

// Enable SERVICE that allows using controllers with views
builder.Services.AddControllersWithViews();

// Add services into the IoC container to use them
builder.Services.AddScoped<IPersonsRepository, PersonsRepository>();
builder.Services.AddScoped<ICountriesRepository, CountriesRepository>();
builder.Services.AddScoped<ICountriesService, CountriesService>();
builder.Services.AddScoped<IPersonService, PersonService>();

// Initialize DbContext use
builder.Services.AddDbContext<ApplicationDbContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection"));
});

// Build app
var app = builder.Build();

// Enable developer-specific exception page
if(builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");

// Enable features such as static file use, routing and controller mapping
app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

// LET'S GOOOO
app.Run();
