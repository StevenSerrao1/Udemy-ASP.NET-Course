using StocksApp_Whole.DTO;

namespace StocksApp_Whole.Models
{
    /// <summary>
    /// Represents model class to supply list of buy orders and sell orders to the Trades/Orders view
    /// </summary>
    public class OrdersModel
    {
        public List<BuyOrderResponse> BuyOrders { get; set; } = new List<BuyOrderResponse>();
        public List<SellOrderResponse> SellOrders { get; set; } = new List<SellOrderResponse>();
    }
}