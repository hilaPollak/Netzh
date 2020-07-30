using BE.Models;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Function_and_algorithms
{
    class KMeans
    {

        public List<Report> ReportsList { get; set; }
        public int K { get; set; }
        static Random rand = new Random();

        public KMeans(List<Report> reportsList, int k)
        {
            ReportsList = reportsList;
            K = k;
        }

        public List<GeoCoordinate> K_Means()
        {
            if (ReportsList.Count == 0)
                return null;

            List<GeoCoordinate> clustersIdList = ClustersGenerator();

            bool isChanged;
            do
            {
                isChanged = false;

                //for each report looking for the closest cluster 
                for (int i = 0; i < ReportsList.Count; i++)
                {
                    double min = Double.MaxValue;

                    for (int j = 0; j < clustersIdList.Count; j++)
                    {
                        double temp = ReportsList[i].GetCoordinate().GetDistanceTo(clustersIdList[j]);
                        if (temp < min)
                        {
                            min = temp;
                            ReportsList[i].ClusterIdNumber = j;
                        }
                    }
                }

                //Recenter the clusters
                ReportsList.OrderBy(c => c.ClusterIdNumber);
                double clustersLongitudeSum = 0;
                double clustersLatitudeSum = 0;
                double oldLat = 0, oldLong = 0;
                int counter = 0;
                for (int id = 0; id < clustersIdList.Count; id++)
                {
                    for (int i = 0; i < ReportsList.Count; i++)
                    {
                        if (ReportsList[i].ClusterIdNumber == id)
                        {
                            clustersLatitudeSum += ReportsList[i].GetCoordinate().Latitude;
                            clustersLongitudeSum += ReportsList[i].GetCoordinate().Longitude;
                            counter++;
                        }
                    }
                    oldLat = clustersIdList[id].Latitude;
                    oldLong = clustersIdList[id].Longitude;
                    clustersIdList[id].Latitude = counter == 0 ? 0 : clustersLatitudeSum / counter;
                    clustersIdList[id].Longitude = counter == 0 ? 0 : clustersLongitudeSum / counter;
                    if (clustersIdList[id].Longitude != oldLong || clustersIdList[id].Latitude != oldLat)
                    {
                        isChanged = true;
                    }
                    clustersLongitudeSum = 0;
                    clustersLatitudeSum = 0;
                    counter = 0;
                }
            } while (isChanged);

            return clustersIdList;
        }

        private List<GeoCoordinate> ClustersGenerator()
        {
            List<GeoCoordinate> clustersIdList = new List<GeoCoordinate>();

            double minLatitude = ReportsList.Min(r => r.GetCoordinate().Latitude);
            double maxLatitude = ReportsList.Max(r => r.GetCoordinate().Latitude);
            double minLongitude = ReportsList.Min(r => r.GetCoordinate().Longitude);
            double maxLongitude = ReportsList.Max(r => r.GetCoordinate().Longitude);

            for (int i = 0; i < K; i++)
            {
                double latitude = minLatitude + rand.NextDouble() * (maxLatitude - minLatitude);
                double longitude = minLongitude + rand.NextDouble() * (maxLongitude - minLongitude);
                GeoCoordinate coordinate = new GeoCoordinate(latitude, longitude);
                clustersIdList.Add(coordinate);
            }

            return clustersIdList;
        }
    }
}
