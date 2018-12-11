using BedeSlots.Data.Models;
using BedeSlots.DTO;
using BedeSlots.Services.Data.Contracts;
using BedeSlots.Web.Providers.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace BedeSlots.Web.Controllers
{
    [Authorize]
    public class HistoryController : Controller
    {
        private readonly ITransactionService transactionService;
        private readonly IPaginationProvider<TransactionDto> paginationProvider;
        private readonly UserManager<User> userManager;
        private readonly ICurrencyConverterService currencyConverterService;

        public HistoryController(ITransactionService transactionService, IPaginationProvider<TransactionDto> paginationProvider, UserManager<User> userManager, ICurrencyConverterService currencyConverterService)
        {
            this.transactionService = transactionService;
            this.paginationProvider = paginationProvider;
            this.userManager = userManager;
            this.currencyConverterService = currencyConverterService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoadData()
        {
            try
            {
                var user = await this.userManager.GetUserAsync(HttpContext.User);

                string draw, sortColumn, sortColumnDirection, searchValue;
                int pageSize, skip, recordsTotal;

                this.paginationProvider.GetParameters(out draw, out sortColumn, out sortColumnDirection, out searchValue, out pageSize, out skip, out recordsTotal, HttpContext, Request);

                var transactions = this.transactionService.GetUserTransactions(HttpContext.User.Claims.FirstOrDefault().Value);

                //Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    transactions = transactions
                        .Where(t => t.User.Contains(searchValue)
                        || t.Description.Contains(searchValue));
                }

                //Sorting
                transactions = this.paginationProvider.SortData(sortColumn, sortColumnDirection, transactions);

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
                        Amount = Math.Round(this.currencyConverterService.ConvertFromBaseToOtherAsync(t.Amount, user.Currency).Result, 2) + WebConstants.CurrencySymbols[user.Currency],
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