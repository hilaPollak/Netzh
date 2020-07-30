using BE.Models;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Function_and_algorithms
{
    public static class ReportExtention
    {
        public static GeoCoordinate GetCoordinate(this Report report)
        {
            string[] strLatLong = report.LatLongLocation.Split(',');
            return new GeoCoordinate(Double.Parse(strLatLong[0]), Double.Parse(strLatLong[1]));
        }
    }
}
