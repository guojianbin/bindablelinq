using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel;

namespace BindingOriented.SyncLinq.Debugging.Model
{
    /// <summary>
    /// 
    /// </summary>
    internal sealed class Console : INotifyPropertyChanged
    {
        private StringWriter _innerWriter;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleStream"/> class.
        /// </summary>
        public Console()
        {
            _innerWriter = new StringWriter();
        }

        /// <summary>
        /// Writes the specified format.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void Write(string format, params object[] args)
        {
            _innerWriter.Write(format, args);
            this.OnPropertyChanged(new PropertyChangedEventArgs("Current"));
        }

        /// <summary>
        /// Gets the current value.
        /// </summary>
        /// <value>The current.</value>
        public string Current
        {
            get { return _innerWriter.ToString(); }
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the <see cref="E:PropertyChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        private void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, e);
            }
        }
    }
}
