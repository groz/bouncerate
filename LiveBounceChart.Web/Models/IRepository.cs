using System;
using System.Linq;
using System.Linq.Expressions;

namespace LiveBounceChart.Web.Models
{
    public interface IRepository
    {
        IRepositoryTable<TimeSpan> BounceTimes { get; }

        void Commit();
    }
}