using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.ComponentModel;

namespace Bindable.Linq.Samples.WindowsForms
{
    public class ProcessWrapper : INotifyPropertyChanged
    {
        private string _processName;
        private int _id;
        private string _title;

        public string ProcessName
        {
            get { return _processName; }
            set
            {
                _processName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProcessName"));
            }
        }

        public int ID
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ID"));
            }
        }

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Title"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
