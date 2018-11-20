﻿using BedeSlots.Services.Data.Contracts;
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

        public TransactionsController(ITransactionService transactionService)
        {
            this.transactionService = transactionService;
        }

        public async Task<IActionResult> Index()
        {
            var transactions = await transactionService.GetAllTransactionsAsync();
            var list = new List<TransactionHistoryViewModel>();

            foreach (var transaction in transactions)
            {
                var transactionViewModel = new TransactionHistoryViewModel()
                {
                    Id = transaction.Id,
                    Date = transaction.Date,
                    Type = transaction.Type.ToString(),
                    Amount = transaction.Amount,
                    Description = transaction.Description,
                    UserEmail = transaction.User.Email
                };

                list.Add(transactionViewModel);
            }

            return View(list);
        }

        // GET: Admin/Transactions/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var transaction = await transactionService.GetTransactionAsync(id);

            if (transaction == null)
            {
                return NotFound();
            }

            var model = new TransactionDetailsViewModel()
            {
                Date = transaction.Date,
                Type = transaction.Type.ToString(),
                Amount = transaction.Amount,
                Description = transaction.Description,
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
