﻿using market_tracker_webapi.Infrastructure.PostgresSQLTables;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace market_tracker_webapi.Infrastructure
{
    public class MarketTrackerDataContext : DbContext
    {
        public MarketTrackerDataContext(DbContextOptions options) : base(options) { }

        public DbSet<UserEntity> User { get; set; }
    }
}
