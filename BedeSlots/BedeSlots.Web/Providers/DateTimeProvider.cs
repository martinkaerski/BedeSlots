using BedeSlots.Web.Providers.Contracts;
using System;

namespace BedeSlots.Web.Providers
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public virtual DateTime Now => DateTime.Now;
    }
}
