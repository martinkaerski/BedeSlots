using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BedeSlots.Web.Models
{
    public class RetrieveViewModel
    {
        [Remote(action: "HasEnoughMoney", controller: "Withdraw", areaName: "")]
        public int RetrieveAmount { get; set; }
        public int BankCardId { get; set; }

        public RetrieveViewModel()
        {
            
        }
    }
}
