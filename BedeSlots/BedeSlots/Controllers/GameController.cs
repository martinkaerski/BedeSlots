using BedeSlots.Data.Models;
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
        private readonly IUserBalanceService depositService;
        private readonly IUserService userService;

        public GameController(IGame game, ITransactionService transactionService, UserManager<User> userManager, IUserBalanceService depositService, IUserService userService)
        {
            this.game = game;
            this.transactionService = transactionService;
            this.userManager = userManager;
            this.depositService = depositService;
            this.userService = userService;
        }

        public  IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Spin(GameStakeViewModel stakeModel)
        {
            decimal stake = stakeModel.Stake;
            rows = stakeModel.Rows;
            cols = stakeModel.Cols;

            GameType gameType;

            if (rows == 3 && cols == 4)
            {
                gameType = GameType._4x3;
            }
            else if (rows == 5 && cols == 5)
            {
                gameType = GameType._5x5;
            }
            else if (rows == 5 && cols == 8)
            {
                gameType = GameType._8x5;
            }
            else
            {
                return this.RedirectToAction("Index");
            }

            var user = await userManager.GetUserAsync(HttpContext.User);
            var convertedUserBalance = await this.userService.GetUserBalanceByIdAsync(user.Id);

            if (convertedUserBalance >= stake)
            {
                await this.depositService.GetMoneyAsync(stake, user.Id);
                string gameTypeString = gameType.ToString().Substring(1);

                var stakeTransaction = await this.transactionService.AddTransactionAsync(TransactionType.Stake, user.Id, gameTypeString, stake, user.Currency);

                var result = game.Spin(rows, cols, stake);

                var model = new GameSlotViewModel()
                {
                    Rows = rows,
                    Cols = cols,
                    Matrix = result.Matrix,
                    Stake = result.Money,
                    Balance = convertedUserBalance - stake,
                    Currency = user.Currency
                };

                if (result.Money > 0)
                {
                    var winTransaction = await this.transactionService.AddTransactionAsync(TransactionType.Win, user.Id, gameTypeString, result.Money, user.Currency);

                    await depositService.DepositMoneyAsync(result.Money, user.Id);
                    model.Balance += result.Money;
                    model.Message = $"You won {Math.Round(result.Money, 2)}";
                }
                else
                {
                    model.Message = $"Try again!";
                }

                return this.PartialView("_GameSlotPartial", model);
            }
            else
            {
                var model = new GameSlotViewModel()
                {
                    Rows = rows,
                    Cols = cols,
                    Balance = user.Balance,
                    Message = "Not enough money",
                    Currency = user.Currency
                };

                return this.PartialView("_GameSlotPartial", model);
            }
        }

        public IActionResult BalanceViewComponent()
        {
            return ViewComponent("UserBalance");
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<JsonResult> EnoughMoney(decimal amount, string userId)
        {
            var user = await userManager.GetUserAsync(HttpContext.User);

            return user.Balance >= 0 ? Json(true) : Json($"Not enough money!");
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
            var matrix = game.GenerateMatrix(rows, cols, null);
            var stringMatrix = game.GetCharMatrix(matrix);
            var convertedUserBalance = await this.userService.GetUserBalanceByIdAsync(user.Id);

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