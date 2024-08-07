﻿using market_tracker_webapi.Infrastructure.PostgreSQLTables.Account;
using market_tracker_webapi.Infrastructure.PostgreSQLTables.Account.Auth;
using market_tracker_webapi.Infrastructure.PostgreSQLTables.Account.Users;
using market_tracker_webapi.Infrastructure.PostgreSQLTables.List;
using market_tracker_webapi.Infrastructure.PostgreSQLTables.Market;
using market_tracker_webapi.Infrastructure.PostgreSQLTables.Market.Inventory;
using market_tracker_webapi.Infrastructure.PostgreSQLTables.Market.Inventory.Product;
using market_tracker_webapi.Infrastructure.PostgreSQLTables.Market.Store;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace market_tracker_webapi.Infrastructure;

public class MarketTrackerDataContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<UserEntity> User { get; set; }

    public DbSet<ClientEntity> Client { get; set; }

    public DbSet<OperatorEntity> Operator { get; set; }

    public DbSet<AccountEntity> Account { get; set; }

    public DbSet<TokenEntity> Token { get; set; }

    public DbSet<ProductEntity> Product { get; set; }

    public DbSet<CategoryEntity> Category { get; set; }

    public DbSet<BrandEntity> Brand { get; set; }

    public DbSet<ProductReviewEntity> ProductReview { get; set; }

    public DbSet<ProductFavouriteEntity> ProductFavorite { get; set; }

    public DbSet<ProductStatsCountsEntity> ProductStatsCounts { get; set; }

    public DbSet<FcmRegisterEntity> FcmRegister { get; set; }

    public DbSet<PriceAlertEntity> PriceAlert { get; set; }

    public DbSet<PriceEntryEntity> PriceEntry { get; set; }

    public DbSet<PromotionEntity> Promotion { get; set; }

    public DbSet<ProductAvailabilityEntity> ProductAvailability { get; set; }

    public DbSet<StoreEntity> Store { get; set; }

    public DbSet<CompanyEntity> Company { get; set; }

    public DbSet<CityEntity> City { get; set; }

    public DbSet<ListEntity> List { get; set; }

    public DbSet<ListEntryEntity> ListEntry { get; set; }

    public DbSet<ListClientEntity> ListClient { get; set; }

    public DbSet<PreRegistrationEntity> PreRegister { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ListClientEntity>()
            .HasKey(lc => new { lc.ListId, lc.ClientId });

        // datetime to utc in all entities
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
                {
                    property.SetValueConverter(new UtcDateTimeConverter());
                }
            }
        }
    }
}

public class UtcDateTimeConverter() : ValueConverter<DateTime, DateTime>(v => v.ToUniversalTime(),
    v => DateTime.SpecifyKind(v, DateTimeKind.Utc));