using BedeSlots.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BedeSlots.Services.Data.Contracts
{
    public interface ITransactionService
    {
        Task<ICollection<Transaction>> GetAllTransactionsAsync();

        Task<Transaction> GetTransactionByIdAsync(int id);

        Task<Transaction> AddTransactionAsync(TransactionType type, string userId, int? cardId, decimal amount, GameType? gameType);
    }
}