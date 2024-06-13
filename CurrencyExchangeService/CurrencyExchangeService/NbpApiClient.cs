using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using CurrencyExchangeService.Models;
using Newtonsoft.Json.Linq;

public class NbpApiClient
{
    private static readonly HttpClient client = new HttpClient();

    public async Task<decimal> GetExchangeRateAsync(string currencyCode)
    {
        var response = await client.GetStringAsync($"http://api.nbp.pl/api/exchangerates/rates/A/{currencyCode}/?format=json");
        var json = JObject.Parse(response);
        var rate = json["rates"][0]["mid"].Value<decimal>();
        return rate;
    }

    public async Task<List<ExchangeRate>> GetArchivedExchangeRatesAsync(string currencyCode, DateTime startDate, DateTime endDate)
    {
        var response = await client.GetStringAsync($"http://api.nbp.pl/api/exchangerates/rates/A/{currencyCode}/{startDate:yyyy-MM-dd}/{endDate:yyyy-MM-dd}/?format=json");
        var json = JObject.Parse(response);
        var rates = new List<ExchangeRate>();

        foreach (var item in json["rates"])
        {
            rates.Add(new ExchangeRate
            {
                CurrencyCode = currencyCode,
                Rate = item["mid"].Value<decimal>(),
                Date = item["effectiveDate"].Value<DateTime>()
            });
        }
        return rates;
    }

    public async Task<bool> ValidateCurrencyCodeAsync(string currencyCode)
    {
        var response = await client.GetAsync($"http://api.nbp.pl/api/exchangerates/rates/A/{currencyCode}/?format=json");
        return response.IsSuccessStatusCode;
    }
}
