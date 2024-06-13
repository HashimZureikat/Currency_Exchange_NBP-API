using System;

namespace CurrencyExchangeService.Models
{
    public class Transaction
    {
        public string Type { get; set; } // e.g., "TopUp", "Buy", "Sell"
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }
}
