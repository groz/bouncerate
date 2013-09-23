using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using LiveBounceChart.Web.Models;

namespace LiveBounceChart.Web.DAL
{
    public class BounceDBContext : DbContext, IBounceDB
    {
        public IDbSet<BounceEntry> BounceEntries { get; set; }

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