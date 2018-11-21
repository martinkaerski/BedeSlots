using BedeSlots.Data.Models;
using BedeSlots.Data.Models.Dto;
using BedeSlots.Services.Data.Contracts;
using BedeSlots.Services.External.Contracts;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BedeSlots.Services.Data
{
    public class ExchangeRateApiCallService : IExchangeRateApiCallService
    {
        private readonly IExchangeRatesApiCaller exchangeRateApiCaller;

        public ExchangeRateApiCallService(IExchangeRatesApiCaller exchangeRateApiCaller)
        {
            this.exchangeRateApiCaller = exchangeRateApiCaller;
        }

        //TODO: Deserialise async?
        public async Task<IDictionary<Currency, decimal>> GetAllRatesAsync()
        {
            var stringResultRates = await this.exchangeRateApiCaller.GetCurrenciesRatesAsync();
            var deserializedRates = JsonConvert.DeserializeObject<CurrencyDto>(stringResultRates);

            var ratesKVP = new Dictionary<Currency, decimal>
            {
                { Currency.BGN, decimal.Parse(deserializedRates.Rates.BGN)},
                { Currency.EUR, decimal.Parse(deserializedRates.Rates.EUR)},
                { Currency.GBP, decimal.Parse(deserializedRates.Rates.GBP)},
                { Currency.USD, 1m }
            };

            return ratesKVP;
        }

        public async Task<decimal> GetRateAsync(Currency currencyName)
        {
            var rates = await GetAllRatesAsync();
            return rates[currencyName];
        }
    }
}
