using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Bindable.Linq.Samples.PizzaHaven.Domain;

namespace Bindable.Linq.Samples.PizzaHaven.Binders
{
    public class SelectableTopping : INotifyPropertyChanged
    {
        private Topping _topping;
        private bool _isSelected;

        public SelectableTopping(Topping topping)
        {
            _topping = topping;
        }

        public event PropertyChangedEventHandler PropertyChanged;

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
