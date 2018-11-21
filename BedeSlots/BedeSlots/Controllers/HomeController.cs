using BedeSlots.Data.Models;
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
        private readonly IExchangeRateApiCaller exchangeRateApiCaller;
        private readonly UserManager<User> userManager;

        public HomeController(IExchangeRateApiCaller exchangeRateApiCaller, UserManager<User> userManager)
        {
            this.exchangeRateApiCaller = exchangeRateApiCaller;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var result = await this.exchangeRateApiCaller.GetCurrenciesRates();

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
