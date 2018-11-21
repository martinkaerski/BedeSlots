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
        public async Task<IDictionary<CurrencyName, double>> GetAllRatesAsync()
        {
            var stringResultRates = await this.exchangeRateApiCaller.GetCurrenciesRatesAsync();
            var deserializedRates = JsonConvert.DeserializeObject<CurrencyDto>(stringResultRates);

            var ratesKVP = new Dictionary<CurrencyName, double>
            {
                { CurrencyName.BGN, double.Parse(deserializedRates.Rates.BGN)},
                { CurrencyName.EUR, double.Parse(deserializedRates.Rates.EUR)},
                { CurrencyName.GBP, double.Parse(deserializedRates.Rates.GBP)},
            };

            return ratesKVP;
        }

        public async Task<double> GetRateAsync(CurrencyName currencyName)
        {
            var rates = await GetAllRatesAsync();
            return rates[currencyName];
        }
    }
}
