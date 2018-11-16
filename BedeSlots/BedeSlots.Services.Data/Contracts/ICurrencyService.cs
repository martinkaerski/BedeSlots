using System.Collections.Generic;
using System.Threading.Tasks;
using BedeSlots.Data.Models;

namespace BedeSlots.Services.Data.Contracts
{
    public interface ICurrencyService
    {
        Task<ICollection<Currency>> GetAllCurrenciesAsync();
    }
}