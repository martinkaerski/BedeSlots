using BedeSlots.Data;
using BedeSlots.Data.Models;
using BedeSlots.Services.Data.Contracts;
using System;
using System.Threading.Tasks;

namespace BedeSlots.Services.Data
{
    public class CurrencyConverterService : ICurrencyConverterService
    {
        private readonly BedeSlotsDbContext context;
        private readonly ICurrencyService currencyService;

        public CurrencyConverterService(BedeSlotsDbContext context, ICurrencyService currencyService)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.currencyService = currencyService;
        }

        public async Task<double> ConvertToUsd(double amount, CurrencyName currencyName)
        {
            var currency = await this.currencyService.GetCurrencyAsync(currencyName);
            var rateToBaseCurrency = currency.RateToBaseCurrency;
            
            return amount * (1 / rateToBaseCurrency);
        }
    }
}
