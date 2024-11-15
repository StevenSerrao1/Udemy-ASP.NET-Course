using StocksApp_Whole.ServiceContracts;
using StocksApp_Whole.Services;
using StocksApp_Whole.Entities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IStocksService, StocksService>();

builder.Services.AddHttpClient();

builder.Services.AddDbContext<StockMarketDbContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection"));
});

builder.Services.AddSingleton<FinnhubService>();

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();

app.MapControllers();

app.Run();
