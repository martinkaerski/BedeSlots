using BedeSlots.Data;
using BedeSlots.Data.Models;
using BedeSlots.Services.Data.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BedeSlots.Services.Data
{
    public class CardService : ICardService
    {
        private readonly BedeSlotsDbContext context;

        public CardService(BedeSlotsDbContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<ICollection<BankCard>> GetUserCardsAsync(string userId)
        {
            var cards = await context.BankCards
                .Where(c => c.UserId == userId)
                .ToListAsync();

            return cards;
        }

        //public async Task<ICollection<CardType>> GetCardTypesAsync()
        //{
        //    var types = await this.context.CardTypes.Where(x => x.IsDeleted == false).ToListAsync();
        //    return types;
        //}

        public async Task<BankCard> AddCardAsync(BankCard bankCard)
        {
            await this.context.BankCards.AddAsync(bankCard);
            await this.context.SaveChangesAsync();

            return bankCard;
        }

        //public async Task<CardType> GetCardTypeByIdaAsync(int id)
        //{
        //    var type = await this.context.CardTypes.FirstOrDefaultAsync(t => t.Id == id);

        //    return type;
        //}

        public async Task<BankCard> GetCardByIdAsync(int id)
        {
            var card = await this.context.BankCards
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == id);

            return card;
        }
    }
}
