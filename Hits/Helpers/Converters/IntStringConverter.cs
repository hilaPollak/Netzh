using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Hits.Helpers.Converters
{
    public class IntStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && (int)value != 0)
            {
                return value.ToString();
            }
            else return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int res;
            if (int.TryParse(value.ToString(), out res))
            {
                return res;
            }
            else return 0;
        }
    }
}
