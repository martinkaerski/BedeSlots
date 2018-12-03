﻿using BedeSlots.Data;
using BedeSlots.Data.Models;
using BedeSlots.DTO;
using BedeSlots.Services.Data.Contracts;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<User> userManager;
        
        public CardService(BedeSlotsDbContext context, UserManager<User> userManager)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.userManager = userManager;
        }

        public async Task<ICollection<BankCard>> GetUserCardsAsync(string userId)
        {
            var cards = await context.BankCards
                .Where(c => c.UserId == userId)
                .ToListAsync();

            return cards;
        }

        public async Task<ICollection<CardDto>> GetUserCardsNumbersAsync(string userId)
        {
            var cardsNumbers = await context.BankCards
                .Where(c => c.UserId == userId)
                .Select(c => new CardDto
                {
                    Id = c.Id,
                    CardNumberLastDigits = c.Number.Substring(12)
                })
                .ToListAsync();

            return cardsNumbers;
        }

        public async Task<BankCard> AddCardAsync(BankCard bankCard)
        {
            await this.context.BankCards.AddAsync(bankCard);
            await this.context.SaveChangesAsync();

            return bankCard;
        }

        public async Task<BankCard> GetCardByIdAsync(int id)
        {
            var card = await this.context.BankCards
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == id);

            return card;
        }

        public bool CardExists(int bankCardId)
        {
            if (this.context.BankCards.Any(c => c.Id == bankCardId))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
