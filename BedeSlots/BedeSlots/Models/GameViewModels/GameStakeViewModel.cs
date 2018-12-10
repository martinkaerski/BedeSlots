using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BedeSlots.Web.Models.GameViewModels
{
    public class GameStakeViewModel
    {
        [Required]
        [Range(1, WebConstants.MaxAmount, ErrorMessage = "The minimum bet amount is 1!")]
        [Remote(action: "HasEnoughMoneyAsync", controller: "UserBalance", areaName: "")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "The bet amount should be a positive number.")]
        public decimal Amount { get; set; }

        public int Rows { get; set; }

        public int Cols { get; set; }

        public string StatusMessage { get; set; }
    }
}
