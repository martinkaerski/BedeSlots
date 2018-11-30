using BedeSlots.Data.Models;
using BedeSlots.Games.Contracts;
using BedeSlots.Services.Data.Contracts;
using BedeSlots.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BedeSlots.Web.Controllers
{
    [Authorize]
    public class GameController : Controller
    {
        const int rows = 4;
        const int cols = 3;

        private readonly IGame game;
        private readonly UserManager<User> userManager;
        private readonly ITransactionService transactionService;
        private readonly IDepositService depositService;

        public GameController(IGame game, ITransactionService transactionService, UserManager<User> userManager, IDepositService depositService)
        {
            this.game = game;
            this.transactionService = transactionService;
            this.userManager = userManager;
            this.depositService = depositService;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = await userManager.GetUserAsync(HttpContext.User);
            var matrix = game.GenerateMatrix(rows, cols, null);
            var stringMatrix = game.GetCharMatrix(matrix);

            var model = new GameSlotViewModel()
            {
                Rows = rows,
                Cols = cols,
                Matrix = stringMatrix,
                Balance = user.Balance,
                Message = "Good luck!"
            };

            return View(model);
        }

        public async Task<IActionResult> Spin(decimal stake)
        {
            var user = await userManager.GetUserAsync(HttpContext.User);

            if (user.Balance >= stake)
            {
                await this.depositService.WithdrawMoneyAsync(stake, user.Id);
                string gameType = GameType._4x3.ToString().Substring(1);
                
                var stakeTransaction = await this.transactionService.AddTransactionAsync(TransactionType.Stake, user.Id, gameType ,stake);

                var result = game.Spin(rows, cols, stake);

                var model = new GameSlotViewModel()
                {
                    Rows = rows,
                    Cols = cols,
                    Matrix = result.Matrix,
                    Stake = result.Money,
                    Balance = user.Balance
                };

                if (result.Money > 0)
                {
                    var winTransaction = await this.transactionService.AddTransactionAsync(TransactionType.Win, user.Id, gameType ,result.Money);

                    await depositService.DepositMoneyAsync(result.Money, user.Id);
                    model.Balance += result.Money;
                    model.Message = $"You won {result.Money}";
                }
                else
                {
                    model.Message = $"Bad luck. Try again!";
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
                    Message = "Not enough money"
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
    }
}