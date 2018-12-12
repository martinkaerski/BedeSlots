namespace BedeSlots.Services.Data
{
    public static class ServicesConstants
    {
        public const string BaseCurrency = "USD";
        public const string Currencies = "EUR,GBP,BGN,USD";

        public const string ApiBaseAddress = "https://api.exchangeratesapi.io";
        public const string ApiParameters = "/latest?base=" + BaseCurrency + "&symbols=" + Currencies;
    }
}
