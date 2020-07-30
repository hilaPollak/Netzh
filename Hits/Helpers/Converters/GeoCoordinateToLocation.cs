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
    public class GeoCoordinateToLocation : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            GeoCoordinate geo = value as GeoCoordinate;
            return new Microsoft.Maps.MapControl.WPF.Location(geo.Latitude, geo.Longitude);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
