using BedeSlots.Data.Models;
using System.Threading.Tasks;

namespace BedeSlots.Services.Data.Contracts
{
    public interface ITransactionService
    {
        Task<Transaction> RegisterTransactionsAsync(Transaction transaction);

    }
}