using System.Windows.Threading;

namespace Bindable.Linq.Threading
{    
    /// <summary>
    /// A factory for creating the correct <see cref="IDispatcher"/> implementation based on
    /// the current environment.
    /// </summary>
    internal sealed class DispatcherFactory
    {
        /// <summary>
        /// Creates the specified dispatcher.
        /// </summary>
        /// <param name="dispatcher">The dispatcher.</param>
        /// <returns>The correct IDispatcher.</returns>
        public static IDispatcher Create(Dispatcher dispatcher)
        {
            IDispatcher result = null;
#if SILVERLIGHT
            result = new SilverlightDispatcher(dispatcher);
#else
            result = new WpfDispatcher(dispatcher);
#endif
            return result;
        }
    }
}