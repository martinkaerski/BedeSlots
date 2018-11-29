﻿using BedeSlots.Data;
using BedeSlots.Data.Models;
using BedeSlots.Services.Data.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        public async Task<ICollection<Transaction>> GetAllTransactionsAsync()
        {
            var transactions = await this.context.Transactions
                .Include(t => t.User)
                .ToListAsync();

            return transactions;
        }

        public async Task<Transaction> GetTransactionAsync(int id)
        {
            var transaction = await this.context.Transactions
                .Include(t => t.User)
                .ThenInclude(u => u.Cards)
                .SingleOrDefaultAsync(t => t.Id == id);

            return transaction;
        }

        public Transaction CreateTransaction(TransactionType type, string userId, int? cardId, decimal amount)
        {
            var transaction = new Transaction()
            {
                Amount = amount,
                Date = DateTime.Now,
                Type = type,
                CardId = cardId,
                UserId = userId,
            };

            return transaction;
        }
    }
}
