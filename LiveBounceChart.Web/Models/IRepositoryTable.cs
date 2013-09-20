using System.Collections.Generic;
using System.Collections.Specialized;

namespace LiveBounceChart.Web.Models
{
    public interface IRepositoryTable<T>: IEnumerable<T>, INotifyCollectionChanged
    {
        void Add(T element);
    }
}