using System;
using System.ComponentModel.DataAnnotations;

namespace BedeSlots.Data.Models
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        [Required]
        public TransactionType Type { get; set; }

        [Required]
        [Range(1, double.MaxValue)]
        public double Amount { get; set; }

        public int CardId { get; set; }

        public GameType GameType { get; set; }

        public string UserId { get; set; }

        [Required]
        public User User { get; set; }
    }
}
