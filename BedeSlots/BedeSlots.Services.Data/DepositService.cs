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
        private readonly ITransactionService transactionService;

        public DepositService(BedeSlotsDbContext context, ICurrencyConverterService currencyConverterService, ITransactionService transactionService)
        {
            this.context = context;
            this.currencyConverterService = currencyConverterService;
            this.transactionService = transactionService;
        }

        public async Task<User> DepositMoneyAsync(decimal amount, string userId)
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
