using BedeSlots.Data;
using BedeSlots.Data.Models;
using BedeSlots.DTO;
using BedeSlots.Services.Data.Contracts;
using BedeSlots.Services.Data.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BedeSlots.Services.Data
{
    public class UserService : IUserService
    {
        private readonly BedeSlotsDbContext context;
        private readonly ITransactionService transactionService;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<User> userManager;

        public UserService(BedeSlotsDbContext bedeSlotsDbContext, ITransactionService transactionService,
            RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            this.context = bedeSlotsDbContext;
            this.transactionService = transactionService;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            var user = await this.context.Users
                .Where(u => u.IsDeleted == false)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                throw new ServiceException($"There is no user with ID {id}");
            }

            return user;
        }

        public IQueryable<UserDto> GetAllUsers()
        {
            var usersRoles = context.UserRoles.Where(x => x.RoleId != null);

            var allRoles = context.Roles.ToList();
            var roleDictionary = new Dictionary<string, string>();

            foreach (var role in usersRoles)
            {
                roleDictionary.Add(role.UserId, allRoles.FirstOrDefault(x => x.Id == role.RoleId).Name);
            }

            var users = this.context
                .Users
                .Where(u => u.IsDeleted == false)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Username = u.UserName,
                    Firstname = u.FirstName,
                    Lastname = u.LastName,
                    Email = u.Email,
                    Birthdate = u.Birthdate,
                    Balance = u.Balance,
                    Currency = u.Currency,
                    Role = roleDictionary[u.Id]
                })
                .AsQueryable();

            return users;
        }

        public async Task<string> GetUserRoleIdAsync(string userId)
        {
            var role = await this.context.UserRoles.FirstOrDefaultAsync(u => u.UserId == userId);
            var roleId = role.RoleId;

            return roleId;
        }

        public async Task<IdentityUserRole<string>> GetUserRoleAsync(string userId)
        {
            var role = await this.context.UserRoles.FirstOrDefaultAsync(u => u.UserId == userId);

            return role;
        }

        public async Task<string> GetUserRoleNameAsync(string userId)
        {
            var role = await this.context.UserRoles.FirstOrDefaultAsync(u => u.UserId == userId);
            var roleId = role.RoleId;
            var roleName = this.context.Roles.SingleOrDefault(r => r.Id == roleId).Name;

            return roleName;
        }

        public async Task<ICollection<IdentityRole>> GetAllRolesAsync()
        {
            var roles = await this.context.Roles.ToListAsync();

            return roles;
        }

        public async Task<IdentityRole> EditUserRoleAsync(string userId, string newRoleId)
        {
            var userRole = this.context.UserRoles.FirstOrDefault(ur => ur.UserId == userId);
            this.context.UserRoles.Remove(userRole);

            var newRole = await this.context.Roles.FirstOrDefaultAsync(r => r.Id == newRoleId);
            var user = await GetUserByIdAsync(userId);

            if (newRole == null || user == null)
            {
                //TODO: do smth
            }

            await this.userManager.AddToRoleAsync(user, newRole.Name);

            return newRole;
        }

        public async Task<User> DeleteUserAsync(string userId)
        {
            var user = await this.GetUserByIdAsync(userId);
            user.IsDeleted = true;
            user.LockoutEnabled = true;
            await this.context.SaveChangesAsync();

            return user;
        }

        public async Task<Currency> GetUserCurrencyByIdAsync(string userId)
        {
            var user = await this.context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            return user.Currency;
        }
    }
}
