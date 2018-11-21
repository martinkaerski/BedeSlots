using System.Collections.Generic;
using System.Threading.Tasks;
using BedeSlots.Data.Models;

namespace BedeSlots.Services.Data.Contracts
{
    public interface IExchangeRateApiCallService
    {
        Task<IDictionary<CurrencyName, double>> GetAllRatesAsync();
        Task<double> GetRateAsync(CurrencyName currencyName);
    }
}