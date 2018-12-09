using System.Collections.Generic;
using System.Threading.Tasks;
using BedeSlots.Data.Models;

namespace BedeSlots.Services.Data.Contracts
{
    public interface ICurrencyService
    {
        ICollection<Currency> GetAllCurrenciesNames();

        Task<Currency> GetUserCurrencyAsync(string userId);
    }
}