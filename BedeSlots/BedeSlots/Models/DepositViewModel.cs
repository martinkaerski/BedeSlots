using BedeSlots.Data.Models;
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
        [Range(1, double.MaxValue, ErrorMessage = "Deposit value must be positive number!")]
        [Display(Name = "Deposit")]
        public decimal DepositAmount { get; set; }

        public Currency Currency { get; set; }
    }
}
