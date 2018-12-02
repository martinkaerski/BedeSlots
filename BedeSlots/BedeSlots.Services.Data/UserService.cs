using BedeSlots.Data;
using BedeSlots.Data.Models;
using BedeSlots.DTO;
using BedeSlots.Services.Data.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BedeSlots.Services.Data
{
    //TODO: refactoring
    public class UserService : IUserService
    {
        private readonly BedeSlotsDbContext context;
        private readonly ITransactionService transactionService;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<User> userManager;
        private readonly ICurrencyConverterService currencyConverterService;

        public UserService(BedeSlotsDbContext bedeSlotsDbContext, 
            ITransactionService transactionService, 
            RoleManager<IdentityRole> roleManager, 
            UserManager<User> userManager,
            ICurrencyConverterService currencyConverterService)
        {
            this.context = bedeSlotsDbContext;
            this.transactionService = transactionService;
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.currencyConverterService = currencyConverterService;
        }

        public async Task<decimal> GetUserBalanceByIdAsync(string userId)
        {
            var user = await this.context
                .Users
                .Select(u => new
                {
                    Id = u.Id,
                    Balance = u.Balance,
                    Currency = u.Currency
                })
                .FirstOrDefaultAsync(u => u.Id == userId);

            var convertedBalance = await this.currencyConverterService.ConvertFromBaseToOther(user.Balance, user.Currency);

            return convertedBalance;
        }

        public User GetUserById(string id)
        {
            var user = this.context.Users.FirstOrDefault(x => x.Id == id);

            return user;
        }

        public IQueryable<UserDto> GetAllUsers()
        {
            var users = this.context
                .Users
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Username = u.UserName,
                    Firstname = u.FirstName,
                    Lastname = u.LastName,
                    Email = u.Email,
                    Birthdate = u.Birthdate,
                    Balance = u.Balance,
                    Currency = u.Currency
                })
                .AsQueryable();

            return users;
        }

        public async Task<string> GetUserRoleIdAsync(string userId)
        {
            var role = await this.context.UserRoles.FirstOrDefaultAsync(u => u.UserId == userId);
            var roleId = role.RoleId;
            var roleName = this.context.Roles.SingleOrDefault(r => r.Id == roleId).Name;

            return roleId;
        }

        public async Task<IdentityUserRole<string>> GetUserRoleAsync(string userId)
        {
            var role = await this.context.UserRoles.FirstOrDefaultAsync(u => u.UserId == userId);
            var roleId = role.RoleId;
            var roleName = this.context.Roles.SingleOrDefault(r => r.Id == roleId).Name;

            return role;
        }

        public string GetUserRoleName(string userId)
        {
            var role = this.context.UserRoles.FirstOrDefault(u => u.UserId == userId);
            var roleId = role.RoleId;
            var roleName = this.context.Roles.SingleOrDefault(r => r.Id == roleId).Name;

            return roleName;
        }

        public async Task<IEnumerable<Transaction>> GetUserTransactionsAsync(string id)
        {
            var transactions = await this.context.Transactions.Where(t => t.UserId == id).ToListAsync();

            return transactions;
        }

        public async Task<ICollection<IdentityRole>> GetAllRolesAsync()
        {
            var roles = await this.context.Roles.Where(r => r.Name != "MasterAdmin").ToListAsync();
            return roles;
        }

        public async Task<IdentityRole> EditUserRoleAsync(string userId, string newRoleId)
        {
            var userRole = this.context.UserRoles.FirstOrDefault(ur => ur.UserId == userId);
            this.context.UserRoles.Remove(userRole);

            var newRole = await this.context.Roles.FirstOrDefaultAsync(r => r.Id == newRoleId);
            var user = GetUserById(userId);

            if (newRole == null || user == null)
            {
                //TODO: do smth
            }

            await this.userManager.AddToRoleAsync(user, newRole.Name);

            return newRole;
        }
    }
}
