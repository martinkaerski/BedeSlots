using BedeSlots.Data.Models;
using BedeSlots.DTO.ExchangeRatesDto;
using BedeSlots.Services.Data;
using BedeSlots.Services.External.Contracts;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BedeSlots.Services.Tests.ExchangeRatesApiService
{
    [Ignore]
    public class GetAllRatesAsync_Should
    {
        [TestMethod]
        public async Task ReturnRates_WhenInvoked()
        {
            // Arrange
            var exchangeRateApiCallerMock = new Mock<IExchangeRatesApiCaller>();

            string rates = "{\"date\":\"2018-12-14\",\"rates\":{\"USD\":1.0,\"EUR\":0.8861320337,\"BGN\":1.7330970315,\"GBP\":0.7960567125},\"base\":\"USD\"}";
                      
            exchangeRateApiCallerMock
                .Setup(e => e.GetCurrenciesRatesAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(rates);

            var deserializedRates = JsonConvert.DeserializeObject<CurrencyDto>(rates);

            var mockMemoryCache = new Mock<IMemoryCache>();
            //mockMemoryCache
            //    .Setup(x => x.TryGetValue(It.IsAny<object>(), out deserializedRates))
            //    .Returns(true);

            var sut = new ExchangeRateApiService(exchangeRateApiCallerMock.Object, mockMemoryCache.Object );

            // Act
            var result = await sut.GetAllRatesAsync();
            Assert.IsInstanceOfType(result, typeof(IDictionary<Currency, decimal>));
        }
    }
}
