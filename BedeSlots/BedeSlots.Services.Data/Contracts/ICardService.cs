using BedeSlots.Data.Models;
using BedeSlots.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BedeSlots.Services.Data.Contracts
{
    public interface ICardService
    {
        Task<ICollection<BankCard>> GetUserCardsAsync(string userId);

        Task<ICollection<CardDto>> GetUserCardsNumbersAsync(string userId);

        Task<BankCard> AddCardAsync(BankCard bankCard);

        Task<BankCard> GetCardByIdAsync(int id);

        bool CardExists(int bankCardId);
    }
}