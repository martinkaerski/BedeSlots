using BedeSlots.Data.Models;
using System.Threading.Tasks;

namespace BedeSlots.Services.Data.Contracts
{
    public interface IUserService
    {
        Task<User> GetUserById(string id);
        Task<decimal> GetUserBalanceById(string userId);
    }
}