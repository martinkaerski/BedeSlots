using BedeSlots.Data;
using BedeSlots.Data.Models;
using BedeSlots.Services.Data.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BedeSlots.Services.Data
{
    public class CurrencyService : ICurrencyService
    {
        private readonly BedeSlotsDbContext context;

        public CurrencyService(BedeSlotsDbContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<ICollection<Currency>> GetAllCurrenciesAsync()
        {
            var currencies = await this.context.Currencies.ToListAsync();
            return currencies;
        }

        public async Task<Currency> GetCurrencyAsync(CurrencyName name)
        {
            var currency = await this.context.Currencies.SingleOrDefaultAsync(c => c.Name == name);
            return currency;
        }
    }
}
