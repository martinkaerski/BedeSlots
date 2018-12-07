using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BedeSlots.Services.Data.Contracts;
using BedeSlots.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace BedeSlots.Web.Controllers
{
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

        [HttpPost]
        public async Task<IActionResult> Retrieve(RetrieveViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json("Invalid parameters passed!");
            }
            var userId = HttpContext.User.Claims.FirstOrDefault().Value;


            if (userId != null)
            {
                try
                {
                    // simulate transfer between this application and current user's bank account
                    await this.userBalanceService.RetrieveMoneyAsync(model.RetrieveAmount, userId);
                    var card = await this.cardService.GetCardDetailsByIdAsync(model.BankCardId);
                    var cardLastFourDigits = card.LastFourDigit;

                    var user = await this.userService.GetUserCurrencyByIdAsync(userId);

                    await this.transactionService.AddTransactionAsync(Data.Models.TransactionType.Withdraw, userId, cardLastFourDigits, model.RetrieveAmount, user);
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