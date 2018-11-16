using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BedeSlots.Data.Models
{
    public class CardType : Entity
    {
        [Required]
        public string Name { get; set; }

        public ICollection<BankCard> Cards { get; set; }
    }
}
