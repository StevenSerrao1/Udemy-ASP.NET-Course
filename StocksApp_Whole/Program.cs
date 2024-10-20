using StocksApp_Whole.ServiceContracts;
using StocksApp_Whole.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IStocksService, StocksService>();

builder.Services.AddHttpClient();

builder.Services.AddScoped<FinnhubService>();

var app = builder.Build();

app.UseStaticFiles();

app.Map("/", () => "Hello, world!");

app.UseRouting();

app.MapControllers();

app.Run();
