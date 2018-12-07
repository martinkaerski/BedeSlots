using BedeSlots.Data.Models;
using BedeSlots.DTO;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BedeSlots.Services.Data.Contracts
{
    public interface IUserService
    {
        Task<User> GetUserByIdAsync(string id);

        Task<decimal> GetUserBalanceByIdAsync(string userId);

        IQueryable<UserDto> GetAllUsers();

        Task<string> GetUserRoleIdAsync(string userId);

        Task<IEnumerable<Transaction>> GetUserTransactionsAsync(string id);

        Task<ICollection<IdentityRole>> GetAllRolesAsync();

        Task<IdentityRole> EditUserRoleAsync(string userId, string newRoleId);

        Task<IdentityUserRole<string>> GetUserRoleAsync(string userId);

        Task<string> GetUserRoleNameAsync(string userId);

        Task<User> DeleteUserAsync(string userId);

        Task<Currency> GetUserCurrencyByIdAsync(string userId);
    }
}