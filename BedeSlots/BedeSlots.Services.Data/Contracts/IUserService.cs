using System.Threading.Tasks;
using BedeSlots.Data;
using BedeSlots.Data.Models;

namespace BedeSlots.Services.Data.Contracts
{
    public interface IUserService
    {
        Task<User> GetUserById(string id);
    }
}