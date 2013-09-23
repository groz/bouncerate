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

        // http://stackoverflow.com/a/648247
        public IEnumerable<BounceEntry> GetRandomSample(int sampleSize)
        {
            var randomSample = (
                from row in BounceEntries
                orderby Random()
                select row)
                .Take(sampleSize);

            return randomSample.AsEnumerable();
        }

        [Function(Name = "NEWID", IsComposable = true)]
        public Guid Random()
        { 
            // to prove not used by our C# code... 
            throw new NotImplementedException();
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