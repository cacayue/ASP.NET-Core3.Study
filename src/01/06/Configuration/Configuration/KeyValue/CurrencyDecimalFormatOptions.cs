using Microsoft.Extensions.Configuration;

namespace Configuration.KeyValue
{
    public class CurrencyDecimalFormatOptions
    {
        public int Digits { get; set; }
        public string Symbol { get; set; }

        public CurrencyDecimalFormatOptions(IConfiguration configuration)
        {
            Digits = int.Parse(configuration["Digits"]);
            Symbol = configuration["Symbol"];
        }

        public CurrencyDecimalFormatOptions()
        {
            
        }
    }
}