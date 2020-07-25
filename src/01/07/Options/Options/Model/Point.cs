using System.ComponentModel;
using System.Drawing;

namespace Configuration.Model
{
    [TypeConverter(typeof(PointTypeConverter))]
    public class Point
    {
        public double X { get; set; }
        public double Y { get; set; }
    }
}