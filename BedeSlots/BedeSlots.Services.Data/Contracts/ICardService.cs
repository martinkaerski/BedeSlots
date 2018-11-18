using System.Collections.Generic;
using System.Threading.Tasks;
using BedeSlots.Data.Models;

namespace BedeSlots.Services.Data.Contracts
{
    public interface ICardService
    {
        Task<ICollection<BankCard>> GetUserCardsAsync(string userId);
        Task<ICollection<CardType>> GetCardTypesAsync();
        Task<BankCard> AddCardAsync(BankCard bankCard);
        Task<CardType> GetCardTypeByIdaAsync(int id);
        Task<BankCard> GetCardByIdAsync(int id);
    }
}