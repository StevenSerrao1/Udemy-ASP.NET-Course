using Microsoft.AspNetCore.Mvc;
using StocksApp_Whole.ServiceContracts;
using StocksApp_Whole.Services;
using System.Globalization;
using StocksApp_Whole.Models;
using Microsoft.Extensions.Options;
using StocksApp_Whole.Options;

namespace StocksApp_Whole.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly FinnhubService _finnhubService;
        private readonly IOptions<QuoteOptions> _options;

        public HomeController(IConfiguration config, FinnhubService finnhub, IOptions<QuoteOptions> options)
        {
            _configuration = config;
            _finnhubService = finnhub;
            _options = options;
            
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
                CurrentPrice = Convert.ToDouble(stockPrice["c"].ToString(), CultureInfo.InvariantCulture),
                HighestPrice = Convert.ToDouble(stockPrice["h"].ToString(), CultureInfo.InvariantCulture),
                LowestPrice = Convert.ToDouble(stockPrice["l"].ToString(), CultureInfo.InvariantCulture),
                OpenPrice = Convert.ToDouble(stockPrice["o"].ToString(), CultureInfo.InvariantCulture),
            };

            stockInfo = await _finnhubService.GetCompanyProfile(stockSymbol);

            StockModel fullStock = new StockModel()
            {
                StockSymbol = stockSymbol,
                Price = Convert.ToDouble(stockPrice["c"].ToString(), CultureInfo.InvariantCulture),
                StockName = stockInfo["name"].ToString()
            };


            return View(fullStock);
        }
    }
}
