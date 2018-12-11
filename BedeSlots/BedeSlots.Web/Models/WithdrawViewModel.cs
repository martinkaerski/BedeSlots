using BedeSlots.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BedeSlots.Web.Models
{
    public class WithdrawViewModel
    {
        public WithdrawViewModel()
        {
        }

        [Range(1, WebConstants.MaxAmount, ErrorMessage = "The withdraw amount should be more than 1.")]
        [Remote(action: "HasEnoughMoneyAsync", controller: "UserBalance", areaName: "")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "The withdraw amount should be a positive number.")]
        public int Amount { get; set; }

        public int BankCardId { get; set; }

        public Currency Currency { get; set; }

        public string StatusMessage { get; set; }
    }
}
