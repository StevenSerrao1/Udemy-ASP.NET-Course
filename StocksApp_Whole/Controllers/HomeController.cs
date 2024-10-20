using Microsoft.AspNetCore.Mvc;
using StocksApp_Whole.ServiceContracts;
using StocksApp_Whole.Services;
using System.Globalization;
using StocksApp_Whole.Models;
using Microsoft.Extensions.Options;
using StocksApp_Whole.Options;
using StocksApp_Whole.ViewModels;

namespace StocksApp_Whole.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly FinnhubService _finnhubService;
        private readonly IOptions<QuoteOptions> _options;
        private readonly IStocksService _stocksService;

        public HomeController(IConfiguration config, IStocksService stocksService, FinnhubService finnhub, IOptions<QuoteOptions> options)
        {
            _configuration = config;
            _finnhubService = finnhub;
            _options = options;
            _stocksService = stocksService;
        }

        [Route("/trade/{stockSymbol}")]
        public async Task<IActionResult> Index(string? stockSymbol)
        {

            Dictionary<string, object>? stockPrice = null;
            Dictionary<string, object>? stockInfo = null;

            if (stockSymbol == null)
            {
                stockSymbol = "MSFT";
            };

            stockPrice = await _finnhubService.GetStockPriceQuote(stockSymbol);

            QuoteModel stock = new QuoteModel()
            {
                StockSymbol = stockSymbol,
                CurrentPrice = stockPrice != null ? Convert.ToDouble(stockPrice["c"].ToString(), CultureInfo.InvariantCulture) : null,
                HighestPrice = stockPrice != null ? Convert.ToDouble(stockPrice["h"].ToString(), CultureInfo.InvariantCulture) : null,
                LowestPrice = stockPrice != null ? Convert.ToDouble(stockPrice["l"].ToString(), CultureInfo.InvariantCulture) : null,
                OpenPrice = stockPrice != null ? Convert.ToDouble(stockPrice["o"].ToString(), CultureInfo.InvariantCulture) : null
            };

            stockInfo = await _finnhubService.GetCompanyProfile(stockSymbol);

            StockViewModel fullStock = new StockViewModel()
            {
                StockSymbol = stockSymbol,
                Price = stockPrice != null ? Convert.ToDouble(stockPrice["c"].ToString(), CultureInfo.InvariantCulture) : 0,
                StockName = stockInfo != null ? stockInfo["name"].ToString() : null
            };

            ViewBag.FinnhubToken = _configuration["FinnhubToken"];

            return View(fullStock);
        }
    }
}
