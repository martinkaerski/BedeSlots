using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BedeSlots.Data.Models
{
    public class Currency : Entity
    {
        [Required]
        [StringLength(DataModelsConstants.CurrencyNameLength,
            MinimumLength = DataModelsConstants.CurrencyNameLength)]
        public string Name { get; set; }

        public string Symbol { get; set; }

        public ICollection<User> Users { get; set; }

        public ICollection<BankCard> Cards { get; set; }
    }
}
