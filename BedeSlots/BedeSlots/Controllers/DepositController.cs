using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BedeSlots.Data.Models;
using BedeSlots.Services.Data;
using BedeSlots.Services.Data.Contracts;
using BedeSlots.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BedeSlots.Web.Controllers
{
    public class DepositController : Controller
    {

        private readonly UserManager<User> userManager;
        private readonly ITransactionService transactionService;
        private readonly IUserService userService;
        private readonly ICardService cardService;

        public DepositController(UserManager<User> userManager, ITransactionService transactionService, IUserService userService, ICardService cardService)
        {
            this.userManager = userManager;
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
            if (user != null)
            {
                var cards = await this.cardService.GetUserCardsAsync(user.Id);
                var cardsSelectList = cards.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Number }).ToList();

                var cardTypes = await this.cardService.GetCardTypesAsync();
                var cardTypesSelectList = cardTypes.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();

                var depositVM = new DepositViewModel() { BankCards = cardsSelectList, CardTypes = cardTypesSelectList };
                return View(depositVM);
            }
            else
            {
                return Redirect("/account/login");
            }
        }

        public async Task<IActionResult> Deposit(int bankCardId, int depositAmount)
        {
            if (!ModelState.IsValid)
            {
                return Redirect("Deposit");
            }
            var user = await this.userManager.GetUserAsync(HttpContext.User);
            var bankCard = await this.cardService.GetCardByIdAsync(bankCardId);

            var cardNumberLast4Digits = bankCard.Number.Substring(12, 4);
            var cardNumberWhithHiddenFirst12Digits = new string('*', 12) + cardNumberLast4Digits;

            var transaction = new Transaction()
            {
                Amount = depositAmount,
                Date = DateTime.Now,
                Type = TransactionType.Deposit,
                Description = $"Deposit with card {cardNumberWhithHiddenFirst12Digits}",
                UserId = user.Id,
            };

            var depositTransaction = await this.userService.DepositAsync(transaction);

            var registeredTransaction = await this.transactionService.RegisterTransactionsAsync(depositTransaction);
            // TODO considering status message or details page..
            return View("DepositInfo");

        }

        public IActionResult DepositInfo()
        {
            return View();
        }

    }
}