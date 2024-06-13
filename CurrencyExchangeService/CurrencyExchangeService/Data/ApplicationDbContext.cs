using Microsoft.EntityFrameworkCore;
using CurrencyExchangeService.Models;

namespace CurrencyExchangeService.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<ExchangeRate> ExchangeRates { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<UserCurrency> UserCurrencies { get; set; }
    }
}
