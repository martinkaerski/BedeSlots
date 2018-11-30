using BedeSlots.Data.Models;
using BedeSlots.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BedeSlots.Services.Data.Contracts
{
    public interface IUserService
    {
        Task<User> GetUserById(string id);

        Task<decimal> GetUserBalanceById(string userId);

        Task<ICollection<UserDto>> GetAllUsersAsync();

        Task<string> GetUserRole(User user);

        Task<IEnumerable<Transaction>> GetUserTransactionsAsync(string id);
    }
}