using System;

namespace BedeSlots.Web.Providers.Contracts
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }
    }
}