using BedeSlots.Data.Models;
using BedeSlots.DTO;
using BedeSlots.DTO.BankCardDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BedeSlots.Services.Data.Contracts
{
    public interface ICardService
    {
        Task<ICollection<CardDetailsDto>> GetUserCardsAsync(string userId);

        Task<ICollection<CardDto>> GetUserCardsLastNumbersAsync(string userId);

        Task<ICollection<CardDto>> GetUserCardsAllNumbersAsync(string userId);

        Task<BankCard> AddCardAsync(BankCard bankCard);

        Task<BankCard> GetCardByIdAsync(int id);

        bool CardExists(int bankCardId);

        Task<BankCard> DeleteCardAsync(int cardId);
    }
}