using System;

namespace Bindable.Linq.Interfaces.Internal
{
    /// <summary>
    /// Event handler for the <see cref="ResetEventArgs"/>
    /// </summary>
    internal delegate void ResetEventHandler(object sender, ResetEventArgs args);

    /// <summary>
    /// Event arguments raised when a collection has been reset.
    /// </summary>
    internal class ResetEventArgs : EventArgs
    {
        private readonly string _reason;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResetEventArgs"/> class.
        /// </summary>
        /// <param name="reason">The reason.</param>
        public ResetEventArgs(string reason)
        {
            _reason = reason;
        }

        /// <summary>
        /// Gets a description of why this reset has taken place. This is primarily used for diagnostics.
        /// </summary>
        /// <value>The reason.</value>
        public string Reason
        {
            get { return _reason; }
        }
    }
}