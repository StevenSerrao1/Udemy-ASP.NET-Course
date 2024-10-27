using StocksApp_Whole.ServiceContracts;
using StocksApp_Whole.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<IStocksService, StocksService>();

builder.Services.AddHttpClient();

builder.Services.AddSingleton<FinnhubService>();

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();

app.MapControllers();

app.Run();
