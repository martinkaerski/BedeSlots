using BedeSlots.Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BedeSlots.Web.Models
{
    public class AddCardViewModel
    {
        public string cardNumber { get; set; }
        public int Cvv { get; set; }
        public DateTime Expiry { get; set; }
        public int CardTypeId { get; set; }
        public List<SelectListItem> CardTypes { get; set; }

        public AddCardViewModel()
        {

        }
    }
}
