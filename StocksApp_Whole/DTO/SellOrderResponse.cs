using StocksApp_Whole.Entities;

namespace StocksApp_Whole.DTO
{
    public class SellOrderResponse
    {
        public Guid SellOrderID { get; set; }

        public string? StockSymbol { get; set; }

        public string? StockName { get; set; }

        public DateTime DateAndTimeOfOrder { get; set; }

        public uint Quantity { get; set; }

        public double Price { get; set; }

        public double TradeAmount { get; set; }

        public override string ToString()
        {
            return $"StockName : {StockName}\nStockSymbol : {StockSymbol}\nBuyOrder Id : {SellOrderID}\nDate/Time of Order : {DateAndTimeOfOrder}\nQuantity : {Quantity}\nPrice : {Price}\nTrade Amount : {TradeAmount}";
        }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(SellOrderResponse)) return false;

            SellOrderResponse SellOrderResponse = (SellOrderResponse)obj;
            return
                SellOrderID == SellOrderResponse.SellOrderID &&
                StockSymbol == SellOrderResponse.StockSymbol &&
                StockName == SellOrderResponse.StockName &&
                DateAndTimeOfOrder == SellOrderResponse.DateAndTimeOfOrder &&
                Quantity == SellOrderResponse.Quantity &&
                Price == SellOrderResponse.Price &&
                TradeAmount == SellOrderResponse.TradeAmount;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public static class SOExtensions
    {

        public static SellOrderResponse ToSellOrderResponse(this SellOrder sellOrder)
        {
            return new SellOrderResponse()
            {
                StockSymbol = sellOrder.StockSymbol,
                StockName = sellOrder.StockName,
                DateAndTimeOfOrder = sellOrder.DateAndTimeOfOrder,
                Quantity = sellOrder.Quantity,
                Price = sellOrder.Price,
                SellOrderID = sellOrder.SellOrderID,
                TradeAmount = sellOrder.Price * sellOrder.Quantity
            };
        }

    }
}
