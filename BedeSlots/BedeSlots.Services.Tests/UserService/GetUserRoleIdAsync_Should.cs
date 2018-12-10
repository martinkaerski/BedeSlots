using BedeSlots.Data;
using BedeSlots.Data.Models;
using BedeSlots.Services.Data.Contracts;
using BedeSlots.Services.Data.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace BedeSlots.Services.Tests.UserService
{
    [TestClass]
    public class GetUserRoleIdAsync_Should
    {
        private ServiceProvider serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

        [TestMethod]
        public async Task ReturnRoleId_WhenValidUserIdIsPassed()
        {

            var userStoreMock = new Mock<IUserStore<User>>();
            var userManager = new UserManager<User>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            var contexOptions = new DbContextOptionsBuilder<BedeSlotsDbContext>()
     .UseInMemoryDatabase(databaseName: "ReturnRoleId_WhenValidUserIdIsPassed")
     .UseInternalServiceProvider(serviceProvider).Options;

            var transactionServiceMock = new Mock<ITransactionService>();

            var user = new User();
            var role = new IdentityRole("User");
            IdentityUserRole<string> userRole;

            using (var bedeSlotsContext = new BedeSlotsDbContext(contexOptions))
            {
                bedeSlotsContext.Roles.Add(role);
                bedeSlotsContext.Users.Add(user);

                userRole = new IdentityUserRole<string>() { UserId = user.Id, RoleId = role.Id };

                bedeSlotsContext.UserRoles.Add(userRole);
                bedeSlotsContext.SaveChanges();
            }

            using (var bedeSlotsContext = new BedeSlotsDbContext(contexOptions))
            {
                var sut = new Data.UserService(bedeSlotsContext,
                    userManager);

                var result = await sut.GetUserRoleIdAsync(user.Id);
                Assert.IsTrue(result == role.Id);
            }
        }

        [TestMethod]
        public async Task ThrowServiceException_WhenNullParameterIsPassed()
        {

            var userStoreMock = new Mock<IUserStore<User>>();
            var userManager = new UserManager<User>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            var contexOptions = new DbContextOptionsBuilder<BedeSlotsDbContext>()
     .UseInMemoryDatabase(databaseName: "ReturnRoleId_WhenValidUserIdIsPassed")
     .UseInternalServiceProvider(serviceProvider).Options;

            var transactionServiceMock = new Mock<ITransactionService>();
            
            using (var bedeSlotsContext = new BedeSlotsDbContext(contexOptions))
            {
                var sut = new Data.UserService(bedeSlotsContext,
                    userManager);

                await Assert.ThrowsExceptionAsync<ServiceException>(async () => await sut.GetUserRoleIdAsync(null));

            }
        }


    }
}