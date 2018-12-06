using BedeSlots.Data.Models;
using System;

namespace BedeSlots.DTO
{
   public class TransactionDto
    {
        public DateTime Date { get; set; }

        public TransactionType Type { get; set; }

        public decimal Amount { get; set; }

        public string Description { get; set; }

        public string User { get; set; }
    }
}
