using System.Collections;
using System.Collections.Specialized;
using Bindable.Linq.Helpers;
using Bindable.Linq.Threading;

namespace Bindable.Linq.Adapters.Incoming
{
    internal class ObservableCollectionToBindableCollectionAdapter<TElement> : BindableCollectionAdapterBase<TElement>
    {
        public ObservableCollectionToBindableCollectionAdapter(IEnumerable sourceCollection, IDispatcher dispatcher)
            : base(sourceCollection, dispatcher)
        {
            var observable = sourceCollection as INotifyCollectionChanged;
            if (observable != null)
            {
                // TODO
                observable.CollectionChanged += Weak.Event<NotifyCollectionChangedEventArgs>( (sender, e) => Dispatcher.Dispatch(() => OnCollectionChanged(e))).KeepAlive(InstanceLifetime).HandlerProxy.Handler;
            }
        }
    }
}
