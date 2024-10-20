using StocksApp_Whole.Entities;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace StocksApp_Whole.DTO
{
    public class BuyOrderResponse
    {
        public Guid BuyOrderID { get; set; }

        public string? StockSymbol { get; set; }

        public string? StockName { get; set; }

        public DateTime DateAndTimeOfOrder { get; set; }

        public uint Quantity { get; set; }

        public double Price { get; set; }

        public double TradeAmount { get; set; }

        public override string ToString()
        {
            return $"StockName : {StockName}\nStockSymbol : {StockSymbol}\nBuyOrder Id : {BuyOrderID}\nDate/Time of Order : {DateAndTimeOfOrder}\nQuantity : {Quantity}\nPrice : {Price}\nTrade Amount : {TradeAmount}";
        }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(BuyOrderResponse)) return false;

            BuyOrderResponse buyOrderResponse = (BuyOrderResponse)obj;

            return
                BuyOrderID == buyOrderResponse.BuyOrderID &&
                StockSymbol == buyOrderResponse.StockSymbol &&
                StockName == buyOrderResponse.StockName &&
                DateAndTimeOfOrder == buyOrderResponse.DateAndTimeOfOrder &&
                Quantity == buyOrderResponse.Quantity &&
                Price == buyOrderResponse.Price &&
                TradeAmount == buyOrderResponse.TradeAmount;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }

    public static class BOExtensions
    {

        public static BuyOrderResponse ToBuyOrderResponse(this BuyOrder buyOrder)
        {
            return new BuyOrderResponse()
            {
                StockSymbol = buyOrder.StockSymbol,
                StockName = buyOrder.StockName,
                DateAndTimeOfOrder = buyOrder.DateAndTimeOfOrder,
                Quantity = buyOrder.Quantity,
                Price = buyOrder.Price,
                BuyOrderID = buyOrder.BuyOrderID,
                TradeAmount = buyOrder.Price * buyOrder.Quantity
            };
        }

    }
}
