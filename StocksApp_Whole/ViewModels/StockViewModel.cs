using System.ComponentModel.DataAnnotations;

namespace StocksApp_Whole.ViewModels
{
    public class StockViewModel
    {
        public string? StockSymbol { get; set; }

        public string? StockName { get; set; }

        public double Price { get; set; }

        [Range(1, 1000000, ErrorMessage ="Enter a number between 1 - 1 000 000")]
        public uint Quantity { get; set; }
    }
}
