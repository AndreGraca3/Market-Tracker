using market_tracker_webapi.Infrastructure.PostgreSQLTables;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Infrastructure
{
    public class MarketTrackerDataContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<UserEntity> User { get; set; }
        
        public DbSet<TokenEntity> Token { get; set; }

        public DbSet<ProductEntity> Product { get; set; }

        public DbSet<CategoryEntity> Category { get; set; }
        
        public DbSet<BrandEntity> Brand { get; set; }
        
        public DbSet<ProductReviewEntity> ProductReview { get; set; }
                
        public DbSet<StoreEntity> Store { get; set; }
                
        public DbSet<CompanyEntity> Company { get; set; }
                
        public DbSet<CityEntity> City { get; set; }
    }
}
