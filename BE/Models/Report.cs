namespace BE.Models
{
    using System;
    using System.ComponentModel;

    public class Report : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int reportID;
        private DateTime fallingTime;
        private string address;
        private string details1;
        private string details2;
        private int reporterID;
        private string latLongLocation;
        private int numberOfBooms;
        private int clusterIdNumber;
        private Boolean updated;

        public int ReportID
        {
            get
            {
                return reportID;
            }
            set
            {
                if (value != reportID)
                {
                    reportID = value;
                    raisePropertyChanged("ReportID");
                }
            }
        }

        public DateTime FallingTime
        {
            get
            {
                return fallingTime;
            }
            set
            {
                if (value != fallingTime && fallingTime != null)
                {
                    fallingTime = value;
                    raisePropertyChanged("FallingTime");
                }
            }
        }

        public string Address
        {
            get
            {
                return address;
            }
            set
            {
                if (value != address)
                {
                    address = value;
                    raisePropertyChanged("Address");
                }
            }
        }

        public string Details1
        {
            get
            {
                return details1;
            }
            set
            {
                
                    details1 = value;
                    raisePropertyChanged("Details1");
                
            }
        }

        public string Details2
        {
            get
            {
                return details2;
            }
            set
            {

                details2 = value;
                raisePropertyChanged("Details2");

            }
        }


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

        public string LatLongLocation
        {
            get
            {
                return latLongLocation;
            }
            set
            {
                if (value != latLongLocation)
                {
                    latLongLocation = value;
                    raisePropertyChanged("LatLongLocation");
                }
            }
        }

        public int NumberOfBooms
        {
            get
            {
                return numberOfBooms;
            }
            set
            {
                if (value != numberOfBooms)
                {
                    numberOfBooms = value;
                    raisePropertyChanged("NumberOfBooms");
                }
            }
        }

        public int ClusterIdNumber
        {
            get
            {
                return clusterIdNumber;
            }
            set
            {
                if (value != clusterIdNumber)
                {
                    clusterIdNumber = value;
                    raisePropertyChanged("ClusterIdNumber");
                }
            }
        }

        public Boolean Updated
        {
            get
            {
                return updated;
            }
            set
            {
                if (value != updated)
                {
                    updated = value;
                    raisePropertyChanged("Updated");
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
        /// <summary>
        /// defult constructor
        /// </summary>
        public Report()
        {
            FallingTime = DateTime.Now;
            ClusterIdNumber = -1;
        }
    }
}
