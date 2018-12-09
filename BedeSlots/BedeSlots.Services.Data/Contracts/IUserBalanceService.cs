using System.Threading.Tasks;
using BedeSlots.Data.Models;

namespace BedeSlots.Services.Data.Contracts
{
    public interface IUserBalanceService
    {
        Task<User> DepositMoneyAsync(decimal amount, string userId);

        Task<User> ReduceMoneyAsync(decimal amount, string userId);

        Task<decimal> GetUserBalanceByIdAsync(string userId);

        Task<decimal> GetUserBalanceByIdInBaseCurrencyAsync(string userId);
    }
}