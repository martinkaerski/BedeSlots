﻿using BedeSlots.Common;
using BedeSlots.Common.Providers.Contracts;
using BedeSlots.Data.Models;
using BedeSlots.Services.Data.Contracts;
using BedeSlots.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BedeSlots.Web.Controllers
{
    [Authorize]
    public class CardController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly ICardService cardService;
        private readonly IDateTimeProvider dateTimeProvider;

        public CardController(UserManager<User> userManager, ICardService cardService, IDateTimeProvider dateTimeProvider)
        {
            this.userManager = userManager;
            this.cardService = cardService;
            this.dateTimeProvider = dateTimeProvider;
        }

        [HttpGet]
        public IActionResult AddCard()
        {
            var cardTypes = Enum.GetValues(typeof(CardType)).Cast<CardType>();

            var cardTypesSelectList = cardTypes.Select(c => new SelectListItem { Value = ((int)c).ToString(), Text = c.ToString() }).ToList();

            var addCardVM = new AddCardViewModel() { CardTypes = cardTypesSelectList };
            return View(addCardVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCard(AddCardViewModel model)
        {
            var userId = this.userManager.GetUserId(HttpContext.User);

            if (!ModelState.IsValid)
            {
                return Json(new { message = $"Invalid parameters!" });
            }
            DateTime expiryDate = model.Expiry;

            var card = await this.cardService.AddCardAsync(model.CardNumber, model.CardholderName, model.Cvv, expiryDate, model.CardType, userId);

            return ViewComponent("SelectCard");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { message = $"Invalid parameters!" });
            }

            var card = await this.cardService.DeleteCardAsync(id);
            return RedirectToAction("Index", "Deposit");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int cardId)
        {
            var card = await this.cardService.GetCardDetailsByIdAsync(cardId);

            var model = new CardInfoViewModel()
            {
                Id = card.Id,
                CardNumber = card.LastFourDigit,
                CardType = card.Type.GetDisplayName(),
                Cvv = card.Cvv,
                Expiry = card.ExpiryDate,
                Cardholder = card.CardholerName
            };

            return PartialView("_DetailsCardPartial", model);
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<JsonResult> DoesCardExistInDatabase(string cardNumber)
        {
            var cardNumberWithoutSpaces = cardNumber.Replace(" ", "");

            var userId = this.userManager.GetUserId(HttpContext.User);

            var cardsNumbers = await this.cardService.GetUserCardsAllNumbersAsync(userId);

            var doesExists = cardsNumbers.Any(c => c.Number == cardNumberWithoutSpaces);

            if (!doesExists)
            {
                return Json(true);
            }
            else
            {
                return Json($"Card {cardNumber} already added!");
            }
        }

        [AcceptVerbs("Get", "Post")]
        public JsonResult IsValidExpiryDate(DateTime expiry)
        {
            if (expiry <= dateTimeProvider.Now)
            {
                return Json($"Invalid expiry date!");
            }
            else
            {
                return Json(true);
            }
        }
    }
}