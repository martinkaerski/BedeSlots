using BedeSlots.Data.Models;
using BedeSlots.Games.Contracts;
using BedeSlots.Services.Data.Contracts;
using BedeSlots.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace BedeSlots.Web.Controllers
{    
    [Authorize]
    public class GameController : Controller
    {
        private readonly IGame game;
        private readonly UserManager<User> userManager;
        private readonly ITransactionService transactionService;

        public GameController(IGame game, ITransactionService transactionService, UserManager<User> userManager)
        {
            this.game = game;
            this.transactionService = transactionService;
            this.userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var matrix = game.GenerateMatrix(5, 5, null);
            var stringMatrix = game.GetCharMatrix(matrix);

            var model = new GameSlotViewModel()
            {
                Rows = 5,
                Cols = 5,
                Matrix = stringMatrix
            };

            return View(model);
        }

        public async Task<IActionResult> Spin(decimal money)
        {
            var user = await userManager.GetUserAsync(HttpContext.User);
            var stakeTransaction = this.transactionService.CreateTransaction(TransactionType.Stake, user.Id, null, money);
            await transactionService.RegisterTransactionsAsync(stakeTransaction);
            var result = game.Spin(5, 5, money);

            var model = new GameSlotViewModel()
            {
                Rows = 5,
                Cols = 5,
                Matrix = result.Matrix,
                Money = result.Money
            };

            if (result.Money > 0)
            {
                var winTransaction = this.transactionService.CreateTransaction(TransactionType.Win, user.Id, null, result.Money);
                await transactionService.RegisterTransactionsAsync(winTransaction);
            }

            return this.PartialView("_GameSlotPartial", model);
        }
    }
}