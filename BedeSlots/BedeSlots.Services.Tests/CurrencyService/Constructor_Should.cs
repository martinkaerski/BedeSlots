using BedeSlots.Data;
using BedeSlots.Services.Data.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BedeSlots.Services.Tests.CurrencyService
{
    [TestClass]
    public class Constructor_Should
    {
        private readonly ServiceProvider serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

        [TestMethod]
        public void ThrowServiceException_WhenNullContextParameterIsPassed()
        {
            Assert.ThrowsException<ServiceException>(() => new Data.CurrencyService(null));
        }

        [TestMethod]
        public void InitializeCorrectly_WhenValidContextIsPassed()
        {
            var contexOptions = new DbContextOptionsBuilder<BedeSlotsDbContext>()
                 .UseInMemoryDatabase(databaseName: "InitializeCorrectly_WhenValidContextIsPassed")
                 .UseInternalServiceProvider(serviceProvider).Options;

            using (var bedeSlotsContext = new BedeSlotsDbContext(contexOptions))
            {
                var sut = new Data.CurrencyService(bedeSlotsContext);
                Assert.IsInstanceOfType(sut, typeof(Data.CurrencyService));
            }
        }
    }
}
