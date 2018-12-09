﻿using BedeSlots.Data;
using BedeSlots.Data.Models;
using BedeSlots.DTO;
using BedeSlots.DTO.BankCardDto;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BedeSlots.Services.Tests.CardService
{
    [TestClass]
    public class GetUserCardsLastNumbersAsync_Should
    {
        [TestMethod]
        public async Task ThrowArgumentNullException_WhenUserIdIsNull()
        {
            var userStoreMock = new Mock<IUserStore<User>>();
            var userManager = new UserManager<User>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            Data.CardService cardService;

            var contexOptions = new DbContextOptionsBuilder<BedeSlotsDbContext>()
     .UseInMemoryDatabase(databaseName: "ThrowArgumentNullException_WhenUserIdIsNull").Options;

            using (var bedeSlotsContext = new BedeSlotsDbContext(contexOptions))
            {
                cardService = new Data.CardService(bedeSlotsContext, userManager);
                await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await cardService.GetUserCardsLastNumbersAsync(null));
            }
        }

        [TestMethod]
        public async Task ThrowArgumentException_WhenNotExistingUserIdIsPassed()
        {
            var userStoreMock = new Mock<IUserStore<User>>();
            var userManager = new UserManager<User>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            Data.CardService cardService;

            var contexOptions = new DbContextOptionsBuilder<BedeSlotsDbContext>()
     .UseInMemoryDatabase(databaseName: "ThrowArgumentException_WhenNotExistingUserIdIsPassed").Options;

            using (var bedeSlotsContext = new BedeSlotsDbContext(contexOptions))
            {
                cardService = new Data.CardService(bedeSlotsContext, userManager);
                await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await cardService.GetUserCardsLastNumbersAsync("not existing user id"));
            }
        }

        [TestMethod]
        public async Task ReturnCollectionOfCardNumbersLastFourDigitsInDatabase_WhenValidUserIdIsPassed()
        {
            var userStoreMock = new Mock<IUserStore<User>>();
            var userManager = new UserManager<User>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            ICollection<CardDto> cards;
            Data.CardService cardService;

            var user = new User();
            var card = new BankCard()
            {
                UserId = user.Id,
                CvvNumber = "123",
                Number = "1616161616161616",
                Type = CardType.Visa
            };

            var contexOptions = new DbContextOptionsBuilder<BedeSlotsDbContext>()
     .UseInMemoryDatabase(databaseName: "ReturnCollectionOfCardNumbersLastFourDigitsInDatabase_WhenValidUserIdIsPassed").Options;

            using (var bedeSlotsContext = new BedeSlotsDbContext(contexOptions))
            {
                bedeSlotsContext.BankCards.Add(card);
                bedeSlotsContext.Users.Add(user);
                bedeSlotsContext.SaveChanges();

                cardService = new Data.CardService(bedeSlotsContext, userManager);
                cards = await cardService.GetUserCardsLastNumbersAsync(user.Id);
            }
            using (var bedeSlotsContext = new BedeSlotsDbContext(contexOptions))
            {
                Assert.IsTrue(cards.Count == 1);
                Assert.IsInstanceOfType(cards, typeof(ICollection<CardDto>));
                Assert.IsTrue(cards.First().Number == card.Number.Substring(12));
            }
        }
    }
}