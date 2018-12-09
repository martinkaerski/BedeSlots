using BedeSlots.Data;
using BedeSlots.Data.Models;
using BedeSlots.Services.Data.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BedeSlots.Services.Data
{
    public class CurrencyService : ICurrencyService
    {
        private readonly BedeSlotsDbContext context;

        public CurrencyService(BedeSlotsDbContext context)
        {
            this.context = context;
        }

        public async Task<Currency> GetUserCurrency(string userId)
        {
            var currency = await this.context.Users
                .Where(u => u.Id == userId)
                .Select(u => u.Currency)
                .FirstOrDefaultAsync();

            return currency;
        }

        public ICollection<Currency> GetAllCurrenciesNames()
        {
           return Enum.GetValues(typeof(Currency)).Cast<Currency>().ToList();
        }
    }
}
