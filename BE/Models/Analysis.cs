using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Models
{
    public class Analysis : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string selectedArea;
        private string selectedAreaLocation;
        private DateTime selectedDate = DateTime.Now;

        public string SelectedArea
        {
            get
            {
                return selectedArea;
            }
            set
            {
                if (value != selectedArea)
                {
                    selectedArea = value;
                    raisePropertyChanged("SelectedArea");
                }
            }
        }

        public string SelectedAreaLocation
        {
            get
            {
                return selectedAreaLocation;
            }
            set
            {
                if (value != selectedAreaLocation)
                {
                    selectedAreaLocation = value;
                    raisePropertyChanged("SelectedAreaLocation");
                }
            }
        }

        public DateTime SelectedDate
        {
            get
            {
                return selectedDate;
            }
            set
            {
                if (value != selectedDate)
                {
                    selectedDate = value;
                    raisePropertyChanged("SelectedDate");
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
