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

        public WithdrawController(IUserBalanceService userBalanceService, IUserService userService, ITransactionService transactionService, ICardService cardService)
        {
            this.userBalanceService = userBalanceService;
            this.userService = userService;
            this.transactionService = transactionService;
            this.cardService = cardService;
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

            // simulate transfer between this application and current user's bank account
            await this.userBalanceService.RetrieveMoneyAsync(model.RetrieveAmount, userId);

            var card = await this.cardService.GetCardDetailsByIdAsync(model.BankCardId);

            var userCurrency = await this.userService.GetUserCurrencyByIdAsync(userId);

            await this.transactionService.AddTransactionAsync(Data.Models.TransactionType.Withdraw,
                userId, card.LastFourDigit, model.RetrieveAmount, userCurrency);

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