using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.Specialized;
using Bindable.Linq;
using Bindable.Linq.Configuration;

namespace Bindable.Linq.Collections
{
    /// <summary>
    /// Used in the <see cref="T:GroupByIterator`2"/> as the result of a grouping.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TElement">The type of the element.</typeparam>
    internal sealed class BindableGrouping<TKey, TElement> : IBindableGrouping<TKey, TElement>
    {
        private readonly IBindableQuery<TElement> _groupWhereQuery;
        private readonly TKey _key;

        /// <summary>
        /// Initializes a new instance of the <see cref="BindableGrouping&lt;TKey, TElement&gt;"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="groupWhereQuery">The group query.</param>
        public BindableGrouping(TKey key, IBindableQuery<TElement> groupWhereQuery)
        {
            _key = key;
            _groupWhereQuery = groupWhereQuery;
            _groupWhereQuery.CollectionChanged += new NotifyCollectionChangedEventHandler(GroupWhereQuery_CollectionChanged);
            _groupWhereQuery.PropertyChanged += new PropertyChangedEventHandler(GroupWhereQuery_PropertyChanged);
        }

        #region IBindableGrouping<TKey,TElement> Members
        /// <summary>
        /// Gets the key of the <see cref="T:System.Linq.IGrouping`2"/>.
        /// </summary>
        /// <value></value>
        /// <returns>The key of the <see cref="T:System.Linq.IGrouping`2"/>.</returns>
        public TKey Key
        {
            get { return _key; }
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Occurs when the collection changes.
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<TElement> GetEnumerator()
        {
            return _groupWhereQuery.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count
        {
            get { return _groupWhereQuery.Count; }
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        public IBindingConfiguration Configuration
        {
            get { return _groupWhereQuery.Configuration; }
        }

        #endregion

        private void GroupWhereQuery_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.OnPropertyChanged(e);
        }

        private void GroupWhereQuery_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanged(e);
        }

        /// <summary>
        /// Raises the <see cref="E:PropertyChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        private void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:CollectionChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        private void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            NotifyCollectionChangedEventHandler handler = this.CollectionChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public void Dispose()
        {
            _groupWhereQuery.CollectionChanged -= new NotifyCollectionChangedEventHandler(GroupWhereQuery_CollectionChanged);
            _groupWhereQuery.PropertyChanged -= new PropertyChangedEventHandler(GroupWhereQuery_PropertyChanged);
            _groupWhereQuery.Dispose();
        }
    }
}