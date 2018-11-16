using BedeSlots.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace BedeSlots.Services.Data
{
    public class TransactionService
    {
        private readonly BedeSlotsDbContext context;

        public TransactionService(BedeSlotsDbContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }


    }
}
