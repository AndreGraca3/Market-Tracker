using market_tracker_webapi.Infrastructure.PostgreSQLTables;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Infrastructure
{
    public class MarketTrackerDataContext : DbContext
    {
                public DbSet<UserEntity> User { get; set; }
                
                public DbSet<StoreEntity> Store { get; set; }
                
                public DbSet<CompanyEntity> Company { get; set; }
                
                public DbSet<CityEntity> City { get; set; }
                
        public MarketTrackerDataContext(DbContextOptions options) : base(options) { }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<StoreEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<CompanyEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<ProductEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
            
            modelBuilder.Entity<CityEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
        }
    }
}
