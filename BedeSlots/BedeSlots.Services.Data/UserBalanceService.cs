using BedeSlots.Common;
using BedeSlots.Data;
using BedeSlots.Data.Models;
using BedeSlots.Services.Data.Contracts;
using BedeSlots.Services.Data.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace BedeSlots.Services.Data
{
    //TODO: change the name of the service
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

        public async Task<User> DepositMoneyAsync(decimal amount, string userId)
        {
            var user = await this.context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user.Currency != CommonConstants.BaseCurrency)
            {
                amount = await this.currencyConverterService.ConvertToBaseCurrency(amount, user.Currency);
            }

            user.Balance += amount;

            this.context.Update(user);
            await this.context.SaveChangesAsync();
            return user;
        }

        public async Task<User> RetrieveMoneyAsync(decimal amount, string userId)
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
                throw new ServiceException("Not enough money!");
            }

            this.context.Update(user);
            await this.context.SaveChangesAsync();
            return user;
        }

        public async Task<decimal> GetUserBalanceByIdAsync(string userId)
        {
            var user = await this.context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            return user.Balance;
        }

    }
}
