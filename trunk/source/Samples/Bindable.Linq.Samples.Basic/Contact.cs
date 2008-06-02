namespace Bindable.Linq.SampleApplication
{
    using System.ComponentModel;

    public class Contact : INotifyPropertyChanged
    {
        private string _company;
        private string _name;
        private string _phoneNumber;

        public Contact() {}

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Name"));
            }
        }

        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set
            {
                _phoneNumber = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PhoneNumber"));
            }
        }

        public string Company
        {
            get { return _company; }
            set
            {
                _company = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Company"));
            }
        }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
    }
}