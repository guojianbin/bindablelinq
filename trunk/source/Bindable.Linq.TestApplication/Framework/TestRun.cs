using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Diagnostics;

namespace Bindable.Linq.TestApplication.Framework
{
    public class TestRun : INotifyPropertyChanged
    {
        private string _description;
        private double _totalMilliseconds;
        private BackgroundWorker _backgroundWorker;
        private Stopwatch _mainStopwatch = new Stopwatch();

        public TestRun()
        {

        }

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                if (_backgroundWorker != null)
                {
                    _backgroundWorker.ReportProgress(0);
                }
                else
                {
                    OnPropertyChanged(new PropertyChangedEventArgs("Description"));
                }
            }
        }

        public double TotalMilliseconds
        {
            get { return _totalMilliseconds; }
            set {
                _totalMilliseconds = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TotalMilliseconds"));
            }
        }

        public bool IsRunning
        {
            get { return _backgroundWorker == null || _backgroundWorker.IsBusy; }
        }

        public Stopwatch MainStopwatch
        {
            get { return _mainStopwatch; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Execute()
        {
            _backgroundWorker = new BackgroundWorker();
            _backgroundWorker.DoWork += new DoWorkEventHandler(BackgroundWorker_DoWork);
            _backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BackgroundWorker_RunWorkerCompleted);
            _backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(BackgroundWorker_ProgressChanged);
            _backgroundWorker.WorkerSupportsCancellation = true;
            _backgroundWorker.WorkerReportsProgress = true;
            _backgroundWorker.RunWorkerAsync();
            OnPropertyChanged(new PropertyChangedEventArgs("IsRunning"));
        }

        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("Description"));
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            _mainStopwatch = new Stopwatch();
            ExecuteOverride();
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _mainStopwatch.Stop();
            this.TotalMilliseconds = _mainStopwatch.ElapsedMilliseconds;
            OnPropertyChanged(new PropertyChangedEventArgs("IsRunning"));
        }

        protected virtual void ExecuteOverride()
        {
            
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, e);
            }
        }
    }
}
