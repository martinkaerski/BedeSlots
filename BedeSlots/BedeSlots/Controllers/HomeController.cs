using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BedeSlots.Models;
using BedeSlots.Web.Models;
using BedeSlots.Services.Data;

namespace BedeSlots.Controllers
{
    public class HomeController : Controller
    {
        private readonly DepositService depositService;

        public HomeController(DepositService depositService)
        {
            this.depositService = depositService;
        }

        public IActionResult Index()
        {
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
        [HttpGet]
        public IActionResult Deposit()
        {
            //var cards = this.depositService.GetUserCards("test");

            var depositVM = new DepositViewModel() { };
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
