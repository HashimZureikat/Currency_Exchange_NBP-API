using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using CurrencyExchangeService.Models;

namespace CurrencyExchangeService
{
    [ServiceContract]
    public interface ICurrencyExchangeService
    {
        [OperationContract]
        Task<User> CreateUserAsync(string username, string password);

        [OperationContract]
        Task<User> LoginUserAsync(string username, string password);

        [OperationContract]
        Task<bool> TopUpAccountAsync(string username, decimal amount);

        [OperationContract]
        Task<decimal> GetExchangeRateAsync(string currencyCode);

        [OperationContract]
        Task<List<ExchangeRate>> GetArchivedExchangeRatesAsync(DateTime startDate, DateTime endDate);

        [OperationContract]
        Task<bool> BuyCurrencyAsync(string username, string currencyCode, decimal amount);

        [OperationContract]
        Task<bool> SellCurrencyAsync(string username, string currencyCode, decimal amount);

        [OperationContract]
        Task<decimal> GetUserBalanceAsync(string username);

        [OperationContract]
        Task<decimal> CalculateExchangeAmountAsync(string fromCurrency, string toCurrency, decimal amount);
    }
}
