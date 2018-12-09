using BedeSlots.Data;
using BedeSlots.Data.Models;
using BedeSlots.DTO.BankCardDto;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;

namespace BedeSlots.Services.Tests.CardService
{
    [TestClass]
    public class GetUserCardsAsync_Should
    {
        [TestMethod]
        public async Task ThrowArgumentNullException_WhenNullParameterIsPassed()
        {
            var userStoreMock = new Mock<IUserStore<User>>();
            var userManager = new UserManager<User>(userStoreMock.Object, null, null, null, null, null, null, null, null);
            var contexOptions = new DbContextOptionsBuilder<BedeSlotsDbContext>()
     .UseInMemoryDatabase(databaseName: "ThrowArgumentNullException_WhenNullParameterIsPassed").Options;

            using (var bedeSlotsContext = new BedeSlotsDbContext(contexOptions))
            {
                var cardService = new Data.CardService(bedeSlotsContext, userManager);
                await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await cardService.GetUserCardsAsync(null));
            }
        }
        [TestMethod]
        public async Task ThrowArgumentException_WhenUnexistingUserIdIsPassed()
        {
            var userStoreMock = new Mock<IUserStore<User>>();
            var userManager = new UserManager<User>(userStoreMock.Object, null, null, null, null, null, null, null, null);
            var contexOptions = new DbContextOptionsBuilder<BedeSlotsDbContext>()
     .UseInMemoryDatabase(databaseName: "ThrowArgumentException_WhenUnexistingUserIdIsPassed").Options;

            string notExistingId = "not existing id";

            using (var bedeSlotsContext = new BedeSlotsDbContext(contexOptions))
            {
                //    bedeSlotsContext.Users.Add()
                var cardService = new Data.CardService(bedeSlotsContext, userManager);
                await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
                await cardService.GetUserCardsAsync(notExistingId));
            }
        }
        [TestMethod]
        public async Task ReturnUserCards_WhenExistingUserIdIsPassed()
        {
            var userStoreMock = new Mock<IUserStore<User>>();
            var userManager = new UserManager<User>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            var contexOptions = new DbContextOptionsBuilder<BedeSlotsDbContext>()
     .UseInMemoryDatabase(databaseName: "ReturnUserCards_WhenExistingUserIdIsPassed").Options;

            var user = new User();
            var card = new BankCard()
            {
                UserId = user.Id,
                User = user,
                CvvNumber = "123",
                Number = "1616161616161616",
                Type = CardType.Visa,
                CreatedOn = DateTime.Now
            };

            ICollection<CardDetailsDto> userCards;
            Data.CardService cardService;

            using (var bedeSlotsContext = new BedeSlotsDbContext(contexOptions))
            {
                bedeSlotsContext.Users.Add(user);
                bedeSlotsContext.BankCards.Add(card);
                bedeSlotsContext.SaveChanges();
            }

            using (var bedeSlotsContext = new BedeSlotsDbContext(contexOptions))
            {
                cardService = new Data.CardService(bedeSlotsContext, userManager);
                userCards = await cardService.GetUserCardsAsync(user.Id);
                Assert.IsTrue(userCards.Count == 1);
            }
        }
    }
}
