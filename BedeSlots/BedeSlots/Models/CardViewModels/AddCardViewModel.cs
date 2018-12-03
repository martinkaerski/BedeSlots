﻿using BedeSlots.Common.CustomAttributes;
using BedeSlots.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BedeSlots.Web.Models
{
    public class AddCardViewModel
    {
        public AddCardViewModel()
        {
        }

        [Required]
        [Remote(action: "DoesCardExistInDatabase", controller: "Card", areaName: "")]
        [StringLength(19, MinimumLength =19, ErrorMessage ="The card number should be 16 digits.")]
        [Display(Name = "Card Number")]
        public string CardNumber { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "The cardholder name should be at least 3 symbols.")]
        [Display(Name = "Card Holder")]
        public string CardholderName { get; set; }

        [Required]
        [StringLength(4, MinimumLength = 3, ErrorMessage = "The CVV should be 3 or 4 numbers.")]
        [Display(Name = "CVV")]
        public string Cvv { get; set; }

        [Required]
        [ExpiryDate(ErrorMessage = "Invalid expiry date!")]
        [DisplayFormat(DataFormatString = "{0:MM-yy}")]
        [DataType(DataType.Date)]
        [Display(Name = "Expiry Date")]
        public DateTime Expiry { get; set; }

        [Required]
        [Display(Name = "Card type")]
        public CardType CardType { get; set; }

        public List<SelectListItem> CardTypes { get; set; }
    }
}
