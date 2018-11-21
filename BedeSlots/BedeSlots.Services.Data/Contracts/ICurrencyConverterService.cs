using System.Threading.Tasks;
using BedeSlots.Data.Models;

namespace BedeSlots.Services.Data.Contracts
{
    public interface ICurrencyConverterService
    {
        Task<decimal> ConvertToUsd(decimal amount, Currency currencyName);

        Task<decimal> ConvertFromUsdToOther(decimal amount, Currency currencyName);
    }
}