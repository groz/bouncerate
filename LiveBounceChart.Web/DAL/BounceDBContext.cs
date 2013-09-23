using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Web;
using LiveBounceChart.Web.Models;

namespace LiveBounceChart.Web.DAL
{
    public class BounceDBContext : DbContext, IBounceDB
    {
        public IDbSet<BounceEntry> BounceEntries { get; set; }

        public BounceEntry[] RandomSample(int sampleSize, double outliersPercent)
        {
            // TODO: move this to SQL
            // TODO: limit number of entries retrieved

            BounceEntry[] allEntries = BounceEntries.ToArray();

            int nOutliers = (int)outliersPercent * Math.Min(sampleSize, allEntries.Length);

            IEnumerable<BounceEntry> sample = allEntries
                .OrderByDescending(entry => entry.UtcExitTime)
                .Take(sampleSize + 2 * nOutliers);

            return sample
                .OrderBy(entry => entry.BouncePeriod)
                .Skip(nOutliers)
                .Take(sampleSize - 2 * nOutliers)
                .ToArray();
        }

        int IBounceDB.SaveChanges()
        {
            return base.SaveChanges();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}