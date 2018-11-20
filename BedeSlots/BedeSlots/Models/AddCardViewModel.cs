using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BedeSlots.Web.Models
{
    public class AddCardViewModel
    {
        [Required]
        [Display(Name = "Card number")]
        public string CardNumber { get; set; }

        [Required]
        [Display(Name = "CVV")]
        public int Cvv { get; set; }

        [Required]
        [Display(Name = "Expiry")]
        public DateTime Expiry { get; set; }

        [Required]
        [Display(Name = "Card type")]
        public int CardTypeId { get; set; }

        [Required]
        [Display(Name = "Currency")]
        public int CurrencyId { get; set; }

        public List<SelectListItem> CardTypes { get; set; }
        public List<SelectListItem> Currencies { get; set; }

        public AddCardViewModel()
        {

        }
    }
}
