using BedeSlots.Common;
using BedeSlots.Data;
using BedeSlots.Data.Models;
using BedeSlots.Services.Data.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BedeSlots.Services.Data
{
    public class UserBalanceService : IUserBalanceService
    {
        private readonly BedeSlotsDbContext context;
        private readonly ICurrencyConverterService currencyConverterService;
        private readonly ITransactionService transactionService;

        public UserBalanceService(BedeSlotsDbContext context, ICurrencyConverterService currencyConverterService, ITransactionService transactionService)
        {
            this.context = context;
            this.currencyConverterService = currencyConverterService;
            this.transactionService = transactionService;
        }

        public async Task<decimal> DepositMoneyAsync(decimal amount, string userId)
        {
            var user = await this.context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user.Currency != CommonConstants.BaseCurrency)
            {
                amount = await this.currencyConverterService.ConvertToBaseCurrency(amount, user.Currency);
            }

            user.Balance += amount;

            this.context.Update(user);
            await this.context.SaveChangesAsync();
            return amount;
        }

        public async Task<decimal> ReduceMoneyAsync(decimal amount, string userId)
        {
            var user = await this.context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user.Currency != CommonConstants.BaseCurrency)
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
            }

            this.context.Update(user);
            await this.context.SaveChangesAsync();
            return amount;
        }

        public async Task<decimal> GetUserBalanceByIdAsync(string userId)
        {
            var user = await this.context.Users
                            .Where(u => u.Id == userId)
                            .Select(u => new
                            {
                                Balance = u.Balance,
                                Currency = u.Currency
                            })
                            .FirstOrDefaultAsync();

            decimal balance = user.Balance;

            if (user.Currency != CommonConstants.BaseCurrency)
            {
                balance = await this.currencyConverterService.ConvertFromBaseToOther(balance, user.Currency);
            }

            return balance;
        }

        public async Task<decimal> GetUserBalanceByIdInBaseCurrencyAsync(string userId)
        {
            var balance = await this.context.Users
                            .Where(u => u.Id == userId)
                            .Select(u => u.Balance)
                            .FirstOrDefaultAsync();

            return balance;
        }
    }
}
