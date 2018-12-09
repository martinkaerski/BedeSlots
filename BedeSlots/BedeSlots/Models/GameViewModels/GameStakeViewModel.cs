using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BedeSlots.Web.Models.GameViewModels
{
    public class GameStakeViewModel
    {
        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "The minimum bet amount is 1!")]
        [Remote(action: "EnoughMoney", controller: "Game", areaName: "")]
        public decimal Stake { get; set; }

        public int Rows { get; set; }

        public int Cols { get; set; }

        public string StatusMessage { get; set; }
    }
}
