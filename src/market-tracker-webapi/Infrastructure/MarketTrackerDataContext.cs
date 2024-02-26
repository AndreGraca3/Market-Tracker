using market_tracker_webapi.Infrastructure.PostgresSQLTables;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace market_tracker_webapi.Infrastructure
{
    public class MarketTrackerDataContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public MarketTrackerDataContext(IConfiguration configuration, DbContextOptions options) : base(options) 
        { 
            _configuration = configuration;
        }
        public DbSet<UserEntity> User { get; set; }
    }
}
