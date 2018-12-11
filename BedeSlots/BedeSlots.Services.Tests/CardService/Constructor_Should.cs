using BedeSlots.Data;
using BedeSlots.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace BedeSlots.Services.Tests.CardService
{
    [TestClass]
    public class Constructor_Should
    {
        [TestMethod]
        public void ThrowArgumentNullException_WhenNullContextIsPassed()
        {
            //arrange
            var userStoreMock = new Mock<IUserStore<User>>();
            var userManager = new UserManager<User>(userStoreMock.Object, null, null, null, null, null, null, null, null);
            //act
            //assert
            Assert.ThrowsException<ArgumentNullException>(() => new Data.CardService(null, userManager));
        }
        [TestMethod]
        public void ThrowArgumentNullException_WhenNullUserManagerIsPassed()
        {
            //arrange 
            var bedeSlotsContext = new BedeSlotsDbContext(new Microsoft.EntityFrameworkCore.DbContextOptions<BedeSlotsDbContext>());
            //act
            //assert
            Assert.ThrowsException<ArgumentNullException>(() => new Data.CardService(bedeSlotsContext, null));
        }
        [TestMethod]
        public void NotThrowException_WhenValidParametersArePassed()
        {
            ////arrange 
            var userStoreMock = new Mock<IUserStore<User>>();
            var userManager = new UserManager<User>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            var bedeSlotsContext = new BedeSlotsDbContext(new Microsoft.EntityFrameworkCore.DbContextOptions<BedeSlotsDbContext>());
            //act
            //assert
            var sut = new Data.CardService(bedeSlotsContext, userManager);
        }
    }
}
