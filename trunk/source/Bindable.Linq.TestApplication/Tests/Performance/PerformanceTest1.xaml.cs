using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Bindable.Linq;
using Bindable.Linq.TestApplication;
using Bindable.Linq.TestApplication.Framework;

namespace Bindable.Linq.TestApplication.Tests.Performance
{
    /// <summary>
    /// Interaction logic for PerformanceTest1.xaml
    /// </summary>
    public partial class PerformanceTest1 : Page
    {
        public PerformanceTest1()
        {
            InitializeComponent();
            this.LinqTestRuns = new ObservableCollection<TestRun>();
            this.SyncLinqTestRuns = new ObservableCollection<TestRun>();
        }

        public static readonly DependencyProperty LinqTestRunsProperty = DependencyProperty.Register("LinqTestRuns", typeof(ObservableCollection<TestRun>), typeof(PerformanceTest1), new UIPropertyMetadata(null));
        public static readonly DependencyProperty SyncLinqTestRunsProperty = DependencyProperty.Register("SyncLinqTestRuns", typeof(ObservableCollection<TestRun>), typeof(PerformanceTest1), new UIPropertyMetadata(null));

        public ObservableCollection<TestRun> LinqTestRuns
        {
            get { return (ObservableCollection<TestRun>)GetValue(LinqTestRunsProperty); }
            set { SetValue(LinqTestRunsProperty, value); }
        }

        public ObservableCollection<TestRun> SyncLinqTestRuns
        {
            get { return (ObservableCollection<TestRun>)GetValue(SyncLinqTestRunsProperty); }
            set { SetValue(SyncLinqTestRunsProperty, value); }
        }

        private NotifyCollectionChangedAction GetAction()
        {
            switch (_actionCombo.Text)
            {
                case "Add":
                    return NotifyCollectionChangedAction.Add;
                case "Remove":
                    return NotifyCollectionChangedAction.Remove;
                default:
                    return NotifyCollectionChangedAction.Reset;
            }
        }

        private void GoSyncLinq_Clicked(object sender, RoutedEventArgs e)
        {
            SyncLinqPerformanceTestRun run = new SyncLinqPerformanceTestRun(int.Parse(_startWith.Text), int.Parse(_items.Text), GetAction());
            this.SyncLinqTestRuns.Add(run);
            run.Execute();
        }

        private void GoLinq_Clicked(object sender, RoutedEventArgs e)
        {
            LinqPerformanceTestRun run = new LinqPerformanceTestRun(int.Parse(_startWith.Text), int.Parse(_items.Text), GetAction());
            this.LinqTestRuns.Add(run);
            run.Execute();
        }

        private class LinqPerformanceTestRun : PerformanceTestRun<Contact>
        {
            private IEnumerable _query;

            public LinqPerformanceTestRun(int startingItems, int additionalItems, NotifyCollectionChangedAction action)
                :base(startingItems, additionalItems, action)
            {

            }

            private void Generate(int items)
            {
                List<Contact> contacts = new List<Contact>();
                this.MainStopwatch.Stop();
                for (int i = 0; i < items; i++) 
                {
                    contacts.Add(new Contact() { Name = Guid.NewGuid().ToString(), Company = Guid.NewGuid().ToString() });
                }
                this.MainStopwatch.Start();
                contacts.ForEach(e => this.Inputs.Add(e));
            }

            protected override void Initialize()
            {
                Generate(this.StartingItems);

                _query = this.Inputs
                       .Where(c => c.Name.EndsWith("5") || c.Name.EndsWith("7"))
                       .OrderBy(c => c.Company)
                       .ThenBy(c => c.Name)
                       .Select(c => new
                       {
                           Name = c.Name.ToUpper(),
                           Company = c.Company.ToUpper()
                       });
                this.Enumerate(_query);
            }

            protected override void PerformAction(NotifyCollectionChangedAction action)
            {
                switch (action)
                {
                    case NotifyCollectionChangedAction.Add:
                        this.Generate(this.AdditionalItems);
                        this.Enumerate(_query);
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        for (int i = 0; i < this.AdditionalItems; i++)
                        {
                            this.Inputs.RemoveAt(0);
                        }
                        this.Enumerate(_query);
                        break;
                }
            }
        }

        private class SyncLinqPerformanceTestRun : PerformanceTestRun<Contact>
        {
            private IBindableQuery _query;

            public SyncLinqPerformanceTestRun(int startingItems, int additionalItems, NotifyCollectionChangedAction action)
                : base(startingItems, additionalItems, action)
            {

            }

            private void Generate(int items)
            {
                List<Contact> contacts = new List<Contact>();
                this.MainStopwatch.Stop();
                for (int i = 0; i < items; i++)
                {
                    contacts.Add(new Contact() { Name = Guid.NewGuid().ToString(), Company = Guid.NewGuid().ToString() });
                }
                this.MainStopwatch.Start();
                contacts.ForEach(e => this.Inputs.Add(e));
            }

            protected override void Initialize()
            {
                Generate(this.StartingItems);

                _query = this.Inputs
                       .AsBindable()
                       .Where(c => c.Name.EndsWith("5") || c.Name.EndsWith("7"))
                       .OrderBy(c => c.Company)
                       .ThenBy(c => c.Name)
                       .Select(c => new
                       {
                           Name = c.Name.ToUpper(),
                           Company = c.Company.ToUpper()
                       });
                this.Enumerate(_query);
            }

            protected override void PerformAction(NotifyCollectionChangedAction action)
            {
                switch (action)
                {
                    case NotifyCollectionChangedAction.Add:
                        this.Generate(this.AdditionalItems);
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        for (int i = 0; i < this.AdditionalItems; i++)
                        {
                            this.Inputs.RemoveAt(0);
                        }
                        break;
                }
            }
        }
    }
}
