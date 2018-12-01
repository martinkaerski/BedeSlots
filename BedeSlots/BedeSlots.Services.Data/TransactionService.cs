using BedeSlots.Data;
using BedeSlots.Data.Models;
using BedeSlots.Services.Data.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
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

        public IQueryable<Transaction> GetAllTransactions()
        {
            var transactions = this.context.Transactions
                .Include(t => t.User)
                //.Select(t => new TransactionDto
                //{
                //    Date = t.Date,
                //    Amount = t.Amount,
                //    Description = t.Description, 
                //    Type = t.Type,
                //    User = t.User.Email
                //})
                .AsQueryable();

            return transactions;
        }

        public async Task<Transaction> GetTransactionByIdAsync(int id)
        {
            var transaction = await this.context.Transactions
                .Include(t => t.User)
                .ThenInclude(u => u.Cards)
                .SingleOrDefaultAsync(t => t.Id == id);

            return transaction;
        }

        public async Task<Transaction> AddTransactionAsync(TransactionType type, string userId, string description, decimal amount)
        {
            var transaction = new Transaction()
            {
                Amount = amount,
                Date = DateTime.Now,
                Type = type,
                UserId = userId,
                Description = description
            };

            await this.context.Transactions.AddAsync(transaction);
            await this.context.SaveChangesAsync();

            return transaction;
        }
    }
}
