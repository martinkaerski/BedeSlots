﻿using BedeSlots.Data.Models;
using BedeSlots.Games.Contracts;
using BedeSlots.Services.Data.Contracts;
using BedeSlots.Web.Models;
using BedeSlots.Web.Models.GameViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BedeSlots.Web.Controllers
{
    [Authorize]
    public class GameController : Controller
    {
        private int rows = 4;
        private int cols = 3;

        private readonly IGame game;
        private readonly UserManager<User> userManager;
        private readonly ITransactionService transactionService;
        private readonly IUserBalanceService userBalanceService;
        private readonly IUserService userService;

        public GameController(IGame game, ITransactionService transactionService, UserManager<User> userManager, IUserBalanceService userBalanceService, IUserService userService)
        {
            this.game = game;
            this.transactionService = transactionService;
            this.userManager = userManager;
            this.userBalanceService = userBalanceService;
            this.userService = userService;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Spin(GameStakeViewModel stakeModel)
        {
            if (!ModelState.IsValid)
            {
                this.StatusMessage = "Error! The bet is not completed.";
                return PartialView("_StatusMessage", this.StatusMessage);
            }

            var user = await userManager.GetUserAsync(HttpContext.User);
            var convertedUserBalance = await this.userBalanceService.GetUserBalanceByIdAsync(user.Id);

            decimal stake = stakeModel.Stake;
            if (stake > convertedUserBalance)
            {
                ModelState.AddModelError("Stake", "Error! You don't have enough money to make this bet!");
                this.StatusMessage = "Error! You don't have enough money to make this bet!";
                return PartialView("_StatusMessage", this.StatusMessage);
            }

            rows = stakeModel.Rows;
            cols = stakeModel.Cols;

            GameType gameType;

            if (rows == 4 && cols == 3)
            {
                gameType = GameType._4x3;
            }
            else if (rows == 5 && cols == 5)
            {
                gameType = GameType._5x5;
            }
            else if (rows == 8 && cols == 5)
            {
                gameType = GameType._8x5;
            }
            else
            {
                return this.RedirectToAction("Index");
            }

            await this.userBalanceService.ReduceMoneyAsync(stake, user.Id);
            string gameTypeString = gameType.ToString().Substring(1);

            var stakeTransaction = await this.transactionService.AddTransactionAsync(TransactionType.Stake, user.Id, gameTypeString, stake, user.Currency);

            var result = game.Spin(rows, cols, stake);

            var model = new GameSlotViewModel()
            {
                Rows = rows,
                Cols = cols,
                Matrix = result.Matrix,
                Stake = result.Amount,
                Balance = convertedUserBalance - stake,
                Currency = user.Currency
            };

            if (result.Amount > 0)
            {
                var winTransaction = await this.transactionService.AddTransactionAsync(TransactionType.Win, user.Id, gameTypeString, result.Amount, user.Currency);

                await userBalanceService.DepositMoneyAsync(result.Amount, user.Id);
                model.Balance += result.Amount;
                model.Message = $"You won {Math.Round(result.Amount, 2)}";
            }
            else
            {
                model.Message = $"Try again!";
            }

            return this.PartialView("_GameSlotPartial", model);
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<JsonResult> EnoughMoney(decimal stake)
        {
            var user = await userManager.GetUserAsync(HttpContext.User);
            var userBalance = await this.userBalanceService.GetUserBalanceByIdAsync(user.Id);

            return stake <= userBalance ? Json(true) : Json($"You don't have enough money! Please reduce your bet or make a deposit.");
        }

        public async Task<IActionResult> SlotMachine(string size)
        {
            switch (size)
            {
                case "4x3":
                    rows = 4;
                    cols = 3;
                    break;
                case "5x5":
                    rows = 5;
                    cols = 5;
                    break;
                case "8x5":
                    rows = 8;
                    cols = 5;
                    break;
                default:
                    return this.RedirectToAction("Index");
            }

            var user = await userManager.GetUserAsync(HttpContext.User);
            var stringMatrix = game.GenerateCharMatrix(rows, cols);
            var convertedUserBalance = await this.userBalanceService.GetUserBalanceByIdAsync(user.Id);

            var model = new GameSlotViewModel()
            {
                Rows = rows,
                Cols = cols,
                Matrix = stringMatrix,
                Balance = convertedUserBalance,
                Message = "Good luck!",
                Currency = user.Currency
            };

            return View(model);
        }
    }
}