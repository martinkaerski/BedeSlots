using BedeSlots.Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BedeSlots.Web.Models
{
    public class AddCardViewModel
    {
        [Display(Name = "Card number")]
        public string cardNumber { get; set; }

        [Display(Name = "CVV")]
        public int Cvv { get; set; }

        [Display(Name = "Expiry")]
        public DateTime Expiry { get; set; }

        [Display(Name = "Card type")]
        public int CardTypeId { get; set; }

        [Display(Name = "Currency")]
        public int CurrencyId { get; set; }

        public List<SelectListItem> CardTypes { get; set; }
        public List<SelectListItem> Currencies { get; set; }

        public AddCardViewModel()
        {

        }
    }
}
