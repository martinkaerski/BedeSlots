using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BedeSlots.Services.Data.Contracts;
using BedeSlots.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace BedeSlots.Web.Controllers
{
    public class RetrieveController : Controller
    {
        private readonly IUserBalanceService userBalanceService;
        private readonly IUserService userService;

        public RetrieveController(IUserBalanceService userBalanceService, IUserService userService)
        {
            this.userBalanceService = userBalanceService;
            this.userService = userService;
        }

        public async Task<IActionResult> Retrieve(RetrieveViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json("Invalid parameters passed!");
            }
            var userId = HttpContext.User.Claims.FirstOrDefault().Value;


            if (userId != null)
            {
                try
                {
                    // simulate transfer between this application and current user's bank account
                    await this.userBalanceService.RetrieveMoneyAsync(model.RetrieveAmount, userId);

                }
                catch (Exception ex)
                {
                    return Json(new { message = "Can't retrieve this amount of money!" });
                }

            }
            else
            {
                return Json(new { message = "Invalid user!" });
            }

            return ViewComponent("UserBalance");
        }

        public IActionResult Index()
        {
            var retrieveViewModel = new RetrieveViewModel();

            return View(retrieveViewModel);
        }
    }
}