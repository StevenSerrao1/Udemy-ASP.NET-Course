using StocksApp_Whole.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient();

builder.Services.AddScoped<FinnhubService>();

var app = builder.Build();

app.UseStaticFiles();

app.Map("/", () => "Hello, world!");

app.UseRouting();

app.MapControllers();

app.Run();
