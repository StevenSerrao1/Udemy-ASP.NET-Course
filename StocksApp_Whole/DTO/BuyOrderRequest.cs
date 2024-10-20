using System.ComponentModel.DataAnnotations;
using StocksApp_Whole.DTO.ValidationAttributes;
using StocksApp_Whole.Entities;

namespace StocksApp_Whole.DTO
{
    public class BuyOrderRequest
    {
        [Required]
        public string? StockSymbol { get; set; }

        [Required]
        public string? StockName { get; set; }

        [MinDate("01/01/2000")]
        public DateTime DateAndTimeOfOrder { get; set; }

        [Range(1, 1000000)]
        public uint Quantity { get; set; }

        [Range(1, 1000000)]
        public double Price { get; set; }

        public override string ToString()
        {
            return $"StockName : {StockName}\nStockSymbol : {StockSymbol}\nDate/Time of Order : {DateAndTimeOfOrder}\nQuantity : {Quantity}\nPrice : {Price}\n";
        }

        public BuyOrder ToBuyOrder()
        {
            return new BuyOrder
            {
                StockSymbol = StockSymbol,
                StockName = StockName,
                Price = Price,
                Quantity = Quantity,
                DateAndTimeOfOrder = DateAndTimeOfOrder,
            };
        }

    }
}
