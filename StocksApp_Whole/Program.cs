using StocksApp_Whole.ServiceContracts;
using StocksApp_Whole.Services;
using StocksApp_Whole.Entities;
using Microsoft.EntityFrameworkCore;
using Services;
using RepositoryContracts;
using Repositories;
using ServiceContracts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddTransient<IStocksService, StocksService>();
builder.Services.AddTransient<IFinnhubService, FinnhubService>();
builder.Services.AddTransient<IStocksRepository, StocksRepository>();
builder.Services.AddTransient<IFinnhubRepository, FinnhubRepository>();


builder.Services.AddHttpClient();

builder.Services.AddDbContext<ApplicationDbContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection"));
});

builder.Services.AddSingleton<FinnhubService>();

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();

app.MapControllers();

app.Run();
