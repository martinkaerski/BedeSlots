using BedeSlots.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace BedeSlots.Web.Areas.Admin.Models
{
    public class UserListingViewModel
    {
        public ICollection<UserDto> Users { get; set; }

        public ICollection<SelectListItem> Roles { get; set; }

        //[Display(Name = "Username")]
        //public string Username { get; set; }
        //[Display(Name = "Email")]
        //public string Email { get; set; }
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        //public decimal Balance { get; set; }
        //public string Currency { get; set; }
        //public IEnumerable<BankCard> Cards { get; set; }
        //public IEnumerable<Transaction> Transactions { get; set; }
        //public string Role { get; set; }

        //public UserListingViewModel()
        //{

        //}

    }
}
