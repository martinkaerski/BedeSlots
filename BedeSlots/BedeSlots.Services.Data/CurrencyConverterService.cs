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
        private readonly IExchangeRateApiCallService exchangeRateApiCallService;

        public CurrencyConverterService(BedeSlotsDbContext context, IExchangeRateApiCallService exchangeRateApiCallService)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.exchangeRateApiCallService = exchangeRateApiCallService;
        }

        public async Task<double> ConvertToUsd(double amount, CurrencyName currencyName)
        {
            var rateToBaseCurrency = await this.exchangeRateApiCallService.GetRateAsync(currencyName);
            
            return amount * (1 / rateToBaseCurrency);
        }
    }
}
