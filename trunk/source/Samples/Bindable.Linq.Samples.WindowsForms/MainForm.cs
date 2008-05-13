using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

namespace Bindable.Linq.Samples.WindowsForms
{
    public partial class MainForm : Form
    {
        private static Dictionary<int, ProcessWrapper> _cachedProcesses = 
            new Dictionary<int, ProcessWrapper>();

        public MainForm()
        {
            InitializeComponent();

            _processWrapperBindingSource.DataSource =
                GetAllProcesses()
                .AsBindable()
                .Polling(TimeSpan.FromMilliseconds(300))
                .OrderBy(p => p.ProcessName)
                .Where(p => p.ProcessName.ToLower()
                    .Contains(_filterTextBox.Text.ToLower()))
                .ToBindingList();
        }

        private IEnumerable<ProcessWrapper> GetAllProcesses()
        {
            List<ProcessWrapper> wrappers = new List<ProcessWrapper>();
            foreach (Process process in Process.GetProcesses())
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
