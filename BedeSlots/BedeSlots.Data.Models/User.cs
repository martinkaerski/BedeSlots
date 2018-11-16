using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BedeSlots.Data.Models
{
    public class User : IdentityUser
    {
        [Required]
        [MinLength(DataModelsConstants.UserNameMinLength)]
        [MaxLength(DataModelsConstants.UserNameMaxLength)]
        public string Name { get; set; }

        [Required]
        public DateTime Birthdate { get; set; }

        public int CurrencyId { get; set; }

        [Required]
        public Currency Currency { get; set; }

        public ICollection<BankCard> Cards { get; set; }

        public ICollection<Transaction> Transactions { get; set; }
    }
}
