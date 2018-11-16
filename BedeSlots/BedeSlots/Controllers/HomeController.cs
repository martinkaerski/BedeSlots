using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BedeSlots.Models;
using BedeSlots.Web.Models;
using BedeSlots.Services.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BedeSlots.Data.Models;
using BedeSlots.Services.Data.Contracts;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BedeSlots.Controllers
{
    public class HomeController : Controller
    {
        //private readonly UserManager<User> userManager;
        //private readonly IUserService userService;
        //private readonly ICardService cardService;

        public HomeController(/*IUserService userService, ICardService cardService, UserManager<User> userManager*/)
        {
            //this.userService = userService;
            //this.cardService = cardService;
            //this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        //[HttpGet]
        //public async Task<IActionResult> Deposit()
        //{
        //    var user = await this.userManager.GetUserAsync(HttpContext.User);
        //    var cards = await this.cardService.GetUserCardsAsync(user.Id);
        //    var cardsSelectList = cards.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Number }).ToList();

        //    var cardTypes = await this.cardService.GetCardTypesAsync();
        //    var cardTypesSelectList = cardTypes.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();

        //    var depositVM = new DepositViewModel() { BankCards = cardsSelectList, CardTypes = cardTypesSelectList };
        //    return View(depositVM);
        //}
        //// TODO after addCard action
        //public IActionResult Deposit(string bankCard, int depositValue)
        //{
        //    return View();
        //}

        //[HttpGet]
        //public async Task<IActionResult> AddCard()
        //{
        //    var cardTypes = await this.cardService.GetCardTypesAsync();
        //    var cardTypesSelectList = cardTypes.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();

        //    var addCardVM = new AddCardViewModel() { CardTypes = cardTypesSelectList };
        //    return View(addCardVM);
        //}

        //[HttpPost]
        //public async Task<IActionResult> AddCard(string cardNumber, int cvv, DateTime expiry, int cardTypeId)
        //{
        //    var cardtype = await this.cardService.GetCardTypeByIdaAsync(cardTypeId);
        //    var user = await this.userManager.GetUserAsync(HttpContext.User);

        //    //var newCardUser = await this.userService.GetUserById(user.Id);

        //    var card = new BankCard()
        //    {
        //        Number = cardNumber,
        //        CvvNumber = cvv,
        //        ExpiryDate = expiry,
        //        TypeId = cardTypeId,
        //        Type = cardtype,
        //        User = user,
        //        UserId = user.Id

        //    };

        //    await this.cardService.AddCardAsync(card);
        //    return View();// replace view() with rediredect()
        //}

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
