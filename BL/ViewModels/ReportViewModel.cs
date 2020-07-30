using BE.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Configuration;
using System.Collections.Specialized;
using System.Collections.Generic;
using GoogleMapsApi.Entities.PlaceAutocomplete.Request;
using GoogleMapsApi;
using BL.Commands;
using GoogleMapsApi.Entities.Geocoding.Request;
using GoogleMapsApi.Entities.Geocoding.Response;
using GoogleMapsApi.Entities.Common;
using System.Windows;
using System.Diagnostics;
using DAL.Interface;
using DAL.Accessing;
using System;

namespace BL.ViewModels
{
    public class ReportViewModel
    {
        private Report report = new Report();
        private Signal signal = new Signal();

        public Report Report
        {
            get { return report; }
        }

        public Signal Signal
        {
            get { return signal; }
        }

        private ICommand _AddressChangedCommand;
        private ICommand _AddressSelectedCommand;
        private ICommand _SendReportCommand;

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

        public ICommand SendReportCommand
        {
            get
            {
                return _SendReportCommand ?? (_SendReportCommand = new RelayCommand(
                    x => sendReport(), 
                    x => canSendReport()));
            }
        }

        public ObservableCollection<string> Addresses { get; set; } = new ObservableCollection<string>();

        private async void addressChanged()
        {
            if (Report.Address != null && Report.Address != "" && !Addresses.Contains(Report.Address))
            {
                try
                {
                    PlaceAutocompleteRequest request = new PlaceAutocompleteRequest();

                    request.ApiKey = "AIzaSyBnocIjKY-nzP_Txj7PG8A5QmwtnEjOP4Y";
                    request.Input = Report.Address;

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
                geocodingRequest.Address = Report.Address;
                geocodingRequest.ApiKey = "AIzaSyBnocIjKY-nzP_Txj7PG8A5QmwtnEjOP4Y";
                GeocodingResponse geocode = await GoogleMaps.Geocode.QueryAsync(geocodingRequest);

                if (geocode.Status == Status.OK)
                {
                    IEnumerator<Result> iter = geocode.Results.GetEnumerator();
                    iter.MoveNext();
                    Report.LatLongLocation = iter.Current.Geometry.Location.Latitude.ToString() + "," + iter.Current.Geometry.Location.Longitude.ToString();
                }
            }
            catch (System.Exception) { }
        }

        private bool canSendReport()
        {
            Idal dal = FactoryDal.GetDal();
            return
                report.NumberOfBooms != 0 &&
                report.FallingTime != null &&
                report.LatLongLocation != null &&
                report.ReportID != 0 &&
                !dal.ReportIDExists(report.ReportID) &&
                dal.ReporterIDExists(report.ReporterID);
        }
        private void sendReport()
        {
            Idal dal = FactoryDal.GetDal();
            bool success = dal.AddReport(report);
            if (success)
            {
                Signal.SuccessfullyAdded = false;
                Signal.SuccessfullyAdded = true;
                Report.ReporterID = 0;
                Report.ReportID = 0;
                Report.NumberOfBooms = 0;
                Report.LatLongLocation = "0,0";
                Report.Address = "";
                Report.FallingTime = DateTime.Now;
            }
            else
            {
                Signal.UnsuccessfullyAdded = false;
                Signal.UnsuccessfullyAdded = true;
            }
        }
    }
}