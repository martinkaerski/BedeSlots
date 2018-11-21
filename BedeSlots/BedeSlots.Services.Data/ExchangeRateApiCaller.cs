using BedeSlots.Data.Models.Dto;
using BedeSlots.Services.Data.Contracts;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BedeSlots.Services.Data
{
    public class ExchangeRateApiCaller : IExchangeRateApiCaller
    {
        public async Task<bool> GetCurrenciesRates()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri("https://api.exchangeratesapi.io");
                    var response = await client.GetAsync($"/latest?symbols=USD,GBP,BGN");
                    response.EnsureSuccessStatusCode();

                    var stringResult = await response.Content.ReadAsStringAsync();
                    var rates = JsonConvert.DeserializeObject<CurrencyDto>(stringResult);
                    return true;
                }
                catch (HttpRequestException httpRequestException)
                {
                    return false;
                }
            }
        }
    }
}
