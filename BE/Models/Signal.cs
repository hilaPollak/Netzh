using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Models
{
    public class Signal : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool successfullyAdded;
        private bool unsuccessfullyAdded;

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
        public bool SuccessfullyAdded
        {
            get
            {
                return successfullyAdded;
            }
            set
            {
                if (value != successfullyAdded)
                {
                    successfullyAdded = value;
                    raisePropertyChanged("SuccessfullyAdded");
                }
            }
        }

        public bool UnsuccessfullyAdded
        {
            get
            {
                return unsuccessfullyAdded;
            }
            set
            {
                if (value != unsuccessfullyAdded)
                {
                    unsuccessfullyAdded = value;
                    raisePropertyChanged("UnsuccessfullyAdded");
                }
            }
        }

       
    }
}
