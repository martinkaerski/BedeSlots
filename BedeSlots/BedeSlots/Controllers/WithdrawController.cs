using BedeSlots.Services.Data.Contracts;
using BedeSlots.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BedeSlots.Web.Controllers
{
    [Authorize]
    public class WithdrawController : Controller
    {
        private readonly IUserBalanceService userBalanceService;
        private readonly IUserService userService;
        private readonly ITransactionService transactionService;
        private readonly ICardService cardService;
        private readonly ICurrencyService currencyService;

        public WithdrawController(IUserBalanceService userBalanceService, IUserService userService, ITransactionService transactionService, ICardService cardService, ICurrencyService currencyService)
        {
            this.userBalanceService = userBalanceService;
            this.userService = userService;
            this.transactionService = transactionService;
            this.cardService = cardService;
            this.currencyService = currencyService;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Withdraw(RetrieveViewModel model)
        {
            if (!ModelState.IsValid)
            {
                this.StatusMessage = "Error! The withdraw is not completed.";
                return PartialView("_StatusMessage", this.StatusMessage);
            }

            var userId = HttpContext.User.Claims.FirstOrDefault().Value;

            // simulate transfer between this application and current user's bank account
            await this.userBalanceService.ReduceMoneyAsync(model.Amount, userId);

            var card = await this.cardService.GetCardDetailsByIdAsync(model.BankCardId);
            var userCurrency = await this.currencyService.GetUserCurrencyAsync(userId);

            await this.transactionService.AddTransactionAsync(Data.Models.TransactionType.Withdraw,
                userId, card.LastFourDigit, model.Amount, userCurrency);

            return ViewComponent("UserBalance");
        }

        [HttpGet]
        public IActionResult Index()
        {
            var retrieveViewModel = new RetrieveViewModel();

            return View(retrieveViewModel);
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<JsonResult> HasEnoughMoneyAsync(decimal retrieveAmount)
        {
            var userBalance = await this.userBalanceService.GetUserBalanceByIdAsync(this.User.Claims.FirstOrDefault().Value);

            if (userBalance >= retrieveAmount)
            {
                return Json(true);
            }
            else
            {
                return Json("Can't withdraw this amount of money!");
            }
        }
    }
}