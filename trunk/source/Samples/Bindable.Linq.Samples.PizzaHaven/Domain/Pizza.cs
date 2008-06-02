using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Bindable.Linq.Samples.PizzaHaven.Domain
{
    public class Pizza
    {
        private string _name;
        private decimal _basePrice;
        private List<Topping> _availableToppings;

        public Pizza(string name, decimal basePrice, params Topping[] availableToppings)
        {
            _name = name;
            _basePrice = basePrice;
            _availableToppings = new List<Topping>(availableToppings);
        }

        public string Name
        {
            get { return _name; }
        }

        public decimal BasePrice
        {
            get { return _basePrice; }
        }

        public IEnumerable<Topping> AvailableToppings
        {
            get { return _availableToppings; }
        }
    }
}
