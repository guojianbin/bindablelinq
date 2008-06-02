using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bindable.Linq.Samples.PizzaHaven.Domain;

namespace Bindable.Linq.Samples.PizzaHaven.Binders
{
    public class PizzaCustomizer
    {
        private Pizza _pizza;
        private IBindableCollection<SelectableTopping> _availableToppings;
        private IBindableCollection<Topping> _selectedToppings;
        private IBindable<decimal> _totalPrice;

        public PizzaCustomizer(Pizza pizza)
        {
            _pizza = pizza;

            _availableToppings = _pizza.AvailableToppings
                                       .AsBindable()
                                       .Select(topping => new SelectableTopping(topping));

            _selectedToppings = _availableToppings
                                       .Where(selectableTopping => selectableTopping.IsSelected)
                                       .Select(selectableTopping => selectableTopping.Topping);
            
            _totalPrice = _selectedToppings
                                       .Sum(topping => topping.Price)
                                       .Project(toppingsTotal => toppingsTotal + pizza.BasePrice);
        }

        public Pizza Pizza
        {
            get { return _pizza; }
        }

        public IBindableCollection<SelectableTopping> AvailableToppings
        {
            get { return _availableToppings; }
        }

        public IBindableCollection<Topping> SelectedToppings
        {
            get { return _selectedToppings; }
        }

        public IBindable<decimal> TotalPrice
        {
            get { return _totalPrice; }
        }
    }
}
