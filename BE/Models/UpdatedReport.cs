namespace BE.Models
{
    using System;
    using System.ComponentModel;

    public class UpdatedReport : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int reportID;
        private string newCoordinates;
        private DateTime newTime;
        private int numberOfHits;

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

        public string NewCoordinates
        {
            get
            {
                return newCoordinates;
            }
            set
            {
                if (value != newCoordinates)
                {
                    newCoordinates = value;
                    raisePropertyChanged("NewCoordinates");
                }
            }
        }

        public DateTime NewTime
        {
            get
            {
                return newTime;
            }
            set
            {
                if (value != newTime)
                {
                    newTime = value;
                    raisePropertyChanged("NewTime");
                }
            }
        }

        public int NumberOfHits
        {
            get
            {
                return numberOfHits;
            }
            set
            {
                if (value != numberOfHits)
                {
                    numberOfHits = value;
                    raisePropertyChanged("NumberOfHits");
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
