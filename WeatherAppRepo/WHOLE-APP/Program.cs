using Interfaces;
using Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvc();

builder.Services.AddTransient<IWeatherService, CityWeatherService>();

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();

app.MapControllers();

app.Run();
