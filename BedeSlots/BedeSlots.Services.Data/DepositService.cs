using BedeSlots.Data;
using BedeSlots.Data.Models;
using BedeSlots.Services.Data.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace BedeSlots.Services.Data
{
    //TODO: change the name of the service
    public class DepositService : IDepositService
    {
        private readonly BedeSlotsDbContext context;
        private readonly ICurrencyConverterService currencyConverterService;

        public DepositService(BedeSlotsDbContext context, ICurrencyConverterService currencyConverterService)
        {
            this.context = context;
            this.currencyConverterService = currencyConverterService;
        }

        public async Task<User> DepositAsync(decimal amount, string userId)
        {
            var user = await this.context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user.Currency != Currency.USD)
            {
                amount = await this.currencyConverterService.ConvertToBaseCurrency(amount, user.Currency);
            }

            user.Balance += amount;

            this.context.Update(user);
            await this.context.SaveChangesAsync();
            return user;
        }

        public async Task<User> WithdrawMoneyAsync(decimal amount, string userId)
        {
            var user = await this.context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user.Currency != Currency.USD)
            {
                amount = await this.currencyConverterService.ConvertToBaseCurrency(amount, user.Currency);
            }

            if (user.Balance >= amount)
            {
                user.Balance -= amount;
            }
            else
            {
                throw new InvalidOperationException("Not enough money!");
                //TODO: what to do?
            }

            this.context.Update(user);
            await this.context.SaveChangesAsync();
            return user;
        }
    }
}
