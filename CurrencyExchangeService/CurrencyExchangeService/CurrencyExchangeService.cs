using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using CurrencyExchangeService.Models;

namespace CurrencyExchangeService
{
    public class CurrencyExchangeService : ICurrencyExchangeService
    {
        private readonly NbpApiClient _nbpApiClient = new NbpApiClient();

        // Simulating user data store
        private static readonly Dictionary<string, User> Users = new Dictionary<string, User>();

        public async Task<User> CreateUserAsync(string username, string password)
        {
            await Task.Delay(100);
            if (Users.ContainsKey(username))
            {
                throw new FaultException("Username already exists.");
            }
            var user = new User
            {
                Username = username,
                Password = password,
                Balance = 0,
                UserCurrencies = new List<UserCurrency>(),
                Transactions = new List<Transaction>()
            };
            Users[username] = user;
            return user;
        }

        public async Task<User> LoginUserAsync(string username, string password)
        {
            await Task.Delay(100);
            if (Users.TryGetValue(username, out var user) && user.Password == password)
            {
                return user;
            }
            throw new FaultException("Invalid username or password.");
        }

        public async Task<bool> TopUpAccountAsync(string username, decimal amount)
        {
            await Task.Delay(100);
            if (Users.TryGetValue(username, out var user))
            {
                user.Balance += amount;
                user.Transactions.Add(new Transaction
                {
                    Type = "TopUp",
                    Amount = amount,
                    Date = DateTime.UtcNow
                });
                return true;
            }
            return false;
        }

        public async Task<decimal> GetExchangeRateAsync(string currencyCode)
        {
            return await _nbpApiClient.GetExchangeRateAsync(currencyCode);
        }

        public async Task<List<ExchangeRate>> GetArchivedExchangeRatesAsync(DateTime startDate, DateTime endDate)
        {
            return await _nbpApiClient.GetArchivedExchangeRatesAsync("USD", startDate, endDate); // Example currency
        }

        public async Task<bool> BuyCurrencyAsync(string username, string currencyCode, decimal amount)
        {
            await Task.Delay(100);
            if (Users.TryGetValue(username, out var user))
            {
                var exchangeRate = await _nbpApiClient.GetExchangeRateAsync(currencyCode);
                var cost = amount / exchangeRate;

                if (user.Balance >= cost)
                {
                    user.Balance -= cost;
                    var userCurrency = user.UserCurrencies.FirstOrDefault(uc => uc.CurrencyCode == currencyCode);
                    if (userCurrency == null)
                    {
                        userCurrency = new UserCurrency { CurrencyCode = currencyCode, Amount = 0 };
                        user.UserCurrencies.Add(userCurrency);
                    }
                    userCurrency.Amount += amount;
                    user.Transactions.Add(new Transaction
                    {
                        Type = "Buy",
                        Amount = amount,
                        Date = DateTime.UtcNow
                    });
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> SellCurrencyAsync(string username, string currencyCode, decimal amount)
        {
            await Task.Delay(100);
            if (Users.TryGetValue(username, out var user))
            {
                var userCurrency = user.UserCurrencies.FirstOrDefault(uc => uc.CurrencyCode == currencyCode);
                if (userCurrency != null && userCurrency.Amount >= amount)
                {
                    var exchangeRate = await _nbpApiClient.GetExchangeRateAsync(currencyCode);
                    var earnings = amount * exchangeRate;

                    userCurrency.Amount -= amount;
                    user.Balance += earnings;
                    user.Transactions.Add(new Transaction
                    {
                        Type = "Sell",
                        Amount = amount,
                        Date = DateTime.UtcNow
                    });
                    return true;
                }
            }
            return false;
        }

        public async Task<decimal> GetUserBalanceAsync(string username)
        {
            await Task.Delay(100);
            if (Users.TryGetValue(username, out var user))
            {
                return user.Balance;
            }
            return 0;
        }

        public async Task<decimal> CalculateExchangeAmountAsync(string fromCurrency, string toCurrency, decimal amount)
        {
            try
            {
                bool isFromCurrencyValid = await _nbpApiClient.ValidateCurrencyCodeAsync(fromCurrency);
                bool isToCurrencyValid = await _nbpApiClient.ValidateCurrencyCodeAsync(toCurrency);

                if (!isFromCurrencyValid || !isToCurrencyValid)
                {
                    throw new FaultException($"Invalid currency code(s): {(!isFromCurrencyValid ? fromCurrency : "")} {(!isToCurrencyValid ? toCurrency : "")}");
                }

                decimal fromRate = await _nbpApiClient.GetExchangeRateAsync(fromCurrency);
                decimal toRate = await _nbpApiClient.GetExchangeRateAsync(toCurrency);

                decimal amountInPln = amount * fromRate;
                decimal convertedAmount = amountInPln / toRate;

                return convertedAmount;
            }
            catch (Exception ex)
            {
                throw new FaultException($"An error occurred while calculating exchange amount: {ex.Message}");
            }
        }
    }
}
