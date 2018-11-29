using BedeSlots.Data.Models;
using BedeSlots.Services.Data.Contracts;
using BedeSlots.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BedeSlots.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class TransactionsController : Controller
    {
        private readonly ITransactionService transactionService;
        private readonly ICardService cardService;

        public TransactionsController(ITransactionService transactionService, ICardService cardService)
        {
            this.transactionService = transactionService;
            this.cardService = cardService;
        }

        public async Task<IActionResult> Index()
        {
            var transactions = await this.transactionService.GetAllTransactionsAsync();
            var list = new List<TransactionHistoryViewModel>();

            foreach (var transaction in transactions.Reverse())
            {
                string descriptionData = string.Empty;

                if (transaction.Type == TransactionType.Deposit)
                {
                    if (transaction.CardId != null)
                    {
                        var card = await this.cardService.GetCardByIdAsync((int)transaction.CardId);
                        var cardNumberLastFourDigits = card.Number.Substring(12, 4);
                        descriptionData = cardNumberLastFourDigits;
                    }
                }
                else
                {
                    descriptionData = transaction.GameType.ToString().Substring(1);
                }

                var transactionViewModel = new TransactionHistoryViewModel()
                {
                    Id = transaction.Id,
                    Date = transaction.Date,
                    Type = transaction.Type,
                    GameType = transaction.GameType,
                    Amount = transaction.Amount,
                    Description = descriptionData,
                    UserEmail = transaction.User.Email
                };

                list.Add(transactionViewModel);
            }

            //TODO: return list or model with prop transactionHistoryVM?
            return View(list);
        }

        public async Task<IActionResult> Details(int id)
        {
            var transaction = await transactionService.GetTransactionByIdAsync(id);

            if (transaction == null)
            {
                return NotFound();
            }

            var model = new TransactionDetailsViewModel()
            {
                Date = transaction.Date,
                Type = transaction.Type.ToString(),
                Amount = transaction.Amount,
                Description = "card id =>" + transaction.CardId,
                UserEmail = transaction.User.Email,
                FirstName = transaction.User.FirstName,
                LastName = transaction.User.LastName,
                Birthdate = transaction.User.Birthdate,
                Cards = transaction.User.Cards.Select(c => c.Number).ToList()
            };

            return View(model);
        }
    }
}
