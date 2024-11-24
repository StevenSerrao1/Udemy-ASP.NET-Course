using StocksApp_Whole.DTO;

namespace StockMarketSolution.Models
{
    public class Stocks
    {
        public string? StockSymbol { get; set; }
        public string? StockName { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj is not Stocks) return false;

            Stocks other = (Stocks)obj;
            return StockSymbol == other.StockSymbol && StockName == other.StockName;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}