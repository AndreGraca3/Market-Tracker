using market_tracker_webapi.Infrastructure.PostgreSQLTables;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Infrastructure
{
    public class MarketTrackerDataContext : DbContext
    {
        public MarketTrackerDataContext(DbContextOptions options) : base(options) { }

        public DbSet<UserEntity> User { get; set; }
        
        public DbSet<TokenEntity> Token { get; set; }
    }
}
