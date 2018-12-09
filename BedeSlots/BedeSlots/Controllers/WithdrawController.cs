﻿using BedeSlots.Services.Data.Contracts;
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
        public async Task<IActionResult> Retrieve(RetrieveViewModel model)
        {
            if (!ModelState.IsValid)
            {
                this.StatusMessage = "Error! The withdraw is not completed.";
                return PartialView("_StatusMessage", this.StatusMessage);
            }

            var userId = HttpContext.User.Claims.FirstOrDefault().Value;

            // TODO: refactoring...
            if (userId != null)
            {
                try
                {
                    // simulate transfer between this application and current user's bank account
                    await this.userBalanceService.ReduceMoneyAsync(model.Amount, userId);
                    var card = await this.cardService.GetCardDetailsByIdAsync(model.BankCardId);
                    var cardLastFourDigits = card.LastFourDigit;

                    var userCurrency = await this.currencyService.GetUserCurrency(userId);

                    await this.transactionService.AddTransactionAsync(Data.Models.TransactionType.Withdraw, userId, cardLastFourDigits, model.Amount, userCurrency);
                }
                catch (Exception)
                {
                    return Json(new { message = "Can't retrieve this amount of money!" });
                }

            }
            else
            {
                return Json(new { message = "Invalid user!" });
            }

            return ViewComponent("UserBalance");
        }

        [HttpGet]
        public IActionResult Index()
        {
            var retrieveViewModel = new RetrieveViewModel();

            return View(retrieveViewModel);
        }
    }
}