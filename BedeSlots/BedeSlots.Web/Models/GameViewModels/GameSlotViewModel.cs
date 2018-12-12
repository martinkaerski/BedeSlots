using BedeSlots.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BedeSlots.Web.Models
{
    public class GameSlotViewModel
    {
        [Required]
        public int Rows { get; set; }

        [Required]
        public int Cols { get; set; }

        [Required]
        public string[,] Matrix { get; set; }

        [Required]
        [Range(1, WebConstants.MaxAmount, ErrorMessage = "The minimum bet amount is 1!")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "The bet amount should be a positive number.")]
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
        public decimal Coefficient { get; set; }

        public string StatusMessage { get; set; }
    }
}
