using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BedeSlots.Data.Models
{
    public class CurrencyNew
    {
        //TODO: Delete it
        public CurrencyNew()
        {
            this.BaseCurrency = "USD";

        }

        public string BaseCurrency { get; set; }

        public IDictionary<Currency, double> Rates { get; set; }
    }
}
