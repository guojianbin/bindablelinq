using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Bindable.Linq.Dependencies;
using Bindable.Linq.Tests.TestHelpers;
using NUnit.Framework;
using Bindable.Linq.Dependencies.PathNavigation.Tokens;
using Bindable.Linq.Configuration;

namespace Bindable.Linq.Tests.Unit.Dependencies
{
    [TestFixture]
    public class PropertyChangeDependencyTests : TestFixture
    {
        [Test]
        public void PropertyChangeDependency_NestedPropertyDependencyCreatedBeforeObjectPopulation()
        {
            Customer customer = new Customer();
            Stack<string> events = new Stack<string>();

            Action<object, string> callback = 
                delegate(object changedObject, string propertyPath)
                {
                    events.Push(propertyPath);
                    Assert.AreEqual(customer, changedObject);
                };
            IToken dependency = BindingConfigurations.Default.CreatePathNavigator().TraverseNext(customer, "Addresses.Home.Line1", callback);

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            customer.Name = "Paul";
            Assert.AreEqual(0, events.Count);

            customer.Name = "Fred";
            Assert.AreEqual(0, events.Count);

            customer.Addresses = new Addresses();
            Assert.AreEqual(1, events.Count);
            Assert.AreEqual("Addresses", events.Pop());

            customer.Addresses.Home = new Address();
            Assert.AreEqual(1, events.Count);
            Assert.AreEqual("Addresses.Home", events.Pop());

            customer.Addresses.Home.Line1 = "399 McBryde";
            Assert.AreEqual(1, events.Count);
            Assert.AreEqual("Addresses.Home.Line1", events.Pop());

            customer.Addresses.Home.Line1 = "299 McBryde";
            Assert.AreEqual(1, events.Count);
            Assert.AreEqual("Addresses.Home.Line1", events.Pop());

            customer.Addresses.Home.Line2 = "Whyalla Norrie";
            Assert.AreEqual(0, events.Count);
        }

        [Test]
        public void PropertyChangeDependency_NestedPropertyDependencyCreatedAfterObjectPopulation()
        {
            Customer customer = new Customer();
            customer.Name = "Paul";
            customer.Addresses = new Addresses();
            customer.Addresses.Home = new Address();
            customer.Addresses.Home.Line1 = "399 McBryde";
            customer.Addresses.Home.Line2 = "Whyalla Norrie";
            Stack<string> events = new Stack<string>();

            Action<object, string> callback =
                delegate(object changedObject, string propertyPath)
                {
                    events.Push(propertyPath);
                    Assert.AreEqual(customer, changedObject);
                };
            IToken dependency = BindingConfigurations.Default.CreatePathNavigator().TraverseNext(customer, "Addresses.Home.Line1", callback);

            customer.Name = "Paul";
            Assert.AreEqual(0, events.Count);

            customer.Addresses.Home.Line1 = "799 McBryde";
            Assert.AreEqual(1, events.Count);
            Assert.AreEqual("Addresses.Home.Line1", events.Pop());

            customer.Addresses = new Addresses();
            Assert.AreEqual(1, events.Count);
            Assert.AreEqual("Addresses", events.Pop());

            customer.Addresses.Home = new Address();
            Assert.AreEqual(1, events.Count);
            Assert.AreEqual("Addresses.Home", events.Pop());

            customer.Addresses.Home.Line1 = "799 McBryde";
            Assert.AreEqual(1, events.Count);
            Assert.AreEqual("Addresses.Home.Line1", events.Pop());

            customer.Addresses.Home.Line1 = "199 McBryde";
            Assert.AreEqual(1, events.Count);
            Assert.AreEqual("Addresses.Home.Line1", events.Pop());

            customer.Addresses.Home.Line2 = "Whyalla Norrie";
            Assert.AreEqual(0, events.Count);
        }

        [Test]
        public void PropertyChangeDependency_NestedPropertyChangeStopsEventsBeingRaised()
        {
            Customer customer = new Customer();
            customer.Name = "Paul";
            customer.Addresses = new Addresses();
            customer.Addresses.Home = new Address();
            customer.Addresses.Home.Line1 = "399 McBryde";
            customer.Addresses.Home.Line2 = "Whyalla Norrie";
            Stack<string> events = new Stack<string>();

            Action<object, string> callback =
                delegate(object changedObject, string propertyPath)
                {
                    events.Push(propertyPath);
                    Assert.AreEqual(customer, changedObject);
                };
            IToken dependency = BindingConfigurations.Default.CreatePathNavigator().TraverseNext(customer, "Addresses.Home.Line1", callback);
            
            customer.Addresses.Home.Line1 = "799 McBryde";
            Assert.AreEqual(1, events.Count);
            Assert.AreEqual("Addresses.Home.Line1", events.Pop());

            Address oldHomeAddres = customer.Addresses.Home;

            customer.Addresses.Home = new Address();
            Assert.AreEqual(1, events.Count);
            Assert.AreEqual("Addresses.Home", events.Pop());

            oldHomeAddres.Line1 = "Blah";
            Assert.AreEqual(0, events.Count);
        }

        #region Mock objects 

        class DomainObject : INotifyPropertyChanged
        {

            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
            {
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, e);
                }
            }
        }

        class Customer : DomainObject
        {
            private Addresses _addresses;
            private string _name;

            public Addresses Addresses
            {
                get { return _addresses; }
                set
                {
                    _addresses = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Addresses"));
                }
            }

            public string Name
            {
                get { return _name; }
                set
                {
                    _name = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Name"));
                }
            }
        }

        class Addresses : DomainObject
        {
            private Address _home;
            private Address _work;

            public Address Home
            {
                get { return _home; }
                set
                {
                    _home = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Home"));
                }
            }

            public Address Work
            {
                get { return _work; }
                set
                {
                    _work = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Work"));
                }
            }
        }

        class Address : DomainObject
        {
            private string _line1;
            private string _line2;
            private int _postcode;

            public string Line1
            {
                get { return _line1; }
                set
                {
                    _line1 = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Line1"));
                }
            }

            public string Line2
            {
                get { return _line2; }
                set
                {
                    _line2 = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Line2"));
                }
            }

            public int Postcode
            {
                get { return _postcode; }
                set
                {
                    _postcode = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Postcode"));
                }
            }
        }

        #endregion
    }
}
