using Bindable.Linq.Interfaces;

namespace Bindable.Linq.Samples.PizzaHaven.Binders
{
    using Domain;
    using Bindable.Linq.Operators;

    public class PizzaCustomizer
    {
        private readonly IBindableCollection<SelectableTopping> _availableToppings;
        private readonly Pizza _pizza;
        private readonly IBindableCollection<Topping> _selectedToppings;
        private readonly IBindable<decimal> _totalPrice;
        private readonly IBindable<string> _healthWarningMessage;

        public PizzaCustomizer(Pizza pizza)
        {
            _pizza = pizza;

            _availableToppings = _pizza.AvailableToppings
                                    .AsBindable()
                                    .Select(topping => new SelectableTopping(topping));

            _selectedToppings = _availableToppings
                                    .Where(selectableTopping => selectableTopping.IsSelected)
                                    .Select(selectableTopping => selectableTopping.Topping);

            _healthWarningMessage = _selectedToppings.Count()
                                    .Switch()
                                        .Case(0,
                                            "Surely you would like more toppings?")
                                        .Case(toppings => toppings >= 3,
                                            "Too many toppings!")
                                        .Default(
                                            "Perfecto!")
                                    .EndSwitch();

            _totalPrice = _selectedToppings.Sum(topping => topping.Price).Project(toppingsTotal => toppingsTotal + pizza.BasePrice);
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

        public IBindable<string> HealthWarningMessage
        {
            get { return _healthWarningMessage; }
        }

        public IBindable<decimal> TotalPrice
        {
            get { return _totalPrice; }
        }
    }
}