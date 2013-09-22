using System.Collections.Generic;
using LiveBounceChart.Web.Models;

namespace LiveBounceChart.Web.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<LiveBounceChart.Web.DAL.BounceDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(LiveBounceChart.Web.DAL.BounceDBContext context)
        {
            var data = new List<BounceEntry>
            {
                new BounceEntry()
                {
                    Id = 0,
                    BouncePeriod = TimeSpan.FromSeconds(5),
                    UtcExitTime = DateTime.Now
                },

                new BounceEntry()
                {
                    Id = 1,
                    BouncePeriod = TimeSpan.FromSeconds(25),
                    UtcExitTime = DateTime.Now
                },

                new BounceEntry()
                {
                    Id = 2,
                    BouncePeriod = TimeSpan.FromSeconds(13),
                    UtcExitTime = DateTime.Now
                },
            };

            data.ForEach(be => context.BounceEntries.AddOrUpdate(e => e.Id, be));

            context.SaveChanges();

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
