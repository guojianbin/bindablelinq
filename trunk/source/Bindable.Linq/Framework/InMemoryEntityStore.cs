using System;
using System.Linq;
using Bindable.Linq.Collections;
using Bindable.Linq.Helpers;
using Bindable.Linq.Interfaces;
using Bindable.Linq.Threading;

namespace Bindable.Linq.Framework
{
    public class InMemoryEntityStore<TEntity, TIdentity> : DispatcherBound, IEntityStore<TEntity, TIdentity> where TEntity : class
    {
        private readonly BindableCollection<WeakReference> _itemReferences;
        private readonly IIdentifier<TEntity, TIdentity> _identifier;

        public InMemoryEntityStore(IDispatcher dispatcher, IIdentifier<TEntity, TIdentity> identifier)
            : base(dispatcher)
        {
            _itemReferences = new BindableCollection<WeakReference>(dispatcher);
            _identifier = identifier;
        }

        public IBindableCollection<TEntity> GetAll()
        {
            AssertDispatcherThread();
            return _itemReferences.Select(weak => weak.Target as TEntity).Where(entity => entity != null);
        }

        public void Add(TEntity entity)
        {
            AssertDispatcherThread();
            _itemReferences.Add(new WeakReference(entity, true));
        }

        public void Remove(TEntity entity)
        {
            AssertDispatcherThread();
            var reference = _itemReferences.ToList().Where(weak => weak.Target == entity).FirstOrDefault();
            if (reference != null)
            {
                _itemReferences.Remove(reference);
            }
        }

        public TEntity Get(TIdentity identity)
        {
            AssertDispatcherThread();
            return GetAll().ToList().Where(entity => _identifier.GetIdentity(entity).Equals(identity)).FirstOrDefault();
        }
    }
}
