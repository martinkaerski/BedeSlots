using BedeSlots.Data;
using BedeSlots.Data.Models;
using BedeSlots.DTO;
using BedeSlots.Services.Data.Contracts;
using BedeSlots.Services.Data.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BedeSlots.Services.Data
{
    public class TransactionService : ITransactionService
    {
        private readonly BedeSlotsDbContext context;
        private readonly ICurrencyConverterService currencyConverterService;

        public TransactionService(BedeSlotsDbContext context, ICurrencyConverterService currencyConverterService)
        {
            this.context = context ?? throw new ServiceException(nameof(context));
            this.currencyConverterService = currencyConverterService ?? throw new ServiceException(nameof(currencyConverterService));
        }

        public IQueryable<TransactionDto> GetAllTransactions()
        {
            var transactions = this.context.Transactions
                                           .Select(t => new TransactionDto
                                           {
                                               Date = t.Date,
                                               Amount = t.Amount,
                                               Description = t.Description,
                                               Type = t.Type,
                                               User = t.User.Email
                                           })
                                           .AsQueryable();

            return transactions;
        }

        public async Task<Transaction> GetTransactionByIdAsync(int transactionId)
        {
            if (!await this.context.Transactions.AnyAsync(t => t.Id == transactionId))
            {
                throw new ServiceException($"Transaction with Id:{transactionId} not exist!");
            }
            var transaction = await this.context.Transactions
                .Include(t => t.User)
                .ThenInclude(u => u.Cards)
                .SingleOrDefaultAsync(t => t.Id == transactionId);

            return transaction;
        }

        public async Task<Transaction> AddTransactionAsync(TransactionType type, string userId, string description, decimal amount, Currency currency)
        {
            if (userId == null)
            {
                throw new ServiceException("UserId can not be null!");
            }
            if (amount < 0)
            {
                throw new ServiceException("Amount must be positive number!");
            }

            var convertedAmount = await this.currencyConverterService.ConvertToBaseCurrencyAsync(amount, currency);

            var transaction = new Transaction()
            {
                Amount = convertedAmount,
                Date = DateTime.Now,
                Type = type,
                UserId = userId,
                Description = description
            };

            await this.context.Transactions.AddAsync(transaction);
            await this.context.SaveChangesAsync();

            return transaction;
        }

        public IQueryable<TransactionDto> GetUserTransactions(string userId)
        {
            var transactions = this.context.Transactions
               .Where(t => t.UserId == userId)
               .Select(t => new TransactionDto
               {
                   Date = t.Date,
                   Amount = t.Amount,
                   Description = t.Description,
                   Type = t.Type,
                   User = t.User.Email
               })
               .AsQueryable();

            return transactions;
        }
    }
}
