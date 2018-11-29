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
                AvailableMoney = user.Balance,
                Message = "Good luck!"
            };

            return View(model);
        }

        public async Task<IActionResult> Spin(decimal money)
        {
            var user = await userManager.GetUserAsync(HttpContext.User);

            if (user.Balance >= money)
            {
                await this.depositService.WithdrawMoneyAsync(money, user.Id);
                var stakeTransaction = this.transactionService.AddTransaction(TransactionType.Stake, user.Id, null, money, GameType._4x3);

                var result = game.Spin(5, 5, money);

                var model = new GameSlotViewModel()
                {
                    Rows = rows,
                    Cols = cols,
                    Matrix = result.Matrix,
                    Money = result.Money,
                    AvailableMoney = user.Balance
                };

                if (result.Money > 0)
                {
                    var winTransaction = this.transactionService.AddTransaction(TransactionType.Win, user.Id, null, result.Money, GameType._4x3);
                    await depositService.DepositAsync(result.Money, user.Id);
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
                    AvailableMoney = user.Balance,
                    Message = "Not enough money"
                };

                return this.PartialView("_GameSlotPartial", model);
            }
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<JsonResult> EnoughMoney(decimal amount, string userId)
        {
            var user = await userManager.GetUserAsync(HttpContext.User);

            return user.Balance >= 0 ? Json(true) : Json($"Not enough money!");
        }
    }
}