using BedeSlots.Data.Models;
using BedeSlots.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BedeSlots.Services.Data.Contracts
{
    public interface ITransactionService
    {
        IQueryable<TransactionDto> GetAllTransactions();

        IQueryable<TransactionDto> GetUserTransactionsAsync(string id);

        Task<Transaction> GetTransactionByIdAsync(int id);

        Task<Transaction> AddTransactionAsync(TransactionType type, string userId, string description, decimal amount, Currency currrency);
    }
}