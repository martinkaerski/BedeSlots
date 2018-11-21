﻿using BedeSlots.Data.Models;
using BedeSlots.Models;
using BedeSlots.Services.Data;
using BedeSlots.Services.Data.Contracts;
using BedeSlots.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BedeSlots.Controllers
{
    public class HomeController : Controller
    {
        private readonly IExchangeRateApiCallService exchangeRateApiCallService;
        private readonly UserManager<User> userManager;

        public HomeController(IExchangeRateApiCallService exchangeRateApiCallService, UserManager<User> userManager)
        {
            this.exchangeRateApiCallService = exchangeRateApiCallService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            //var result = this.exchangeRateApiCallService.GetAllRatesAsync();

            var user = await this.userManager.GetUserAsync(HttpContext.User);
            if (user != null)
            {
                ViewData["Balance"] = new UserBalanceViewModel() { Balance = user.Balance };
            }

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
