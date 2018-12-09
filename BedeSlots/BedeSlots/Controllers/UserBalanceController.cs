using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BedeSlots.Web.Controllers
{
    [Authorize]
    public class UserBalanceController : Controller
    {
        [HttpGet]
        public IActionResult BalanceViewComponent()
        {
            return ViewComponent("UserBalance");
        }
    }
}