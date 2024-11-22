using Microsoft.AspNetCore.Mvc;
using StocksApp_Whole.ServiceContracts;
using StocksApp_Whole.Services;
using System.Globalization;
using StocksApp_Whole.Models;
using Microsoft.Extensions.Options;
using StocksApp_Whole.Options;
using StocksApp_Whole.ViewModels;
using StocksApp_Whole.DTO;
using System.Diagnostics;

namespace StocksApp_Whole.Controllers
{
    [Route("[controller]")]
    public class TradeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly FinnhubService _finnhubService;
        private readonly IOptions<QuoteOptions> _options;
        private readonly IStocksService _stocksService;

        public TradeController(IConfiguration config, IStocksService stocksService, FinnhubService finnhub, IOptions<QuoteOptions> options)
        {
            _configuration = config;
            _finnhubService = finnhub;
            _options = options;
            _stocksService = stocksService;
        }

        [Route("/")]
        public async Task<IActionResult> Index(string? stockSymbol)
        {
            // Set the default stockSymbol to "MSFT" if none is provided
            stockSymbol ??= "MSFT";

            // Populate fullStock with stock info
            StockViewModel fullStock = await _stocksService.GetStockInfo(stockSymbol);

            // Register current URL and Finnhub token in ViewBag
            ViewBag.CurrentUrl = "~/Trade/Index";
            ViewBag.FinnhubToken = _configuration["FinnhubToken"];

            // Return view with model data
            return View(fullStock);
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> BuyOrder(BuyOrderRequest buyOrderRequest)
        {
            //update date of order
            buyOrderRequest.DateAndTimeOfOrder = DateTime.Now;
            StockViewModel stockTradeSuccess = await _stocksService.GetStockInfo(buyOrderRequest.StockSymbol);
            buyOrderRequest.Price = stockTradeSuccess.Price;
            buyOrderRequest.StockName = stockTradeSuccess.StockName;

            //re-validate the model object after updating the date
            ModelState.Clear();
            TryValidateModel(buyOrderRequest);

            // Edge cases
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                StockViewModel stockTrade = await _stocksService.GetStockInfo(buyOrderRequest.StockSymbol);
                return View("Index", stockTrade);
            }

            //invoke service method
            BuyOrderResponse buyOrderResponse = await _stocksService.CreateBuyOrder(buyOrderRequest);

            return RedirectToAction(nameof(Orders));
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> SellOrder(SellOrderRequest sellOrderRequest)
        {
            //update date of order
            sellOrderRequest.DateAndTimeOfOrder = DateTime.Now;
            StockViewModel stockTradeSuccess = await _stocksService.GetStockInfo(sellOrderRequest.StockSymbol);
            sellOrderRequest.Price = stockTradeSuccess.Price;
            sellOrderRequest.StockName = stockTradeSuccess.StockName;

            //re-validate the model object after updating the date
            ModelState.Clear();
            TryValidateModel(sellOrderRequest);

            // Edge cases
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                StockViewModel stockTrade = await _stocksService.GetStockInfo(sellOrderRequest.StockSymbol);
                return View("Index", stockTrade);
            }

            //invoke service method
            SellOrderResponse sellOrderResponse = await _stocksService.CreateSellOrder(sellOrderRequest);

            return RedirectToAction(nameof(Orders));
        }

        [Route("[action]")]
        public async Task<IActionResult> Orders()
        {
            //invoke service methods
            List<BuyOrderResponse> buyOrderResponses = await _stocksService.GetBuyOrders();
            List<SellOrderResponse> sellOrderResponses = await _stocksService.GetSellOrders();

            //create model object
            OrdersModel orders = new OrdersModel() { BuyOrders = buyOrderResponses, SellOrders = sellOrderResponses };

            ViewBag.TradingOptions = _options;

            return View(orders);
        }
    }
}
