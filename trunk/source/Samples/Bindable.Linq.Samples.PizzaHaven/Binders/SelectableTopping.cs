namespace Bindable.Linq.Samples.PizzaHaven.Binders
{
    using System.ComponentModel;
    using Domain;

    public class SelectableTopping : INotifyPropertyChanged
    {
        private readonly Topping _topping;
        private bool _isSelected;

        public SelectableTopping(Topping topping)
        {
            _topping = topping;
        }

        public Topping Topping
        {
            get { return _topping; }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSelected"));
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