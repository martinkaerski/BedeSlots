using Microsoft.VisualStudio.TestTools.UnitTesting;
using BedeSlots.Data;
using BedeSlots.Data.Models;
using BedeSlots.Services.Data.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Threading.Tasks;
using BedeSlots.Services.Data.Exceptions;

namespace BedeSlots.Services.Tests.TransactionService
{
    [TestClass]
    public class GetTransactionByIdAsync_Should
    {
        private readonly ServiceProvider serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

        [TestMethod]
        public async Task ReturnTransaction_WhenValidParametersArePassed()
        {
            var contextOptions = new DbContextOptionsBuilder<BedeSlotsDbContext>()
.UseInMemoryDatabase(databaseName: "ReturnTransaction_WhenValidParametersArePassed")
.UseInternalServiceProvider(serviceProvider).Options;

            var transaction = new Transaction() { User = new User() };
            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                bedeSlotsContext.Transactions.Add(transaction);
                bedeSlotsContext.SaveChanges();
            }
            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                var currencyConverterMock = new Mock<ICurrencyConverterService>();
                var sut = new Data.TransactionService(bedeSlotsContext, currencyConverterMock.Object);
                var result = await sut.GetTransactionByIdAsync(transaction.Id);

                Assert.IsTrue(result.Id == transaction.Id);
            }
        }

        [TestMethod]
        public async Task ThrowServiceException_WhenNotExistingIdIsPassed()
        {

            var contextOptions = new DbContextOptionsBuilder<BedeSlotsDbContext>()
    .UseInMemoryDatabase(databaseName: "ThrowServiceException_WhenNotExistingIdIsPassed")
    .UseInternalServiceProvider(serviceProvider).Options;

            int notExistingId = 99;

            var transaction = new Transaction() { User = new User() };

            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                bedeSlotsContext.Transactions.Add(transaction);
                bedeSlotsContext.SaveChanges();
            }

            using (var bedeSlotsContext = new BedeSlotsDbContext(contextOptions))
            {
                var currencyConverterMock = new Mock<ICurrencyConverterService>();
                var sut = new Data.TransactionService(bedeSlotsContext, currencyConverterMock.Object);

                await Assert.ThrowsExceptionAsync<ServiceException>(async () => await sut.GetTransactionByIdAsync(notExistingId));
            }
        }
    }
}
