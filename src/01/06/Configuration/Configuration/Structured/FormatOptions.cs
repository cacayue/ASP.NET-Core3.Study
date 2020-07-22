using Configuration.KeyValue;
using Microsoft.Extensions.Configuration;

namespace Configuration.Structured
{
    public class FormatOptions
    {
        public DateTimeFormatOptions DateTime { get; set; }
        public CurrencyDecimalFormatOptions CurrencyDecimal { get; set; }

        public FormatOptions()
        {
            
        }

        public FormatOptions(IConfiguration configuration)
        {
            DateTime = new DateTimeFormatOptions(
                configuration.GetSection("DateTime"));
            CurrencyDecimal = new CurrencyDecimalFormatOptions(
                configuration.GetSection("CurrencyDecimal"));
        }
    }
}