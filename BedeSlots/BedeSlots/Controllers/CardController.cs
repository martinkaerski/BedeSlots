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
        private readonly UserManager<User> userManager;
        private readonly IUserService userService;
        private readonly ICardService cardService;

        public CardController(UserManager<User> userManager, IUserService userService, ICardService cardService)
        {
            this.userManager = userManager;
            this.userService = userService;
            this.cardService = cardService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AddCard()
        {
            var cardTypes = await this.cardService.GetCardTypesAsync();
            var cardTypesSelectList = cardTypes.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();

            var addCardVM = new AddCardViewModel() { CardTypes = cardTypesSelectList };
            return View(addCardVM);
        }
        
        // TODO implement Card details view and redirect action to it 
        [HttpPost]
        public async Task<IActionResult> AddCard(string cardNumber, int cvv, DateTime expiry, int cardTypeId)
        {
            var cardtype = await this.cardService.GetCardTypeByIdaAsync(cardTypeId);
            var user = await this.userManager.GetUserAsync(HttpContext.User);

            //var newCardUser = await this.userService.GetUserById(user.Id);

            var card = new BankCard()
            {
                Number = cardNumber,
                CvvNumber = cvv,
                ExpiryDate = expiry,
                TypeId = cardTypeId,
                Type = cardtype,
                User = user,
                UserId = user.Id

            };

            await this.cardService.AddCardAsync(card);
            return View();// replace view() with rediredect()
        }
    }
}