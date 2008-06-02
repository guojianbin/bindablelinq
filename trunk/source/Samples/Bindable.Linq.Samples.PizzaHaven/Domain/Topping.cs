namespace Bindable.Linq.Samples.PizzaHaven.Domain
{
    public class Topping
    {
        private readonly string _name;
        private readonly decimal _price;

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