using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Models
{
    public class Reporter : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int reporterID;
        private string reporterName;
        private string reporterAddress;
        private string latLongReporterLocation;
        private byte[] reporterProfilePicture;

        public int ReporterID
        {
            get
            {
                return reporterID;
            }
            set
            {
                if (value != reporterID)
                {
                    reporterID = value;
                    raisePropertyChanged("ReporterID");
                }
            }
        }

        public string ReporterName
        {
            get
            {
                return reporterName;
            }
            set
            {
                if (value != reporterName)
                {
                    reporterName = value;
                    raisePropertyChanged("ReporterName");
                }
            }
        }

        public string ReporterAddress
        {
            get
            {
                return reporterAddress;
            }
            set
            {
                if (value != reporterAddress)
                {
                    reporterAddress = value;
                    raisePropertyChanged("ReporterAddress");
                }
            }
        }

        public string LatLongReporterLocation
        {
            get
            {
                return latLongReporterLocation;
            }
            set
            {
                if (value != latLongReporterLocation)
                {
                    latLongReporterLocation = value;
                    raisePropertyChanged("LatLongReporterLocation");
                }
            }
        }

        public byte[] ReporterProfilePicture
        {
            get
            {
                return reporterProfilePicture;
            }
            set
            {
                if (value != reporterProfilePicture)
                {
                    reporterProfilePicture = value;
                    raisePropertyChanged("ReporterProfilePicture");
                }
            }
        }

        /// <summary>
        /// this func notify and send the event
        /// </summary>
        /// <param name="property"> the string of the property that changed</param>
        private void raisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
