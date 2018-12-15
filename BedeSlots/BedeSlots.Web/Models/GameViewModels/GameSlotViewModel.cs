﻿using BedeSlots.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BedeSlots.Web.Models
{
    public class GameSlotViewModel
    {
        public string GameName { get; set; }

        [Required]
        public int Rows { get; set; }

        [Required]
        public int Cols { get; set; }

        [Required]
        public string[,] Matrix { get; set; }

        [Required]
        [Range(1, WebConstants.MaxAmount, ErrorMessage = "The minimum bet amount is 1!")]
        [RegularExpression(@"\d*\.?\d*", ErrorMessage = "The bet amount should be a number using dot for floating-point numbers.")]
        [Remote(action: "HasEnoughMoneyAsync", controller: "UserBalance", areaName: "")]
        public decimal Amount { get; set; }

        [Required]
        public decimal Balance { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        public Currency Currency { get; set; }

        [Required]
        public string WinningRows { get; set; }

        [Required]
        public double Coefficient { get; set; }

        public string StatusMessage { get; set; }
    }
}
