using BedeSlots.Common;
using BedeSlots.Data.Models;
using BedeSlots.Services.Data.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Linq;

namespace BedeSlots.Web.Controllers
{
    public class HistoryController : Controller
    {
        private readonly ITransactionService transactionService;

        public HistoryController(ITransactionService transactionService)
        {
            this.transactionService = transactionService;
        }

        public IActionResult Index()
        {
            return View();
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

                // TODO replace GetAllTransaction() with GetUserTransactionAsync but now is not implemented
                var transactions = this.transactionService.GetUserTransactionsAsync(HttpContext.User.Claims.FirstOrDefault().Value);

                //Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    transactions = transactions.Where(t =>
                       EF.Functions.Like(t.User, "%" + searchValue + "%") ||
                       EF.Functions.Like(t.Description, "%" + searchValue + "%") ||
                       EF.Functions.Like(t.Type.ToString(), "%" + searchValue + "%"));

                    //transactions = transactions
                    //    .Where(t => t.User.Email.Contains(searchValue)
                    //    || t.Description.Contains(searchValue)
                    //    || t.Type.ToString().Contains(searchValue));
                }

                //Sorting
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
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
                var data = transactions
                    .Skip(skip)
                    .Take(pageSize)
                    .Select(t => new
                    {
                        Date = t.Date.ToString("G", CultureInfo.InvariantCulture),
                        Type = t.Type.ToString(),
                        Amount = CommonConstants.BaseCurrencySymbol + t.Amount.ToString(),
                        Description = t.Type == TransactionType.Deposit
                        ? $"Deposit with card **** **** **** {t.Description}"
                        : $"{t.Type.ToString()} on game {t.Description}",
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
    }
}