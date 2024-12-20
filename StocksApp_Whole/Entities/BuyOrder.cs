﻿using System.ComponentModel.DataAnnotations;

namespace StocksApp_Whole.Entities
{
    public class BuyOrder
    {
        [Key]
        public Guid BuyOrderID { get; set; }

        [Required]
        [StringLength(10)]
        public string? StockSymbol { get; set; }

        [Required]
        [StringLength(30)]
        public string? StockName { get; set; }

        public DateTime DateAndTimeOfOrder { get; set; }

        [Range(1, 1000000)]
        public uint Quantity { get; set; }

        [Range(1, 1000000)]
        public double Price { get; set; }
    }
}
