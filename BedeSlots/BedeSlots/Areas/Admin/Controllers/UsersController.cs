using BedeSlots.Data.Models;
using BedeSlots.Services.Data.Contracts;
using BedeSlots.Web.Areas.Admin.Models;
using BedeSlots.Web.Areas.Admin.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BedeSlots.Web.Areas.Admin.Controllers
{
    [Area(WebConstants.AdminArea)]
    [Authorize(Roles = WebConstants.AdminRole + "," + WebConstants.MasterAdminRole)]
    public class UsersController : Controller
    {
        private readonly IUserService userService;
        private readonly ITransactionService transactionService;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UsersController(
            IUserService userService,
            UserManager<User> userManager,
            ITransactionService transactionService,
            RoleManager<IdentityRole> roleManager)
        {
            this.userService = userService;
            this.transactionService = transactionService;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = await this.userService.GetAllUsersAsync();

            var usersViewModelsTask = users
                .Select(async u => new UserViewModel
                {
                    Id = u.Id,
                    Username = u.Username,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Birthdate = u.Birthdate,
                    Balance = u.Balance,
                    Currency = u.Currency,
                    Email = u.Email,
                    Role = await userService.GetUserRole(u.Id)
                });

            var usersViewModels = await Task.WhenAll(usersViewModelsTask);

            var roles = await this.roleManager
                .Roles
                .Where(r => r.Name != WebConstants.MasterAdminRole)
                .Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Name
                })
                .ToListAsync();

            var userViewModel = new UserListingViewModel
            {                
                Users = usersViewModels,
                Roles = roles
            };
                       
            return View(userViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddToRole(AddUserToRoleViewModel model)
        {
            var roleExists = await this.roleManager.RoleExistsAsync(model.Role);
            var user = await this.userManager.FindByIdAsync(model.UserId);
            var userExists = user != null;

            if (!roleExists || !userExists)
            {
                this.ModelState.AddModelError(string.Empty, "Invalid identity details.");
            }

            if (!ModelState.IsValid)
            {
                return this.RedirectToAction(nameof(Index));
            }

            await this.userManager.AddToRoleAsync(user, model.Role);

            return this.RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> GetRole(string data)
        {
            var roleName = await userService.GetUserRole(data);

            return Content(roleName);
        }
    }
}