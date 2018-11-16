using BedeSlots.Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BedeSlots.Web.Models
{
    public class DepositViewModel
    {
        public List<SelectListItem> BankCards { get; set; }
        public int DepositValue { get; set; }
        public BankCard BankCard { get; set; }

        public DepositViewModel()
        {

        }
    }
}
