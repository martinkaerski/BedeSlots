using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BedeSlots.Web.Models
{
    public class SelectCardViewModel
    {

        [Display(Name = "Select card")]
        public List<SelectListItem> CardsList { get; set; }

        [Required(ErrorMessage = "Please add a card before deposit!")]
        [Range(1, int.MaxValue)]
        public int BankCardId { get; set; }

        public SelectCardViewModel()
        {

        }
    }
}
