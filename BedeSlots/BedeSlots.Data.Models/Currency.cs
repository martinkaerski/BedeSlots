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

        [Required]
        public char Symbol { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
