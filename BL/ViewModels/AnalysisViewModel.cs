using BE.Models;
using BL.Commands;
using BL.Function_and_algorithms;
using DAL.Accessing;
using DAL.Interface;
using GoogleMapsApi;
using GoogleMapsApi.Entities.Geocoding.Request;
using GoogleMapsApi.Entities.Geocoding.Response;
using GoogleMapsApi.Entities.PlaceAutocomplete.Request;
using GoogleMapsApi.Entities.PlacesDetails;
using LiveCharts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Device.Location;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BL.ViewModels
{
    public class GoogleGeoCodeResponse
    {

        public string status { get; set; }
        public results[] results { get; set; }

    }

    public class results
    {
        public string formatted_address { get; set; }
        public geometry geometry { get; set; }
        public string[] types { get; set; }
        public address_component[] address_components { get; set; }
    }

    public class geometry
    {
        public string location_type { get; set; }
        public location location { get; set; }
    }

    public class location
    {
        public string lat { get; set; }
        public string lng { get; set; }
    }

    public class address_component
    {
        public string long_name { get; set; }
        public string short_name { get; set; }
        public string[] types { get; set; }
    }
    public class AnalysisViewModel
    {
        private Analysis analysis = new Analysis();
        private Report selectedReport = new Report();
        private Reporter reporter = new Reporter();
        private string details1;
        public string Details1
        {
            get { return details1; }
            set { details1 = value; }
        }
        private string details2;
        public string Details2
        {
            get { return details2; }
            set { details2 = value; }
        }

        public Analysis Analysis
        {
            get { return analysis; }
        }
        public Report SelectedReport
        {
            get { return selectedReport; }
        }
        public Reporter Reporter
        {
            get { return reporter; }
        }
        public IChartValues GraphValues { get; set; } = new ChartValues<double>();
        public Func<double, string> Formatter { get; set; }
        public string[] Labels { get; set; }

        public AnalysisViewModel()
        {
          
            fillReports();
            Formatter = value => value + "KM";
            Labels = new[] { "Distance" };
            
        }

        private ICommand _AddressChangedCommand;
        private ICommand _AddressSelectedCommand;
        private ICommand _ShowReportDetailsCommand;
        private ICommand _ShowReportsCommand;
        private ICommand _ShowAllAreasCommand;
        private ICommand _FindKMeansListCommand;
        private ICommand _ShowStatisticsCommand;

        public ICommand AddressChangedCommand
        {
            get
            {
                return _AddressChangedCommand ?? (_AddressChangedCommand = new RelayCommand(
                    x =>
                    {
                        addressChanged();
                    }));
            }
        }
        public ICommand AddressSelectedCommand
        {
            get
            {
                return _AddressSelectedCommand ?? (_AddressSelectedCommand = new RelayCommand(
                    x =>
                    {
                        addressSelected();
                    }));
            }
        }
        public ICommand ShowReportDetailsCommand
        {
            get
            {
                return _ShowReportDetailsCommand ?? (_ShowReportDetailsCommand = new RelayCommand(
                    x => fetchReport((int)x)
                    ));
            }
        }
        public ICommand ShowReportsCommand
        {
            get
            {
                return _ShowReportsCommand ?? (_ShowReportsCommand = new RelayCommand(
                    x => showReports(),
                    x => canShowReports()
                    ));
            }
        }
        public ICommand ShowAllAreasCommand
        {
            get
            {
                return _ShowAllAreasCommand ?? (_ShowAllAreasCommand = new RelayCommand(
                    x => showAllReports(),
                    x => canShowAllReports()
                    ));
            }
        }
        public ICommand FindKMeansListCommand
        {
            get
            {
                return _FindKMeansListCommand ?? (_FindKMeansListCommand = new RelayCommand(
                    x => showKMeansResult(),
                    x => canShowKMeansResult()
                    ));
            }
        }
        public ICommand ShowStatisticsCommand
        {
            get
            {
                return _ShowStatisticsCommand ?? (_ShowStatisticsCommand = new RelayCommand(
                    x => showStatistics(),
                    x => canShowStatistics()
                    ));
            }
        }

        private bool canShowStatistics()
        {
            return StatisticalRelocations.Count != 0;
        }

        private void showStatistics()
        {
            GraphValues.Clear();


            double tempDouble, maxValue = 0;
            for (int i = 0; i < StatisticalRelocations.Count; i++)
            {
                foreach (Report report in ShownReports)
                {
                    if (report.ClusterIdNumber == i)
                    {
                        tempDouble = report.GetCoordinate().GetDistanceTo(StatisticalRelocation[i]);
                        if (tempDouble > maxValue)
                        {
                            maxValue = tempDouble / 1000;
                        }
                    }
                }
                GraphValues.Add(maxValue);
            }


        }

        private bool canShowKMeansResult()
        {
            return ShownReports.Count != 0;
        }
        
       
        private void showKMeansResult()
        {
            KMeans km = new KMeans(ShownReports.ToList(), 3);
            List<GeoCoordinate> kmeansLocations = km.K_Means();
            StatisticalRelocations.Clear();
            foreach (GeoCoordinate geoCoordinate in kmeansLocations)
            {
                StatisticalRelocation.Add(geoCoordinate);
                StatisticalRelocations.Add(geoCoordinate.Latitude.ToString() + "," + geoCoordinate.Longitude.ToString());
            }
             // showStatistics();
        }

        public ObservableCollection<string> Addresses { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<Report> ShownReports { get; set; } = new ObservableCollection<Report>();
        public ObservableCollection<GeoCoordinate> StatisticalRelocation { get; set; } = new ObservableCollection<GeoCoordinate>();
        public ObservableCollection<string> StatisticalRelocations { get; set; } = new ObservableCollection<string>();
        private List<Report> tempReportsToShow = new List<Report>();

        private async void addressChanged()
        {
            if (Analysis.SelectedArea != null && Analysis.SelectedArea != "" && !Addresses.Contains(Analysis.SelectedArea))
            {
                try
                {
                    PlaceAutocompleteRequest request = new PlaceAutocompleteRequest();

                    request.ApiKey = "AIzaSyBnocIjKY-nzP_Txj7PG8A5QmwtnEjOP4Y";
                    request.Input = Analysis.SelectedArea;

                    var response = await GoogleMaps.PlaceAutocomplete.QueryAsync(request);

                    Addresses.Clear();
                    foreach (var item in response.Results)
                    {
                        Addresses.Add(item.Description);
                    }
                }
                catch (System.Exception) { }
            }
        }

        private async void addressSelected()
        {
            try
            {
                GeocodingRequest geocodingRequest = new GeocodingRequest();
                geocodingRequest.Address = Analysis.SelectedArea;
                geocodingRequest.ApiKey = "AIzaSyBnocIjKY-nzP_Txj7PG8A5QmwtnEjOP4Y";
                GeocodingResponse geocode = await GoogleMaps.Geocode.QueryAsync(geocodingRequest);

                if (geocode.Status == Status.OK)
                {
                    IEnumerator<Result> iter = geocode.Results.GetEnumerator();
                    iter.MoveNext();
                    Analysis.SelectedAreaLocation = iter.Current.Geometry.Location.Latitude.ToString() + "," + iter.Current.Geometry.Location.Longitude.ToString();
                }
            }
            catch (System.Exception) { }
        }

        private void fetchReport(int reportId)
        {
            //showKMeansResult();
            
            foreach (Report rep in ShownReports)
            {
                if (rep.ReportID == reportId)
                {
                    SelectedReport.Address = rep.Address;
                    SelectedReport.FallingTime = rep.FallingTime;
                    SelectedReport.LatLongLocation = rep.LatLongLocation;
                    SelectedReport.NumberOfBooms = rep.NumberOfBooms;
                    SelectedReport.ReporterID = rep.ReporterID;
                    SelectedReport.ReportID = rep.ReportID;
                    SelectedReport.Updated = rep.Updated;
                    
                    

                    Reporter tempRep = FactoryDal.GetDal().GetReporter(SelectedReport.ReporterID);

                    Reporter.LatLongReporterLocation = tempRep.LatLongReporterLocation;
                    Reporter.ReporterAddress = tempRep.ReporterAddress;
                    Reporter.ReporterID = tempRep.ReporterID;
                    Reporter.ReporterName = tempRep.ReporterName;
                    Reporter.ReporterProfilePicture = tempRep.ReporterProfilePicture;

                    GraphValues.Clear();
                   

                        double distance = 0;
                        for (int i = 0; i < StatisticalRelocations.Count; i++)
                        {
                            if (rep.ClusterIdNumber == i)

                        {
                            try
                            {
                                Idal dal = FactoryDal.GetDal();
                                UpdatedReport upReport = dal.GetUpdatedReport(rep.ReportID);
                                double lon = getLong(upReport.NewCoordinates);
                                double lat = getLat(upReport.NewCoordinates);
                                GeoCoordinate g = new GeoCoordinate(lat, lon);

                                distance = g.GetDistanceTo(StatisticalRelocation[i]) / 1000;

                                string[] latLong = upReport.NewCoordinates.ToString().Split(',');

                                Details1 = ReverseGeoCoding(latLong[0], latLong[1]);

                                string[] latLong2 = StatisticalRelocations[i].ToString().Split(',');

                                Details2 = ReverseGeoCoding(latLong2[0], latLong2[1]);

                                SelectedReport.Details1 = details1;
                                SelectedReport.Details2 = details2;
                            }
                            catch (Exception)
                            {
                                MessageBox.Show("The report hasn't been updated, \n please update the report first!");
                                //throw new Exception("The report not updated \n please update the report first");
                            }

                        }
                        }
                        GraphValues.Add(distance);
                        GraphValues.Add(distance);
                    
                }
            }
        }

        private Boolean canShowReports()
        {
            return Analysis.SelectedDate != null && Analysis.SelectedArea != "" && Analysis.SelectedArea != null;
        }

        private void showReports()
        {
            fillReports();
            ShownReports.Clear();
            double lat = getLat(Analysis.SelectedAreaLocation), lon = getLong(Analysis.SelectedAreaLocation), tempLat, tempLong;
            foreach (Report report in tempReportsToShow)
            {
                tempLat = getLat(report.LatLongLocation);
                tempLong = getLong(report.LatLongLocation);
                if ((Math.Abs(tempLong - lon) <= 1.25 && Math.Abs(tempLat - lat) <= 1.25) && (report.FallingTime.Date == analysis.SelectedDate.Date))
                {
                    ShownReports.Add(report);
                }
            }

        }

        private bool canShowAllReports()
        {
            return Analysis.SelectedDate != null;
        }

        private void showAllReports()
        {
            fillReports();
            ShownReports.Clear();
            foreach (Report report in tempReportsToShow)
            {
                if (report.FallingTime.Date == analysis.SelectedDate.Date)
                {
                    ShownReports.Add(report);
                }
            }
        }

        private void fillReports()
        {
            tempReportsToShow.Clear();
            foreach (Report report in FactoryDal.GetDal().GetReports())
            {
                tempReportsToShow.Add(report);
            }
            
        }

        private double getLat(string location)
        {
            string[] latLong = location.ToString().Split(',');
            return Double.Parse(latLong[0]);
        }

        private double getLong(string location)
        {
            string[] latLong = location.ToString().Split(',');
            return Double.Parse(latLong[1]);
        }

        private ICommand _ShowDetailsCommand;
        public ICommand ShowDetailsCommand
        {
            get
            {
                return _ShowDetailsCommand ?? (_ShowDetailsCommand = new RelayCommand(
                    x => ShowDetails()
                    ));
            }
        }


        private void ShowDetails()
        {
            
        }



        const string apiKey = "AIzaSyBnocIjKY-nzP_Txj7PG8A5QmwtnEjOP4Y"; //!!!!paste your API KEY HERE!!!!
        static string baseUrlRGC = "https://maps.googleapis.com/maps/api/geocode/json?latlng="; // part1 of the URL for Reverse GeoCoding
        static string plusUrl = "&key=" + apiKey + "&sensor=false"; // part2 of the URL

        static string ReverseGeoCoding(string latitude, string longitude)
        {
            var json = new WebClient().DownloadString(baseUrlRGC + latitude.Replace(" ", "") + ","
                + longitude.Replace(" ", "") + plusUrl);//concatenate URL with the input lat/lng and downloads the requested resource
            GoogleGeoCodeResponse jsonResult = JsonConvert.DeserializeObject<GoogleGeoCodeResponse>(json); //deserializing the result to GoogleGeoCodeResponse

            string status = jsonResult.status; // get status

            string geoLocation = String.Empty;

            if (status == "OK") //check if status is OK
            {
               
                geoLocation = jsonResult.results[0].formatted_address ; //append the result addresses to every new line
                
                return geoLocation; //return result
            }
            else
            {
                return status; //return status / error if not OK
            }
        }
    }
    
}
