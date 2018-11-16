using BedeSlots.Data;
using BedeSlots.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BedeSlots.Services.Data
{
    public class UserService : IUserService
    {
        public BedeSlotsDbContext BedeSlotsDbContext { get; }

        public UserService(BedeSlotsDbContext bedeSlotsDbContext)
        {
            BedeSlotsDbContext = bedeSlotsDbContext;
        }

        public async Task<User> GetUserById(string id)
        {
            var user = await this.BedeSlotsDbContext.Users.FirstOrDefaultAsync(x => x.Id == id);

            return user;
        }

    }
}
