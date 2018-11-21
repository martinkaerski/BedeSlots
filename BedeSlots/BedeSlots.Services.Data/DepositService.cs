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

        public DepositService(BedeSlotsDbContext context)
        {
            this.context = context;
        }

        public async Task<Transaction> DepositAsync(Transaction transaction)
        {
            var user = await this.context.Users.FirstOrDefaultAsync(u => u.Id == transaction.UserId);
            user.Balance += transaction.Amount;

            return transaction;
        }
    }
}
