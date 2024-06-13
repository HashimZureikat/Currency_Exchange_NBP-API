using System.Collections.Generic;

namespace CurrencyExchangeService.Models
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public decimal Balance { get; set; }
        public List<UserCurrency> UserCurrencies { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
}
