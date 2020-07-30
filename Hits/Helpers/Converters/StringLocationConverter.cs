using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Hits.Helpers.Converters
{
    public class StringLocationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value.ToString() != "")
            {
                try
                {
                    string[] latLong = value.ToString().Split(',');
                    return new Location(Double.Parse(latLong[0]), Double.Parse(latLong[1]));
                }
                catch { return new Location(0,0); }
            }
            return new Location(0,0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Location location = value as Location;
            if (location != null)
            {
                return location.Latitude.ToString() + "," + location.Longitude.ToString();
            }
            return "31.771959,35.217018";
        }
    }
}
