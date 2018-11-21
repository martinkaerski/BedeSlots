using BedeSlots.Data;
using BedeSlots.Data.Models;
using BedeSlots.Services.Data.Contracts;
using Microsoft.EntityFrameworkCore;
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

    }
}
