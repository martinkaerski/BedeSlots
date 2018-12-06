using BedeSlots.Data.Models;
using BedeSlots.Services.Data.Contracts;
using BedeSlots.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BedeSlots.Web.Controllers
{
    [Authorize]
    public class DepositController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly ITransactionService transactionService;
        private readonly IUserService userService;
        private readonly ICardService cardService;
        private readonly IUserBalanceService depositService;

        public DepositController(UserManager<User> userManager, IUserBalanceService depositService, ITransactionService transactionService, IUserService userService, ICardService cardService)
        {
            this.userManager = userManager;
            this.depositService = depositService;
            this.transactionService = transactionService;
            this.userService = userService;
            this.cardService = cardService;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Deposit()
        {
            var user = await this.userManager.GetUserAsync(HttpContext.User);

            var depositVM = new DepositViewModel()
            {
                Currency = user.Currency
            };

            return View(depositVM);
        }

        [HttpPost]
        public async Task<IActionResult> Deposit(DepositViewModel depositViewModel)
        {
            if (!ModelState.IsValid)
            {
                return ViewComponent("SelectCard");
            }


            var user = await this.userManager.GetUserAsync(HttpContext.User);
            var card = await this.cardService.GetCardByIdAsync(depositViewModel.BankCardId);

            var cardNumberLastFourDigits = card.Number.Substring(12, 4);

            var transaction = await this.transactionService.AddTransactionAsync(TransactionType.Deposit, user.Id,
                cardNumberLastFourDigits, depositViewModel.DepositAmount, user.Currency);

            var depositTransaction = await this.depositService.DepositMoneyAsync(depositViewModel.DepositAmount, user.Id);

            string currencySymbol = WebConstants.CurrencySymbols[user.Currency];
            this.StatusMessage = $"Successfully deposit {depositViewModel.DepositAmount} {currencySymbol}.";

            return PartialView("_StatusMessage", this.StatusMessage);
        }

        public IActionResult DepositInfo()
        {
            return View();
        }
    }
}