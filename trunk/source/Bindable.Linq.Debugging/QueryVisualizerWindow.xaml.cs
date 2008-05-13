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
using BindingOriented.SyncLinq.Debugging.Model;
using System.Collections;
using BindingOriented.SyncLinq.Debugging.Model.Nodes;

namespace BindingOriented.SyncLinq.Debugging
{
    /// <summary>
    /// Interaction logic for QueryVisualizerWindow.xaml.
    /// </summary>
    public sealed partial class QueryVisualizerWindow : Window
    {
        public QueryVisualizerWindow(object query)
        {
            InitializeComponent();
            this.Query = new IQueryNode[] { 
                QueryNodeFactory.Create(query) 
            };
        }

        public static readonly DependencyProperty OutputWindowProperty = DependencyProperty.Register("OutputWindow", typeof(string), typeof(QueryVisualizerWindow), new UIPropertyMetadata(string.Empty));
        public static readonly DependencyProperty QueryProperty = DependencyProperty.Register("Query", typeof(object), typeof(QueryVisualizerWindow), new UIPropertyMetadata(null));

        public string OutputWindow
        {
            get { return (string)GetValue(OutputWindowProperty); }
            set { SetValue(OutputWindowProperty, value); }
        }

        public object Query
        {
            get { return GetValue(QueryProperty); }
            set { SetValue(QueryProperty, value); }
        }
    }
}
