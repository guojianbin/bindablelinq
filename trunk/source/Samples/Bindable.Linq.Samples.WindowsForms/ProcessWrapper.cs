namespace Bindable.Linq.Samples.WindowsForms
{
    using System.ComponentModel;

    public class ProcessWrapper : INotifyPropertyChanged
    {
        private int _id;
        private string _processName;
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

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        private void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}