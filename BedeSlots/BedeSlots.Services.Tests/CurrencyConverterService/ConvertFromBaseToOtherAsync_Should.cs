using BedeSlots.Data.Models;
using BedeSlots.Services.Data.Contracts;
using BedeSlots.Services.Data.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BedeSlots.Services.Tests.CurrencyConverterService
{
    [TestClass]
    public class ConvertFromBaseToOtherAsync_Should
    {
        [TestMethod]
        public async Task ReturnAmountConvertedFromBaseToOther_WhenValidParametersArePassed()
        {
            decimal inputVal = 10;
            decimal eracsMockReturnVal = 5;
            decimal expectedVal = inputVal * eracsMockReturnVal;

            var eracsMock = new Mock<IExchangeRateApiService>();
            eracsMock.Setup(e => e.GetRateAsync(It.IsAny<Currency>())).ReturnsAsync(eracsMockReturnVal);

            var sut = new Data.CurrencyConverterService(eracsMock.Object);

            var result = await sut.ConvertFromBaseToOtherAsync(inputVal, Currency.USD);

            Assert.IsTrue(result == expectedVal);
        }

        [TestMethod]
        public async Task ThrowServiceException_WhenNegativeAmountIsPassed()
        {
            decimal negativeNumber = -44;

            var eracsMock = new Mock<IExchangeRateApiService>();

            var sut = new Data.CurrencyConverterService(eracsMock.Object);

            await Assert.ThrowsExceptionAsync<ServiceException>(async () => await sut.ConvertFromBaseToOtherAsync(negativeNumber, Currency.BGN));
        }
    }
}
