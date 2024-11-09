using ServiceContracts;
using Services;
using Microsoft.EntityFrameworkCore;
using Entities;

// Create builder 
var builder = WebApplication.CreateBuilder(args);

// Enable SERVICE that allows using controllers with views
builder.Services.AddControllersWithViews();

// Add services into the IoC container to use them
builder.Services.AddScoped<ICountriesService, CountriesService>();
builder.Services.AddScoped<IPersonService, PersonService>();

// Initialize DbContext use
builder.Services.AddDbContext<PersonsDbContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection"));
});

// Build app
var app = builder.Build();

// Enable developer-specific exception page
if(builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Enable features such as static file use, routing and controller mapping
app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

// LET'S GOOOO
app.Run();
