using System.Threading.Tasks;
using BedeSlots.Data;
using BedeSlots.Data.Models;

namespace BedeSlots.Services.Data
{
    public interface IUserService
    {
        BedeSlotsDbContext BedeSlotsDbContext { get; }

        Task<User> GetUserById(string id);
    }
}