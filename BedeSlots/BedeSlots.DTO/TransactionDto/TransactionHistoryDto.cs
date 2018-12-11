﻿using BedeSlots.Data.Models;
using System;

namespace BedeSlots.DTO.TransactionDto
{
    public class TransactionHistoryDto
    {
        public DateTime Date { get; set; }

        public TransactionType Type { get; set; }

        public decimal Amount { get; set; }

        public string Description { get; set; }
    }
}
