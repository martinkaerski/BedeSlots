﻿using BedeSlots.Data.Models;
using BedeSlots.Services.Data.Contracts;
using BedeSlots.Web.Controllers;
using BedeSlots.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BedeSlots.Web.Tests.Controllers.WithdrawControllerTests
{
    [TestClass]
    public class Index_Should
    {
        [TestMethod]
        public async Task ReturnsViewResult_WhenCalled()
        {
            // Arrange
            var cardServiceMock = new Mock<ICardService>();
            var userBalServiceMock = new Mock<IUserBalanceService>();
            var transactionServiceMock = new Mock<ITransactionService>();
            var currencyServiceMock = new Mock<ICurrencyService>();
            var userManagerMock = SetupUserManagerMock();

            var controller = SetupController(cardServiceMock, userBalServiceMock, transactionServiceMock, userManagerMock, currencyServiceMock);

            var model = new WithdrawViewModel()
            {
                Currency = Currency.BGN
            };

            var user = controller.User;
            var appUser = new User()
            {
                UserName = "user"
            };

            userManagerMock
                .Setup(u => u.GetUserAsync(user))
                .ReturnsAsync(appUser);

            // Act
            var result = await controller.Index();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        private WithdrawController SetupController(Mock<ICardService> cardServiceMock,
            Mock<IUserBalanceService> userBalanceServiceMock,
            Mock<ITransactionService> transactionerviceMock,
            Mock<UserManager<User>> userManagerMock,
            Mock<ICurrencyService> curencyServiceMock)
        {
            var controller = new WithdrawController(
                userBalanceServiceMock.Object,
                transactionerviceMock.Object,
                cardServiceMock.Object,
                curencyServiceMock.Object,
                userManagerMock.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        User = new ClaimsPrincipal()
                    }
                },
                TempData = new Mock<ITempDataDictionary>().Object
            }; ;

            return controller;
        }

        private Mock<UserManager<User>> SetupUserManagerMock()
        {
            return new Mock<UserManager<User>>(
                  new Mock<IUserStore<User>>().Object,
                  null,
                  null,
                  null,
                  null,
                  null,
                  null,
                  null,
                  null);
        }
    }
}
