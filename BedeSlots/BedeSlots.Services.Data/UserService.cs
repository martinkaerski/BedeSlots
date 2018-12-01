using BedeSlots.Data;
using BedeSlots.Data.Models;
using BedeSlots.DTO;
using BedeSlots.Services.Data.Contracts;
using BedeSlots.Services.Data.Models.Users;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BedeSlots.Services.Data
{
    public class UserService : IUserService
    {
        private readonly BedeSlotsDbContext context;
        private readonly ITransactionService transactionService;

        public UserService(BedeSlotsDbContext bedeSlotsDbContext, ITransactionService transactionService)
        {
            this.context = bedeSlotsDbContext;
            this.transactionService = transactionService;
        }

        public async Task<decimal> GetUserBalanceById(string userId)
        {
            var user = await this.context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            return user.Balance;
        }

        public async Task<User> GetUserById(string id)
        {
            var user = await this.context.Users.FirstOrDefaultAsync(x => x.Id == id);

            return user;
        }

        public async Task<ICollection<UserDto>> GetAllUsersAsync()
        {
            var users = await this.context
                .Users.Include(u=>u.Transactions)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    FirstName = u.UserName,
                    LastName = u.LastName,
                    Birthdate = u.Birthdate,
                    Balance = u.Balance,
                    Currency = u.Currency
                })
                .ToListAsync();

            return users;
        }

        public async Task<string> GetUserRole(string userId)
        {
            var role = await this.context.UserRoles.FirstOrDefaultAsync(u => u.UserId == userId);
            var roleId = role.RoleId;
            var roleName = this.context.Roles.SingleOrDefault(r => r.Id == roleId).Name;

            return roleName;
        }

        public async Task<IEnumerable<Transaction>> GetUserTransactionsAsync(string id)
        {
            var transactions = await this.context.Transactions.Where(t => t.UserId == id).ToListAsync();

            return transactions;
        }

    }
}
