using BedeSlots.Data;
using BedeSlots.Data.Models;
using BedeSlots.Services.Data.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BedeSlots.Services.Tests.UserService
{
    [TestClass]
    public class EditUserRoleAsync_Should
    {
        private ServiceProvider serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();
       
        [TestMethod]
        public async Task SuccessfullyChangeUserCurrentRole_WhenValidParametersArePassed()
        {

            var userStoreMock = new Mock<IUserStore<User>>();
            var userManager = new UserManager<User>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            var contexOptions = new DbContextOptionsBuilder<BedeSlotsDbContext>()
     .UseInMemoryDatabase(databaseName: "SuccessfullyChangeUserCurrentRole_WhenValidParametersArePassed")
     .UseInternalServiceProvider(serviceProvider).Options;

            var transactionServiceMock = new Mock<ITransactionService>();

            var user = new User();
            var role = new IdentityRole("User");
            var newRole = new IdentityRole("newRole");
            IdentityUserRole<string> userRole;

            using (var bedeSlotsContext = new BedeSlotsDbContext(contexOptions))
            {
                bedeSlotsContext.Roles.Add(role);
                bedeSlotsContext.Roles.Add(newRole);
                bedeSlotsContext.Users.Add(user);

                userRole = new IdentityUserRole<string>() { UserId = user.Id, RoleId = role.Id };

                bedeSlotsContext.UserRoles.Add(userRole);
                bedeSlotsContext.SaveChanges();

                var sut = new Data.UserService(bedeSlotsContext,
                                               userManager);
                // TODO need help from trainers !! .AddToRoleAsync throws NotSupportedException!
                var result = await sut.EditUserRoleAsync(user.Id, newRole.Id);
             
                Assert.IsTrue(result.Name == newRole.Name);
            }
        }
    }
}
