using BedeSlots.Data.Models;
using BedeSlots.Services.Data.Contracts;
using System.Collections.Generic;

namespace BedeSlots.Services.Data
{
    public class CurrencyService : ICurrencyService
    {
        public CurrencyService()
        {
        }

        public ICollection<Currency> GetAllCurrenciesNames()
        {
            var currencies = new List<Currency>
            {
                Currency.USD,
                Currency.BGN,
                Currency.EUR,
                Currency.GBP
            };

            return currencies;
        }
    }
}
