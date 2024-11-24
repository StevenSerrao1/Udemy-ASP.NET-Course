using StocksApp_Whole.DTO;
using StocksApp_Whole.ServiceContracts;
using StocksApp_Whole.Entities;
using StocksApp_Whole.Services.Helpers;
using StocksApp_Whole.ViewModels;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using StocksApp_Whole.Services;
using Services;

namespace StocksApp_Whole.Services
{
    public class StocksService : IStocksService
    {

        private readonly ApplicationDbContext _buyOrdersDb;
        private readonly ApplicationDbContext _sellOrdersDb;
        private readonly FinnhubService _finnhubService;

        public StocksService(ApplicationDbContext sellOrders, ApplicationDbContext buyOrders, FinnhubService finnhub)
        {
            _buyOrdersDb = buyOrders;
            _sellOrdersDb = sellOrders;
            _finnhubService = finnhub;
        }

        public BuyOrderRequest CreateMockBuyOrderRequest()
        {
            return new BuyOrderRequest()
            {
                StockSymbol = "AAPL",
                StockName = "Apple",
                DateAndTimeOfOrder = new DateTime(2005, 02, 23),
                Quantity = 20,
                Price = 100
            };
        }

        public List<BuyOrderRequest> CreateManyMockBuyOrderRequests()
        {
            return new List<BuyOrderRequest>()
            {

                new BuyOrderRequest()
                {
                    StockSymbol = "AAPL",
                    StockName = "Apple",
                    DateAndTimeOfOrder = new DateTime(2005, 02, 23),
                    Quantity = 20,
                    Price = 100
                },

                new BuyOrderRequest()
                {
                    StockSymbol = "MSFT",
                    StockName = "Microsoft",
                    DateAndTimeOfOrder = new DateTime(2002, 12, 21),
                    Quantity = 10,
                    Price = 110
                },

                new BuyOrderRequest()
                {
                    StockSymbol = "TSLA",
                    StockName = "Tesla Inc.",
                    DateAndTimeOfOrder = new DateTime(2015, 08, 05),
                    Quantity = 50,
                    Price = 100
                }

            };
        }     

        public SellOrderRequest CreateMockSellOrderRequest()
        {
            return new SellOrderRequest()
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                DateAndTimeOfOrder = new DateTime(2001, 12, 11),
                Quantity = 25,
                Price = 110
            };
        }

        public List<SellOrderRequest> CreateManyMockSellOrderRequests()
        {
            return new List<SellOrderRequest>()
            {

                new SellOrderRequest()
                {
                    StockSymbol = "AAPL",
                    StockName = "Apple",
                    DateAndTimeOfOrder = new DateTime(2005, 02, 23),
                    Quantity = 20,
                    Price = 100
                },

                new SellOrderRequest()
                {
                    StockSymbol = "MSFT",
                    StockName = "Microsoft",
                    DateAndTimeOfOrder = new DateTime(2002, 12, 21),
                    Quantity = 10,
                    Price = 110
                },

                new SellOrderRequest()
                {
                    StockSymbol = "TSLA",
                    StockName = "Tesla Inc.",
                    DateAndTimeOfOrder = new DateTime(2015, 08, 05),
                    Quantity = 50,
                    Price = 100
                },
            };
        }

        public async Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest)
        {
            // Check if buyOrderRequest is null
            if (buyOrderRequest == null) throw new ArgumentNullException(nameof(buyOrderRequest));

            // Validate
            ValidationHelper.ValidateObject(buyOrderRequest);

            // convert from BuyOrderRequest type to BuyOrder entity
            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();

            // Generate BuyOrder.BuyOrderId
            buyOrder.BuyOrderID = Guid.NewGuid();

            // Add buyOrder into mock database of List<BuyOrder> type
            _buyOrdersDb.Add(buyOrder);
            await _buyOrdersDb.SaveChangesAsync();

            // Convert to BuyOrderResponse type
            BuyOrderResponse buyOrderResponse = buyOrder.ToBuyOrderResponse();

            // Return await BuyOrderResponse
            return buyOrderResponse;
        }

        public async Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest)
        {
            // Check if sellOrderRequest is null
            if (sellOrderRequest == null) throw new ArgumentNullException(nameof(sellOrderRequest));

            // Validate
            ValidationHelper.ValidateObject(sellOrderRequest);

            // convert from BuyOrderRequest type to BuyOrder entity
            SellOrder sellOrder = sellOrderRequest.ToSellOrder();

            // Generate sellOrder.SellOrderId
            sellOrder.SellOrderID = Guid.NewGuid();

            // Add sellOrder into mock database of List<SellOrder> type
            _sellOrdersDb.Add(sellOrder);
            await _sellOrdersDb.SaveChangesAsync();

            // Convert to sellOrderResponse type
            SellOrderResponse sellOrderResponse = sellOrder.ToSellOrderResponse();;

            // Return await sellOrderResponse
            return sellOrderResponse;
        }

        public async Task<List<BuyOrderResponse>> GetBuyOrders()
        {
            var buyOrders = await _buyOrdersDb.buyOrderDb.ToListAsync();
            // Doing it WITH a stored db procedure
            return buyOrders.Select(bo => bo.ToBuyOrderResponse()).ToList();
        }

        public async Task<List<SellOrderResponse>> GetSellOrders()
        {
            var sellOrders = await _sellOrdersDb.sellOrderDb.ToListAsync();
            // Doing it WITH a stored db procedure
            return sellOrders.Select(so => so.ToSellOrderResponse()).ToList();
        }

        public async Task<StockViewModel> GetStockInfo(string? stockSymbol)
        {
            if (stockSymbol == null) stockSymbol = "MSFT";

            // Fetch stock price and company profile
            Dictionary<string, object>? stockPrice = await _finnhubService.GetStockPriceQuote(stockSymbol);
            Dictionary<string, object>? stockInfo = await _finnhubService.GetCompanyProfile(stockSymbol);

            StockViewModel fullStock = new StockViewModel()
            {
                StockSymbol = stockSymbol,
                Price = stockPrice != null ? Convert.ToDouble(stockPrice["c"].ToString(), CultureInfo.InvariantCulture) : 0,
                StockName = stockInfo != null ? stockInfo["name"].ToString() : null
            };

            return fullStock;
        }
    }
}
