using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StocksApp_Whole.ServiceContracts;
using StocksApp_Whole.DTO;
using StocksApp_Whole.Models;
using System.Diagnostics;
using System.Text.Json;
using StocksApp_Whole.Options;
using StockMarketSolution.Models;
using ServiceContracts;

namespace StockMarketSolution.Controllers
{
    [Route("[controller]")]
    public class StocksController : Controller
    {
        private readonly QuoteOptions _tradingOptions;
        private readonly IFinnhubService _finnhubService;


        /// <summary>
        /// Constructor for TradeController that executes when a new object is created for the class
        /// </summary>
        /// <param name="tradingOptions">Injecting TradeOptions config through Options pattern</param>
        /// <param name="finnhubService">Injecting FinnhubService</param>
        public StocksController(IOptions<QuoteOptions> tradingOptions, IFinnhubService finnhubService)
        {
            _tradingOptions = tradingOptions.Value;
            _finnhubService = finnhubService;
        }


        [Route("[action]/{stock?}")]
        [Route("~/[action]/{stock?}")]
        public async Task<IActionResult> Explore(string? stock, bool showAll = false)
        {
            //get company profile from API server
            List<Dictionary<string, string>>? stocksDictionary = await _finnhubService.GetStocks();

            List<Stocks> stocks = new List<Stocks>();

            if (stocksDictionary is not null)
            {
                //filter stocks
                if (!showAll && _tradingOptions.Top25PopularStocks != null)
                {
                    string[]? Top25PopularStocksList = _tradingOptions.Top25PopularStocks.Split(",");
                    if (Top25PopularStocksList is not null)
                    {
                        stocksDictionary = stocksDictionary
                         .Where(temp => Top25PopularStocksList.Contains(Convert.ToString(temp["symbol"])))
                         .ToList();
                    }
                }

                //convert dictionary objects into Stock objects
                stocks = stocksDictionary
                 .Select(temp => new Stocks() { StockName = Convert.ToString(temp["description"]), StockSymbol = Convert.ToString(temp["symbol"]) })
                .ToList();
            }

            ViewBag.stock = stock;
            return View("Stocks.cshtml", stocks);
        }
    }
}

