using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BedeSlots.Data;
using BedeSlots.Data.Models;
using BedeSlots.Services.Data.Contracts;
using Microsoft.EntityFrameworkCore;

namespace BedeSlots.Services.Data
{
    public class DepositService : IDepositService
    {
        private readonly BedeSlotsDbContext context;
        private readonly ICurrencyConverterService currencyConverterService;

        public DepositService(BedeSlotsDbContext context, ICurrencyConverterService currencyConverterService)
        {
            this.context = context;
            this.currencyConverterService = currencyConverterService;
        }

        public async Task<Transaction> DepositAsync(Transaction transaction)
        {
            var user = await this.context.Users.FirstOrDefaultAsync(u => u.Id == transaction.UserId);
            double amount = transaction.Amount;

            //TODO: use one of them
            var usdId = (int)CurrencyName.USD;
            var userCurrencyName = (CurrencyName)user.CurrencyId;
            
            if (user.CurrencyId != usdId)
            {
               amount = await this.currencyConverterService.ConvertToUsd(transaction.Amount, userCurrencyName);
            }

            user.Balance += amount;
            return transaction;
        }
    }
}
