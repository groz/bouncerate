using System;
using System.Collections.Generic;

namespace LiveBounceChart.Web.Models
{
    public class MemoryRepository : IRepository
    {
        public IRepositoryTable<TimeSpan> BounceTimes { get; private set; }

        public MemoryRepository()
        {
            BounceTimes = new MemoryTable<TimeSpan>(new List<TimeSpan>());
        }

        protected MemoryRepository(List<TimeSpan> data)
        {
            BounceTimes = new MemoryTable<TimeSpan>(data);
        }

        public virtual void Commit()
        {
        }
    }
}