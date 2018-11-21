using BedeSlots.Models;
using BedeSlots.Services.Data;
using BedeSlots.Services.Data.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BedeSlots.Controllers
{
    public class HomeController : Controller
    {
        private readonly IExchangeRateApiCaller exchangeRateApiCaller;

        public HomeController(IExchangeRateApiCaller exchangeRateApiCaller)
        {
            this.exchangeRateApiCaller = exchangeRateApiCaller;
        }

        public async Task<IActionResult> Index()
        {
             var result = await this.exchangeRateApiCaller.GetCurrenciesRates();
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
