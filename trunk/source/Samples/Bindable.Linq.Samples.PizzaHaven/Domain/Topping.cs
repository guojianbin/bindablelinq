using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bindable.Linq.Samples.PizzaHaven.Domain
{
    public class Topping
    {
        private string _name;
        private decimal _price;

        public Topping(string name, decimal price)
        {
            _name = name;
            _price = price;
        }

        public string Name
        {
            get { return _name; }
        }

        public decimal Price
        {
            get { return _price; }
        }
    }
}
