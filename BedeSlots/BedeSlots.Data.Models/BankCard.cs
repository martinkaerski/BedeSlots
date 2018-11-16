using System;
using System.ComponentModel.DataAnnotations;

namespace BedeSlots.Data.Models
{
    public class BankCard : Entity
    {
        [Required]
        [RegularExpression("^[0-9]{16}$", ErrorMessage = "The card number should be 16 digits.")]
        public string Number { get; set; }

        [Required]
        public int CvvNumber { get; set; }

        [Required]
        public DateTime ExpiryDate { get; set; }

        public int TypeId { get; set; }

        public CardType Type { get; set; }

        public string UserId { get; set; }

        [Required]
        public User User { get; set; }
    }
}
