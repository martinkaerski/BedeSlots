using BedeSlots.Data.Models;
using BedeSlots.Services.Data.Contracts;
using BedeSlots.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Threading.Tasks;
using BedeSlots.Common;

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

            var cardNumberWithoutSpaces = model.CardNumber.Replace(" ", "");

            var card = new BankCard()
            {
                Number = cardNumberWithoutSpaces,
                CardholerName = model.CardholderName,
                CvvNumber = model.Cvv,
                ExpiryDate = model.Expiry,
                Type = model.CardType,
                UserId = userId,
            };

            await this.cardService.AddCardAsync(card);

            return ViewComponent("SelectCard");
        }

        //TODO: change deposit
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var card = await this.cardService.DeleteCardAsync(id);
            return RedirectToAction("Deposit", "Deposit");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int cardId)
        {
            var card = await this.cardService.GetCardByIdAsync(cardId);

            var model = new CardInfoViewModel()
            {
                Id = card.Id,
                CardNumber = card.Number.Substring(12),
                CardType = card.Type.GetDisplayName(),
                Cvv = card.CvvNumber,
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
                return Json($"Card {cardNumber} allready added!");
            }
        }
    }
}