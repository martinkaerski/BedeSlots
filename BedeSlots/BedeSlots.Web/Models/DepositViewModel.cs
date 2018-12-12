﻿using BedeSlots.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace BedeSlots.Web.Models
{
    public class DepositViewModel
    {
        public DepositViewModel()
        {
        }

        [Required(ErrorMessage = "Please add a card before deposit!")]
        [Range(1, int.MaxValue)]
        public int BankCardId { get; set; }

        [Required(ErrorMessage = "Please enter a positive amount!")]
        [Range(1, WebConstants.MaxAmount, ErrorMessage ="The deposit amount should be more than 1.")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "The deposit amount should be a number.")]
        [Display(Name = "Deposit")]
        public decimal DepositAmount { get; set; }

        public Currency Currency { get; set; }

        public string StatusMessage { get; set; }
    }
}
