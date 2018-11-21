using BedeSlots.Services.External.Contracts;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BedeSlots.Services.External
{
    public class ExchangeRatesApiCaller : IExchangeRatesApiCaller
    {
        public async Task<string> GetCurrenciesRatesAsync()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri("https://api.exchangeratesapi.io");
                    var response = await client.GetAsync($"/latest?base=USD&symbols=EUR,GBP,BGN");
                    response.EnsureSuccessStatusCode();

                    var stringResult = await response.Content.ReadAsStringAsync();
                    return stringResult;
                }
                catch (HttpRequestException httpRequestException)
                {
                    return httpRequestException.Message;
                }
            }
        }
    }
}
