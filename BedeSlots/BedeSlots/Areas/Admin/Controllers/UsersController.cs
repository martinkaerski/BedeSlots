using BedeSlots.Data.Models;
using BedeSlots.DTO;
using BedeSlots.Services.Data.Contracts;
using BedeSlots.Web.Areas.Admin.Models;
using BedeSlots.Web.Areas.Admin.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BedeSlots.Web.Areas.Admin.Controllers
{
    //TODO: refactoring

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

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(EditRoleViewModel model)
        {
            var role = model.RoleId;
            var user = await this.userManager.FindByIdAsync(model.UserId);
            var userRoles = await this.userManager.GetRolesAsync(user);

            if (userRoles.Contains(WebConstants.MasterAdminRole))
            {
                return View();
            }

            await this.userService.EditUserRoleAsync(model.UserId, model.RoleId);
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string userid)
        {
            var user = await this.userManager.FindByIdAsync(userid);
            var userRoles = await this.userManager.GetRolesAsync(user);

            if (userRoles.Contains(WebConstants.MasterAdminRole))
            {
                return RedirectToAction("Index");
            }

            var roleId = await userService.GetUserRoleIdAsync(userid);

            var roles = await userService.GetAllRolesAsync();
            var rolesSelectList = roles.Select(r => new SelectListItem { Value = r.Id, Text = r.Name }).ToList();
            var model = new EditRoleViewModel()
            {
                UserId = userid,
                RoleId = roleId,
                UserName =  user.FirstName + " " + user.LastName,
                Roles = rolesSelectList
            };

            return PartialView("_EditRolePartial", model);
        }

        [HttpPost]
        public IActionResult LoadData()
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                // Skiping number of Rows count
                var start = Request.Form["start"].FirstOrDefault();
                // Paging Length 10,20
                var length = Request.Form["length"].FirstOrDefault();
                // Sort Column Name
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                // Sort Column Direction ( asc ,desc)
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                // Search Value from (Search box)
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                //Paging Size (10,20,50,100)
                int pageSize = length != null ? int.Parse(length) : 0;
                int skip = start != null ? int.Parse(start) : 0;
                int recordsTotal = 0;

                var users = this.userService.GetAllUsers();

                //Sorting
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    if (sortColumnDirection == "asc")
                    {
                        users = users
                            .OrderBy(u => u.GetType().GetProperty(sortColumn).GetValue(u));
                    }
                    else
                    {
                        users = users
                            .OrderByDescending(u => u.GetType().GetProperty(sortColumn).GetValue(u));
                    }
                }

                //Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    users = users
                        .Where(u => u.Firstname.Contains(searchValue)
                        || u.Lastname.Contains(searchValue)
                        || u.Username.Contains(searchValue)
                        || u.Email.Contains(searchValue));
                }

                //Total number of rows count 
                recordsTotal = users.Count();

                //Paging 
                var data = users
                    .Skip(skip)
                    .Take(pageSize)
                    .Select( u =>  new
                    {
                        Userid = u.Id,
                        Username = u.Username,
                        Firstname = u.Firstname,
                        Lastname = u.Lastname,
                        Email = u.Email,
                        Balance = u.Balance,
                        Currency = u.Currency.ToString(),
                        Role = "User"
                    })
                    .ToList();

                //Returning Json Data
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}