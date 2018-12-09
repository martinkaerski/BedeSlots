using Microsoft.AspNetCore.Mvc;

namespace BedeSlots.Web.Models
{
    public class RetrieveViewModel
    {
        public RetrieveViewModel()
        {
        }

        [Remote(action: "HasEnoughMoney", controller: "Withdraw", areaName: "")]
        public int Amount { get; set; }

        public int BankCardId { get; set; }
    }
}
