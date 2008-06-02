namespace Bindable.Linq.Samples.WindowsForms
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Windows.Forms;
    using Threading;

    public partial class MainForm : Form
    {
        private static readonly Dictionary<int, ProcessWrapper> _cachedProcesses = new Dictionary<int, ProcessWrapper>();

        public MainForm()
        {
            InitializeComponent();

            _processWrapperBindingSource.DataSource = GetAllProcesses().AsBindable().Polling(new WpfDispatcher(), TimeSpan.FromMilliseconds(300)).OrderBy(p => p.ProcessName).Where(p => p.ProcessName.ToLower().Contains(_filterTextBox.Text.ToLower())).ToBindingList();
        }

        private IEnumerable<ProcessWrapper> GetAllProcesses()
        {
            var wrappers = new List<ProcessWrapper>();
            foreach (var process in Process.GetProcesses())
            {
                if (!_cachedProcesses.ContainsKey(process.Id))
                {
                    _cachedProcesses.Add(process.Id, new ProcessWrapper());
                }
                _cachedProcesses[process.Id].ProcessName = process.ProcessName;
                _cachedProcesses[process.Id].Title = process.MainWindowTitle;
                _cachedProcesses[process.Id].ID = process.Id;
                yield return _cachedProcesses[process.Id];
            }
        }
    }
}