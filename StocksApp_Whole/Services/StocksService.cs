using StocksApp_Whole.DTO;
using StocksApp_Whole.ServiceContracts;
using StocksApp_Whole.Entities;
using StocksApp_Whole.Services.Helpers;

namespace StocksApp_Whole.Services
{
    public class StocksService : IStocksService
    {

        private readonly List<BuyOrder> _buyOrders;
        private readonly List<SellOrder> _sellOrders;

        public StocksService()
        {
            _buyOrders = new List<BuyOrder>();
            _sellOrders = new List<SellOrder>();
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

        public Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest)
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
            _buyOrders.Add(buyOrder);

            // Convert to BuyOrderResponse type
            BuyOrderResponse buyOrderResponse = buyOrder.ToBuyOrderResponse();

            // Return await BuyOrderResponse
            return Task.FromResult(buyOrderResponse);
        }

        public Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest)
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
            _sellOrders.Add(sellOrder);

            // Convert to sellOrderResponse type
            SellOrderResponse sellOrderResponse = sellOrder.ToSellOrderResponse();;

            // Return await sellOrderResponse
            return Task.FromResult(sellOrderResponse);
        }

        public Task<List<BuyOrderResponse>> GetBuyOrders()
        {
            List<BuyOrderResponse> AllBuyOrders = _buyOrders.Select(buyorder => buyorder.ToBuyOrderResponse()).ToList();
            return Task.FromResult(AllBuyOrders);
        }

        public Task<List<SellOrderResponse>> GetSellOrders()
        {
            List<SellOrderResponse> AllSellOrders = _sellOrders.Select(sellorder => sellorder.ToSellOrderResponse()).ToList();
            return Task.FromResult(AllSellOrders);
        }
    }
}
