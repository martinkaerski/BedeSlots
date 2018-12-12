﻿using BedeSlots.Data.Models;
using BedeSlots.DTO;
using BedeSlots.DTO.TransactionDto;
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
        private readonly IPaginationProvider<TransactionHistoryDto> paginationProvider;
        private readonly UserManager<User> userManager;
        private readonly ICurrencyConverterService currencyConverterService;

        public HistoryController(ITransactionService transactionService, IPaginationProvider<TransactionHistoryDto> paginationProvider, UserManager<User> userManager, ICurrencyConverterService currencyConverterService)
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
        public async Task<IActionResult> LoadData(string draw, string sortColumn, string sortColumnDirection, string searchValue)
        {
            try
            {
                var user = await this.userManager.GetUserAsync(HttpContext.User);

                //string draw, sortColumn, sortColumnDirection, searchValue;
                int pageSize, skip, recordsTotal;

                this.paginationProvider.GetParameters(out draw, out sortColumn, out sortColumnDirection, out searchValue, out pageSize, out skip, out recordsTotal, HttpContext, Request);

                var transactions = this.transactionService.GetUserTransactionsAsync(user.Id);

                //Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    transactions = transactions
                        .Where(t => t.Type.ToString().ToLower().Contains(searchValue)
                        || (t.Description.ToLower().Contains(searchValue)));
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
                        Description = GetDescriptionByTransactionType(t.Type) + t.Description
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
            return type == TransactionType.Deposit || type == TransactionType.Withdraw
                ? $"{type.ToString()} with card **** **** **** "
                : $"{type.ToString()} on game ";
        }
    }
}