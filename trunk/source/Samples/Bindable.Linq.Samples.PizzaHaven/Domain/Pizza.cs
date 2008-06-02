namespace Bindable.Linq.Samples.PizzaHaven.Domain
{
    using System.Collections.Generic;

    public class Pizza
    {
        private readonly List<Topping> _availableToppings;
        private readonly decimal _basePrice;
        private readonly string _name;

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