using StocksApp_Whole.DTO;

namespace StocksApp_Whole.ServiceContracts
{
    public interface IStocksService
    {
        public BuyOrderRequest CreateMockBuyOrderRequest();
        public List<BuyOrderRequest> CreateManyMockBuyOrderRequests();
        public SellOrderRequest CreateMockSellOrderRequest();
        public List<SellOrderRequest> CreateManyMockSellOrderRequests();

        Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest);

        Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest);

        Task<List<BuyOrderResponse>> GetBuyOrders();

        Task<List<SellOrderResponse>> GetSellOrders();
    }
}
