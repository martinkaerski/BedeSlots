using BedeSlots.Data;
using BedeSlots.Data.Models;
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
        private readonly BedeSlotsDbContext context;
        private readonly ICurrencyService currencyService;

        public ExchangeRateApiCaller(BedeSlotsDbContext context, ICurrencyService currencyService)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.currencyService = currencyService;
        }

        public async Task<bool> GetCurrenciesRates()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri("https://api.exchangeratesapi.io");
                    var response = await client.GetAsync($"/latest?base=USD&symbols=EUR,GBP,BGN");
                    response.EnsureSuccessStatusCode();

                    var stringResult = await response.Content.ReadAsStringAsync();
                    var rates = JsonConvert.DeserializeObject<CurrencyDto>(stringResult);

                    var currencies = await this.currencyService.GetAllCurrenciesAsync();

                    foreach (var currency in currencies)
                    {
                        try
                        {
                            if (currency.Name == CurrencyName.EUR)
                            {
                                currency.RateToBaseCurrency = double.Parse(rates.Rates.EUR);
                            }
                            else if (currency.Name == CurrencyName.BGN)
                            {
                                currency.RateToBaseCurrency = double.Parse(rates.Rates.BGN);
                            }
                            else if (currency.Name == CurrencyName.GBP)
                            {
                                currency.RateToBaseCurrency = double.Parse(rates.Rates.GBP);
                            }

                        }
                        catch (Exception)
                        {
                            throw new ArgumentException();
                        }
                    }

                    this.context.Currencies.UpdateRange(currencies);
                    await this.context.SaveChangesAsync();

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
