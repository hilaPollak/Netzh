using BE.Models;
using BL.Commands;
using DAL.Accessing;
using DAL.Interface;
using GoogleMapsApi;
using GoogleMapsApi.Entities.Geocoding.Request;
using GoogleMapsApi.Entities.Geocoding.Response;
using GoogleMapsApi.Entities.PlaceAutocomplete.Request;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace BL.ViewModels
{
    public class ReporterViewModel : UIElement
    {
        private Reporter reporter = new Reporter();

        private Signal signal = new Signal();

        public Reporter Reporter
        {
            get { return reporter; }
        }

        public Signal Signal
        {
            get { return signal; }
        }

        private ICommand _AddressChangedCommand;
        private ICommand _AddressSelectedCommand;
        private ICommand _AddReporterCommand;

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

        public ICommand AddReporterCommand
        {
            get
            {
                return _AddReporterCommand ?? (_AddReporterCommand = new RelayCommand(
                    x => addReporter(),
                    x => canAddReporter()));
            }
        }

        public ObservableCollection<string> Addresses { get; set; } = new ObservableCollection<string>();

        private async void addressChanged()
        {
            if (Reporter.ReporterAddress != null && Reporter.ReporterAddress != "" && !Addresses.Contains(Reporter.ReporterAddress))
            {
                try
                {
                    PlaceAutocompleteRequest request = new PlaceAutocompleteRequest();

                    request.ApiKey = "AIzaSyBnocIjKY-nzP_Txj7PG8A5QmwtnEjOP4Y";
                    request.Input = Reporter.ReporterAddress;

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
                geocodingRequest.Address = Reporter.ReporterAddress;
                geocodingRequest.ApiKey = "AIzaSyBnocIjKY-nzP_Txj7PG8A5QmwtnEjOP4Y";
                GeocodingResponse geocode = await GoogleMaps.Geocode.QueryAsync(geocodingRequest);

                if (geocode.Status == Status.OK)
                {
                    IEnumerator<Result> iter = geocode.Results.GetEnumerator();
                    iter.MoveNext();
                    Reporter.LatLongReporterLocation = iter.Current.Geometry.Location.Latitude.ToString() + "," + iter.Current.Geometry.Location.Longitude.ToString();
                }
            }
            catch (System.Exception) { }
        }

        private bool canAddReporter()
        {
            Idal dal = FactoryDal.GetDal();
            bool reporterExists = dal.ReporterIDExists(reporter.ReporterID);
            return 
                reporter.ReporterID != 0 && 
                reporter.ReporterAddress != "" &&
                reporter.ReporterAddress != null &&
                !dal.ReporterIDExists(reporter.ReporterID);
        }

        private void addReporter()
        {
            Idal dal = FactoryDal.GetDal();

            bool success = dal.AddReporter(reporter);
            if (success)
            {
                Signal.SuccessfullyAdded = false;
                Signal.SuccessfullyAdded = true;
                Reporter.ReporterAddress = "";
                Reporter.ReporterID = 0;
                Reporter.ReporterName = "";

                MemoryStream ms = new MemoryStream();
                Image img = Image.FromFile(@"C:\Users\הילה\Desktop\Hits\Hits\blank.png");
                img.Save(ms, img.RawFormat);

                Reporter.ReporterProfilePicture = ms.ToArray();
            }
            else
            {
                Signal.UnsuccessfullyAdded = false;
                Signal.UnsuccessfullyAdded = true;
            }
        }
    }
}
