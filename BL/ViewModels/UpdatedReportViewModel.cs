using BE.Models;
using BL.Commands;
using DAL.Accessing;
using DAL.Interface;
using GoogleMapsApi;
using GoogleMapsApi.Entities.Geocoding.Request;
using GoogleMapsApi.Entities.Geocoding.Response;
using GoogleMapsApi.Entities.PlaceAutocomplete.Request;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BL.ViewModels
{
    public class UpdatedReportViewModel
    {
        private UpdatedReport updatedReport = new UpdatedReport();
        private Report oldReport = new Report();
        private Signal signal = new Signal();
        private Analysis analysis = new Analysis();
       

        public UpdatedReport UpdatedReport
        {
            get { return updatedReport; }
        }
        public Signal Signal
        {
            get { return signal; }
        }
        public Report OldReport
        {
            get { return oldReport; }
        }
        public Analysis Analysis
        {
            get { return analysis; }
        }

        public ObservableCollection<Report> ShownReports { get; set; } = new ObservableCollection<Report>();
        public ObservableCollection<string> Addresses { get; set; } = new ObservableCollection<string>();

        private ICommand _FetchReportCommand;
        private ICommand _AddUpdatedReportCommand;
        private ICommand _DateChangedCommand;
        private ICommand _ShowReportCommand;
       

        public ICommand FetchReportCommand
        {
            get
            {
                return _FetchReportCommand ?? (_FetchReportCommand = new RelayCommand(
                    x => fetchReport(),
                    x => canFetchReport()
                    ));
            }
        }
        public ICommand AddUpdatedReportCommand
        {
            get
            {
                return _AddUpdatedReportCommand ?? (_AddUpdatedReportCommand = new RelayCommand(
                    x => addUpdatedreport()
                    ));
            }
        }
        public ICommand DateChangedCommand
        {
            get
            {
                return _DateChangedCommand ?? (_DateChangedCommand = new RelayCommand(
                    x => updateShownReportsByDate()
                    ));
            }
        }
        public ICommand ShowReportCommand
        {
            get
            {
                return _ShowReportCommand ?? (_ShowReportCommand = new RelayCommand(
                    x => ShownReportslist()
                    ));
            }
        }
        
        public UpdatedReportViewModel()
        {
            updateShownReportsByDate();
            ShownReportslist();
        }

        private void updateShownReportsByDate()
        {
            ShownReports.Clear();
            foreach (Report report in FactoryDal.GetDal().GetReports())
            {
                if (report.FallingTime.Date == Analysis.SelectedDate.Date)
                {
                    ShownReports.Add(report);
                }
            }
        }
        private void ShownReportslist()
        {
            ShownReports.Clear();
            foreach (Report report in FactoryDal.GetDal().GetReports())
            {
               
                    ShownReports.Add(report);
                
            }
        }
        private bool canFetchReport()
        {
            return FactoryDal.GetDal().ReportIDExists(updatedReport.ReportID);
        }

        private void fetchReport()
        {
            Idal dal = FactoryDal.GetDal();
            oldReport = dal.GetReport(updatedReport.ReportID);

            UpdatedReport.NewCoordinates = oldReport.LatLongLocation;
            UpdatedReport.NewTime = oldReport.FallingTime;
            UpdatedReport.NumberOfHits = oldReport.NumberOfBooms;
        }

        private void addUpdatedreport()
        {
            Idal dal = FactoryDal.GetDal();
            if (dal.AddUpdatedReport(UpdatedReport))
            {
                Signal.SuccessfullyAdded = false;
                Signal.SuccessfullyAdded = true;
                UpdatedReport.NewCoordinates = "";
                UpdatedReport.NewTime = DateTime.Now;
                UpdatedReport.NumberOfHits = 0;
                UpdatedReport.ReportID = 0;
            }
            else
            {
                Signal.UnsuccessfullyAdded = false;
                Signal.UnsuccessfullyAdded = true;
            }
        }
    }
}
