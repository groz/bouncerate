﻿using System.Data.Entity;
using LiveBounceChart.Web.Models;

namespace LiveBounceChart.Web.DAL
{
    public interface IBounceDB
    {
        IDbSet<BounceEntry> BounceEntries { get; set; }
    }
}