using BedeSlots.Data;
using BedeSlots.Data.Models;
using BedeSlots.Services.Data.Contracts;
using BedeSlots.Services.Data.Exceptions;
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
            this.context = context ?? throw new ServiceException(nameof(context));
        }

        public async Task<Currency> GetUserCurrencyAsync(string userId)
        {
            if (userId == null)
            {
                throw new ServiceException("UserId can not be null!");
            }

            var currency = await this.context.Users
                .Where(u => u.Id == userId)
                .Select(u => u.Currency)
                .FirstOrDefaultAsync();

            return currency;
        }

        public ICollection<Currency> GetAllCurrencies()
        {
           return Enum.GetValues(typeof(Currency)).Cast<Currency>().ToList();
        }
    }
}
