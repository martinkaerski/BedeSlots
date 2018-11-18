using BedeSlots.Data;
using BedeSlots.Data.Models;
using BedeSlots.Services.Data.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BedeSlots.Services.Data
{
    public class TransactionService : ITransactionService
    {
        private readonly BedeSlotsDbContext context;

        public TransactionService(BedeSlotsDbContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Transaction> RegisterTransactionsAsync(Transaction transaction)
        {
            await this.context.Transactions.AddAsync(transaction);
            await this.context.SaveChangesAsync();

            return transaction;
        }

        
    }
}
