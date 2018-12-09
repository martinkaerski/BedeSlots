using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BedeSlots.Data;
using BedeSlots.Services.Data.Contracts;
using BedeSlots.Web.Models;
using Microsoft.AspNetCore.Identity;
using BedeSlots.Data.Models;

namespace BedeSlots.Web.ViewComponents
{
    public class UserBalance : ViewComponent
    {
        private readonly IUserBalanceService userBalanceService;
        private readonly UserManager<User> userManager;

        public UserBalance(IUserBalanceService userBalanceService, UserManager<User> userManager)
        {
            this.userBalanceService = userBalanceService;
            this.userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await this.userManager.GetUserAsync(HttpContext.User);

            var balance = await this.userBalanceService.GetUserBalanceByIdAsync(user.Id);
            var userBalanceVM = new UserBalanceViewModel()
            {
                Balance = Math.Round(balance, 2),
                Currency = user.Currency
            };

            return View("Default", userBalanceVM);
        }
    }
}
