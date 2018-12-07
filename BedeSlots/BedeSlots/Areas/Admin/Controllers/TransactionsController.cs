﻿using BedeSlots.Common;
using BedeSlots.Data.Models;
using BedeSlots.Services.Data.Contracts;
using BedeSlots.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace BedeSlots.Web.Areas.Admin.Controllers
{
    [Area(WebConstants.AdminArea)]
    [Authorize(Roles = WebConstants.AdminRole + "," + WebConstants.MasterAdminRole)]
    public class TransactionsController : Controller
    {
        private readonly ITransactionService transactionService;
        private readonly ICardService cardService;

        public TransactionsController(ITransactionService transactionService, ICardService cardService)
        {
            this.transactionService = transactionService;
            this.cardService = cardService;
        }

        public IActionResult Index()
        {
            return View();
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
                Description = transaction.Description,
                UserEmail = transaction.User.Email,
                FirstName = transaction.User.FirstName,
                LastName = transaction.User.LastName,
                Birthdate = transaction.User.Birthdate,
                Cards = transaction.User.Cards.Select(c => c.Number).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult LoadData()
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                // Skiping number of Rows count
                var start = Request.Form["start"].FirstOrDefault();
                // Paging Length 10,20
                var length = Request.Form["length"].FirstOrDefault();
                // Sort Column Name
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                // Sort Column Direction ( asc ,desc)
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                // Search Value from (Search box)
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                //Paging Size (10,20,50,100)
                int pageSize = length != null ? int.Parse(length) : 0;
                int skip = start != null ? int.Parse(start) : 0;
                int recordsTotal = 0;

                var transactions = this.transactionService.GetAllTransactions();


                //Search
                if (!String.IsNullOrEmpty(searchValue.ToLower()))
                {
                    transactions = transactions
                        .Where(t => t.User.ToLower().Contains(searchValue)
                        || t.Description.Contains(searchValue));
                }

                //Sorting
                if (!(String.IsNullOrEmpty(sortColumn) && String.IsNullOrEmpty(sortColumnDirection)))
                {
                    if (sortColumnDirection == "asc")
                    {
                        transactions = transactions
                            .OrderBy(t => t.GetType().GetProperty(sortColumn).GetValue(t));
                    }
                    else
                    {
                        transactions = transactions
                            .OrderByDescending(t => t.GetType().GetProperty(sortColumn).GetValue(t));
                    }
                }

                //Total number of rows count 
                recordsTotal = transactions.Count();

                //Paging 
                var dataFiltered = transactions
                    .Skip(skip)
                    .Take(pageSize)
                    .ToList();

                var data = dataFiltered
                    .Select(t => new
                    {
                        Date = t.Date.ToString("G", CultureInfo.InvariantCulture),
                        Type = t.Type.ToString(),
                        Amount = CommonConstants.BaseCurrencySymbol + t.Amount.ToString(),
                        Description = GetDescriptionByTransactionType(t.Type) + t.Description,
                        User = t.User
                    })
                    .ToList();

                //Returning Json Data
                return Json(new { draw, recordsFiltered = recordsTotal, recordsTotal, data });
            }
            catch (Exception)
            {
                throw;
            }
        }
        private string GetDescriptionByTransactionType(TransactionType type)
        {
            if (type == TransactionType.Deposit || type == TransactionType.Withdraw)
            {
                return $"{type.ToString()} with card **** **** **** ";
            }
            else 
            {
                return $"{type.ToString()} on game ";
            }
        }

    }
}
