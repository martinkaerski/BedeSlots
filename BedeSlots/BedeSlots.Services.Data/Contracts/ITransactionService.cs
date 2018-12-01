using BedeSlots.Data.Models;
using System.Linq;
using System.Threading.Tasks;

namespace BedeSlots.Services.Data.Contracts
{
    public interface ITransactionService
    {
        IQueryable<Transaction> GetAllTransactions();

        Task<Transaction> GetTransactionByIdAsync(int id);

        Task<Transaction> AddTransactionAsync(TransactionType type, string userId, string description, decimal amount);
    }
}