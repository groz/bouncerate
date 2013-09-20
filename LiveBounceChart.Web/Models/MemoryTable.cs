using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace LiveBounceChart.Web.Models
{
    public class MemoryTable<T> : IRepositoryTable<T>
    {
        private readonly List<T> _collection = new List<T>();

        public MemoryTable(List<T> data)
        {
            _collection = data;
        }

        public void Add(T element)
        {
            _collection.Add(element);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add));
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            NotifyCollectionChangedEventHandler handler = CollectionChanged;
            if (handler != null) handler(this, e);
        }
    }
}