using Microsoft.Extensions.Configuration;

namespace Configuration.KeyValue
{
    public class DateTimeFormatOptions
    {
        public string DatePattern { get; set; }
        public string TimePattern { get; set; }

        public override string ToString()
        {
            return $"Date:{DatePattern};Time:{TimePattern}";
        }
    }
}