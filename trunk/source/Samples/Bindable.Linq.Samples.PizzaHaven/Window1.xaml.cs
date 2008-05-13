using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Bindable.Linq.Samples.PizzaHaven.Binders;
using Bindable.Linq.Samples.PizzaHaven.Domain;

namespace Bindable.Linq.Samples.PizzaHaven
{    
    /// <summary>
    /// Interaction logic for Window1.xaml.
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();

            this.DataContext = new PizzaCustomizer(
                new Pizza("BBQ Bacon and Cheese", 9.95M,
                    new Topping("Anchovies", 2.95M),
                    new Topping("Onion", 2.95M),
                    new Topping("Pepperoni", 2.95M),
                    new Topping("Ice Cream", 2.95M),
                    new Topping("Bananas", 2.95M)));
        }
    }
}
