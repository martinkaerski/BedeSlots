using System.Threading.Tasks;
using BedeSlots.Data.Models;

namespace BedeSlots.Services.Data.Contracts
{
    public interface ICurrencyConverterService
    {
        Task<double> ConvertToUsd(double amount, CurrencyName currencyName);
    }
}