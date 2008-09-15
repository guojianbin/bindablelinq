namespace Bindable.Linq.Samples.PizzaHaven
{
    using System.Windows;
    using Binders;
    using Domain;

    /// <summary>
    /// Interaction logic for Window1.xaml.
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();

            DataContext = new PizzaCustomizer(
                new Pizza("BBQ Bacon and Cheese", 9.95M, 
                    new Topping("Anchovies", 2.95M), 
                    new Topping("Onion", 2.95M), 
                    new Topping("Pepperoni", 2.95M), 
                    new Topping("Ice Cream", 2.95M), 
                    new Topping("Bananas", 2.95M)));
        }
    }
}