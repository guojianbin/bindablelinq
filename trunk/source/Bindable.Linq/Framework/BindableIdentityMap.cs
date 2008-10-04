using Bindable.Linq.Collections;
using Bindable.Linq.Interfaces;
using Bindable.Linq.Helpers;
using Bindable.Linq.Threading;

namespace Bindable.Linq.Framework
{
    public class BindableIdentityMap<TEntity, TIdentity> : DispatcherBound, IIdentityMap<TEntity> where TEntity : class
    {
        private IEntityStore<TEntity, TIdentity> _store;
        private IIdentifier<TEntity, TIdentity> _identifier;

        public BindableIdentityMap(IDispatcher dispatcher, IIdentifier<TEntity, TIdentity> identifier, IEntityStore<TEntity, TIdentity> store) : base(dispatcher)
        {
            _identifier = identifier;
            _store = store;
        }

        public TEntity Store(TEntity entity)
        {
            AssertDispatcherThread();
            var identity = _identifier.GetIdentity(entity);
            var existing = _store.Get(identity);
            if (existing != null)
            {
                // Conflict resolution and merging
                
            }
            else
            {
                _store.Add(entity);
            }

            return entity;
        }

        public IBindableCollection<TEntity> GetAll()
        {
            AssertDispatcherThread();
            return _store.GetAll();
        }
    }
}
