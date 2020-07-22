using Microsoft.Extensions.Configuration;

namespace Configuration.KeyValue
{
    public class DateTimeFormatOptions
    {
        public DateTimeFormatOptions(IConfiguration configuration)
        {
            LongDatePattern = configuration["LongDatePattern"];
            LongTimePattern = configuration["LongTimePattern"];
            ShortDatePattern = configuration["ShortDatePattern"];
            ShortTimePattern = configuration["ShortTimePattern"];
        }

        public DateTimeFormatOptions()
        {
            
        }

        public string LongDatePattern { get; set; }
        public string LongTimePattern { get; set; }
        public string ShortDatePattern { get; set; }
        public string ShortTimePattern { get; set; }
    }
}