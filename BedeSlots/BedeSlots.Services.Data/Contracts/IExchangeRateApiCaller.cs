using System.Threading.Tasks;

namespace BedeSlots.Services.Data.Contracts
{
    public interface IExchangeRateApiCaller
    {
        Task<bool> GetCurrenciesRates();
    }
}