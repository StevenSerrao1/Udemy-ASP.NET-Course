namespace StocksApp_Whole.Models
{
    public class QuoteModel
    {
        public string? StockSymbol { get; set; }
        public double? CurrentPrice { get; set; }
        public double? LowestPrice { get; set; }
        public double? HighestPrice { get; set; }
        public double? OpenPrice { get; set; }
    }
}
