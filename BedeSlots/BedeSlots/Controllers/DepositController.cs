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
        private readonly IDepositService depositService;

        public DepositController(UserManager<User> userManager, IDepositService depositService, ITransactionService transactionService, IUserService userService, ICardService cardService)
        {
            this.userManager = userManager;
            this.depositService = depositService;
            this.transactionService = transactionService;
            this.userService = userService;
            this.cardService = cardService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Deposit()
        {
            var user = await this.userManager.GetUserAsync(HttpContext.User);
            // TODO temp check before private funtionality implementation is done

            var cards = await this.cardService.GetUserCardsAsync(user.Id);
            var cardsSelectList = cards.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Number }).ToList();

            var cardTypes = Enum.GetValues(typeof(CardType)).Cast<CardType>();
            var cardTypesSelectList = cardTypes.Select(c => new SelectListItem { Value = c.ToString(), Text = c.ToString() }).ToList();

            var depositVM = new DepositViewModel() { BankCards = cardsSelectList, CardTypes = cardTypesSelectList };

            return View(depositVM);
        }

        [HttpPost]
        public async Task<IActionResult> Deposit(DepositViewModel depositViewModel)
        {
            if (!ModelState.IsValid)
            {
                return Redirect("Deposit");
            }

            var user = await this.userManager.GetUserAsync(HttpContext.User);
            var card = await this.cardService.GetCardByIdAsync(depositViewModel.BankCardId);

            var cardNumberLastFourDigits = card.Number.Substring(12, 4);
            var transaction = await this.transactionService.AddTransactionAsync(TransactionType.Deposit, user.Id,
                cardNumberLastFourDigits, depositViewModel.Amount);

            var depositTransaction = await this.depositService.DepositMoneyAsync(depositViewModel.Amount, user.Id);

            return View("DepositInfo");
        }

        public IActionResult DepositInfo()
        {
            return View();
        }
    }
}