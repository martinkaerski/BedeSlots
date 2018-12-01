using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BedeSlots.Web.Models
{
    public class GameSlotViewModel
    {
        public int Rows { get; set; }

        public int Cols { get; set; }

        public string[,] Matrix { get; set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "The minimum bet amount is 1 unit!")]
        [Remote(action: "EnoughMoney", controller: "Game", areaName: "")]
        public decimal Stake { get; set; }

        public decimal Balance { get; set; }

        public string Message { get; set; }
    }
}
