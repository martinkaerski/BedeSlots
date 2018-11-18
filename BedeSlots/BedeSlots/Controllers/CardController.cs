using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BedeSlots.Data.Models;
using BedeSlots.Services.Data;
using BedeSlots.Services.Data.Contracts;
using BedeSlots.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BedeSlots.Web.Controllers
{
    public class CardController : Controller
    {
        private readonly ICurrencyService currencyService;
        private readonly UserManager<User> userManager;
        private readonly IUserService userService;
        private readonly ICardService cardService;

        public CardController(ICurrencyService currencyService, UserManager<User> userManager, IUserService userService, ICardService cardService)
        {
            this.currencyService = currencyService;
            this.userManager = userManager;
            this.userService = userService;
            this.cardService = cardService;
        }
        
        [HttpGet]
        public async Task<IActionResult> AddCard()
        {
            var cardTypes = await this.cardService.GetCardTypesAsync();
            var cardTypesSelectList = cardTypes.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();
            var currencies = await this.currencyService.GetAllCurrenciesAsync();
            var currenciesSelectList = currencies.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToList();

            var addCardVM = new AddCardViewModel() { CardTypes = cardTypesSelectList, Currencies = currenciesSelectList };
            return View(addCardVM);
        }

        [HttpPost]
        public async Task<IActionResult> AddCard(string cardNumber, int cvv, DateTime expiry, int cardTypeId, int currencyId)
        {
            var user = await this.userManager.GetUserAsync(HttpContext.User);

            if (!ModelState.IsValid)
            {
                return Redirect("AddCard");
            }
            var card = new BankCard()
            {
                Number = cardNumber,
                CvvNumber = cvv,
                ExpiryDate = expiry,
                TypeId = cardTypeId,
                CurrencyId = currencyId,
                UserId = user.Id
            };

            await this.cardService.AddCardAsync(card);
            return Redirect($"CardInfo/{card.Id}");
        }

        public async Task<IActionResult> CardInfo(int id)
        {
            var card = await this.cardService.GetCardByIdAsync(id);

            var cardInfo = new CardInfoViewModel()
            {
                CardNumber = card.Number,
                CardType = card.Type,
                Currency = card.Currency,
                Cvv = card.CvvNumber.ToString(),
                Expiry = card.ExpiryDate.ToShortDateString(),
                Owner = card.User
            };

            return View(cardInfo);
        }
    }
}