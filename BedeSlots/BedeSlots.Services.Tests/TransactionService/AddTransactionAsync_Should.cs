using BedeSlots.Data;
using BedeSlots.Data.Models;
using BedeSlots.Services.Data.Contracts;
using BedeSlots.Services.Data.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace BedeSlots.Services.Tests.TransactionService
{
    [TestClass]
    public class AddTransactionAsync_Should
    {
        private readonly ServiceProvider serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

        [TestMethod]
        public async Task AddTransactionCorrectly_WhenValidParamentersArePassed()
        {
            var contexOptions = new DbContextOptionsBuilder<BedeSlotsDbContext>()
    .UseInMemoryDatabase(databaseName: "AddTransactionCorrectly_WhenValidParamentersArePassed")
    .UseInternalServiceProvider(serviceProvider).Options;

            var user = new User();
            Transaction result;

            string description = "test";
            int validAmount = 100;
            var validTransactionType = TransactionType.Deposit;
            var baseCurrency = Currency.USD;

            using (var bedeSlotsContext = new BedeSlotsDbContext(contexOptions))
            {
                var currencyConverterMock = new Mock<ICurrencyConverterService>();
                var sut = new Data.TransactionService(bedeSlotsContext, currencyConverterMock.Object);
                currencyConverterMock.Setup((x) => x.ConvertToBaseCurrency(10, Currency.USD)).ReturnsAsync(10);

                result = await sut.AddTransactionAsync(validTransactionType, user.Id, description, validAmount, baseCurrency);
            }

            using (var bedeSlotsContext = new BedeSlotsDbContext(contexOptions))
            {
                Assert.IsTrue(await bedeSlotsContext.Transactions.CountAsync() == 1);
                Assert.IsTrue(await bedeSlotsContext.Transactions.AnyAsync(x => x.Description == description));
            }
        }
        [TestMethod]
        public async Task ThrowServiceException_WhenNegativeAmountIsPassed()
        {
            var contexOptions = new DbContextOptionsBuilder<BedeSlotsDbContext>()
    .UseInMemoryDatabase(databaseName: "ThrowServiceException_WhenNegativeAmountIsPassed")
    .UseInternalServiceProvider(serviceProvider).Options;

            var user = new User();

            string description = "test";
            int negativeAmount = -100;
            var validTransactionType = TransactionType.Deposit;
            var baseCurrency = Currency.USD;

            using (var bedeSlotsContext = new BedeSlotsDbContext(contexOptions))
            {
                var currencyConverterMock = new Mock<ICurrencyConverterService>();
                var sut = new Data.TransactionService(bedeSlotsContext, currencyConverterMock.Object);

                await Assert.ThrowsExceptionAsync<ServiceException>(async () => await sut.AddTransactionAsync(validTransactionType, user.Id, description, negativeAmount, baseCurrency));
            }
        }
        [TestMethod]
        public async Task ThrowServiceException_WhenUserIdParameterIsNull()
        {
            var contexOptions = new DbContextOptionsBuilder<BedeSlotsDbContext>()
    .UseInMemoryDatabase(databaseName: "ThrowServiceException_WhenUserIdParameterIsNull")
    .UseInternalServiceProvider(serviceProvider).Options;

            string description = "test";
            int validAmount = 100;
            var validTransactionType = TransactionType.Deposit;
            var baseCurrency = Currency.USD;

            using (var bedeSlotsContext = new BedeSlotsDbContext(contexOptions))
            {
                var currencyConverterMock = new Mock<ICurrencyConverterService>();
                var sut = new Data.TransactionService(bedeSlotsContext, currencyConverterMock.Object);

                await Assert.ThrowsExceptionAsync<ServiceException>(async () => await sut.AddTransactionAsync(validTransactionType, null, description, validAmount, baseCurrency));
            }
        }
    }
}
