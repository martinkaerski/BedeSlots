using BedeSlots.Data;
using BedeSlots.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BedeSlots.Services.Data
{
    public class DepositService
    {
        private readonly BedeSlotsDbContext context;

        public DepositService(BedeSlotsDbContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }
    }
}
