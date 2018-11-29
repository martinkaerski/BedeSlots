using System.Threading.Tasks;
using BedeSlots.Data.Models;

namespace BedeSlots.Services.Data.Contracts
{
    public interface IDepositService
    {
        Task<User> DepositAsync(decimal amount, string userId);

        Task<User> WithdrawMoneyAsync(decimal amount, string userId);
    }
}