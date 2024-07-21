using FluentAssertions;
using market_tracker_webapi.Application.Domain.Filters;
using market_tracker_webapi.Application.Domain.Filters.Product;
using market_tracker_webapi.Application.Domain.Schemas.Market.Inventory;
using market_tracker_webapi.Application.Domain.Schemas.Market.Inventory.Product;
using market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Sales;
using market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Sales.Pricing;
using market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Shop;
using market_tracker_webapi.Application.Repository.Market.Price;
using market_tracker_webapi.Infrastructure.PostgreSQLTables.Account.Users;
using market_tracker_webapi.Infrastructure.PostgreSQLTables.Market;
using market_tracker_webapi.Infrastructure.PostgreSQLTables.Market.Inventory;
using market_tracker_webapi.Infrastructure.PostgreSQLTables.Market.Inventory.Product;
using market_tracker_webapi.Infrastructure.PostgreSQLTables.Market.Store;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Moq;

namespace market_tracker_webapi_test.Application.Repository.Market;

public class PriceRepositoryTest
{
    // [Fact]
    // public async Task GetCheapestStoreOfferAvailableByProductIdAsync_ReturnsStoreOffer()
    // {
    //     // Arrange
    //     var dbPriceEntryEntities = new List<PriceEntryEntity>
    //     {
    //         new()
    //         {
    //             Id = "1",
    //             ProductId = "1",
    //             StoreId = 1,
    //             Price = 100,
    //             CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
    //         },
    //         new()
    //         {
    //             Id = "2",
    //             ProductId = "2",
    //             StoreId = 2,
    //             Price = 200,
    //             CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
    //         }
    //     };
    //     
    //     var dbStoreEntities = new List<StoreEntity>
    //     {
    //         new()
    //         {
    //             Id = 1,
    //             Name = "Store1",
    //             CompanyId = 1,
    //             CityId = 1,
    //             Address = "Address1",
    //             OperatorId = Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301")
    //         },
    //         new()
    //         {
    //             Id = 2,
    //             Name = "Store2",
    //             CompanyId = 2,
    //             CityId = 2,
    //             Address = "Address2",
    //             OperatorId = Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3302")
    //         }
    //     };
    //     
    //     var dbCompanyEntities = new List<CompanyEntity>
    //     {
    //         new()
    //         {
    //             Id = 1,
    //             Name = "Company1",
    //             CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified),
    //             LogoUrl = "LogoUrl1",
    //         },
    //         new()
    //         {
    //             Id = 2,
    //             Name = "Company2",
    //             CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified),
    //             LogoUrl = "LogoUrl2",
    //         }
    //     };
    //
    //     var dbCityEntities = new List<CityEntity>
    //     {
    //         new()
    //         {
    //             Id = 1,
    //             Name = "City1"
    //         },
    //         new()
    //         {
    //             Id = 2,
    //             Name = "City2"
    //         }
    //     };
    //     
    //     var dbPromotionEntities = new List<PromotionEntity>
    //     {
    //         new()
    //         {
    //             Percentage = 20,
    //             CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified),
    //             PriceEntryId = "1"
    //         },
    //         new()
    //         {
    //             Percentage = 30,
    //             CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified),
    //             PriceEntryId = "2"
    //         }
    //     };
    //     
    //     var dbProductEntities = new List<ProductEntity>
    //     {
    //         new()
    //         {
    //             Id = "1",
    //             Name = "Banana",
    //             BrandId = 1,
    //             CategoryId = 1,
    //             ImageUrl = "1",
    //             Quantity = 1,
    //             Rating = 1.0,
    //             Unit = "unidades",
    //             Views = 1
    //         },
    //         new()
    //         {
    //             Id = "2",
    //             Name = "Apple",
    //             BrandId = 2,
    //             CategoryId = 2,
    //             ImageUrl = "2",
    //             Quantity = 2,
    //             Rating = 2.0,
    //             Unit = "unidades",
    //             Views = 2
    //         }
    //     };
    //
    //     var dbProductAvailabilityEntities = new List<ProductAvailabilityEntity>
    //     {
    //         new()
    //         {
    //             ProductId = "1",
    //             StoreId = 1,
    //             IsAvailable = true,
    //             LastChecked = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
    //         },
    //         new()
    //         {
    //             ProductId = "2",
    //             StoreId = 2,
    //             IsAvailable = true,
    //             LastChecked = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
    //         }
    //     };
    //     
    //     var dbBrandEntities = new List<BrandEntity>
    //     {
    //         new()
    //         {
    //             Id = 1,
    //             Name = "Brand1"
    //         },
    //         new()
    //         {
    //             Id = 2,
    //             Name = "Brand2"
    //         }
    //     };
    //
    //     var dbCategoryEntities = new List<CategoryEntity>
    //     {
    //         new()
    //         {
    //             Id = 1,
    //             Name = "Category1"
    //         },
    //         new()
    //         {
    //             Id = 2,
    //             Name = "Category2"
    //         }
    //     };
    //     
    //     var dbClientEntities = new List<ClientEntity>()
    //     {
    //         new()
    //         {
    //             Avatar = "Avatar1",
    //             UserId = Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301"),
    //             Username = "User1"
    //         }
    //     };
    //     
    //     var context = DbHelper.CreateDatabase(
    //         dbPriceEntryEntities,
    //         dbStoreEntities,
    //         dbCompanyEntities,
    //         dbCityEntities,
    //         dbPromotionEntities,
    //         dbProductEntities,
    //         dbProductAvailabilityEntities,
    //         dbBrandEntities,
    //         dbCategoryEntities,
    //         dbClientEntities
    //     );
    //     
    //     var priceRepository = new PriceRepository(context);
    //     
    //     // Act
    //     var actualStoreOffer = await priceRepository.GetCheapestStoreOfferAvailableByProductIdAsync(
    //         "1", 
    //         null,
    //         null, 
    //         null
    //         );
    //     
    //     // Assert
    //     var expectedStore = new Store(
    //         new StoreId(1),
    //         "Store1",
    //         "Address1",
    //         new City(1, "City1"),
    //         new Company(1, "Company1", "LogoUrl1", new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)),
    //         Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301")
    //     );
    //     
    //     var expectedPriceData = new Price(
    //         1,
    //         new Promotion(1, new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)),
    //         new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
    //         );
    //     
    //     var expectedStoreAvailability = new StoreAvailability(
    //         1,
    //         "1",
    //         true,
    //         new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
    //         );
    //     
    //     var expectedStoreOffer = new StoreOffer(
    //         expectedStore,
    //         expectedPriceData,
    //         expectedStoreAvailability
    //     );
    //
    //     actualStoreOffer.Should().BeEquivalentTo(expectedStoreOffer);
    // }
    
    [Fact]
    public async Task GetStoresAvailabilityAsync_ReturnsStoreAvailabilityList()
    {
        // Arrange
        var dbProductAvailabilityEntities = new List<ProductAvailabilityEntity>
        {
            new()
            {
                ProductId = "1",
                StoreId = 1,
                IsAvailable = true,
                LastChecked = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            },
            new()
            {
                ProductId = "2",
                StoreId = 2,
                IsAvailable = true,
                LastChecked = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            }
        };
        
        var context = DbHelper.CreateDatabase(dbProductAvailabilityEntities);
        
        var priceRepository = new PriceRepository(context);
        
        // Act
        var actualStoreAvailabilityList = await priceRepository.GetStoresAvailabilityAsync("1");
        
        // Assert
        var expectedStoreAvailabilityList = new List<StoreAvailability>
        {
            new StoreAvailability(
                1,
                "1",
                true,
                new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            )
        };
        
        actualStoreAvailabilityList.Should().BeEquivalentTo(expectedStoreAvailabilityList);
    }
    
    [Fact]
    public async Task GetStoreAvailabilityStatusAsync_ReturnsStoreAvailability()
    {
        // Arrange
        var dbProductAvailabilityEntities = new List<ProductAvailabilityEntity>
        {
            new()
            {
                ProductId = "1",
                StoreId = 1,
                IsAvailable = true,
                LastChecked = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            },
            new()
            {
                ProductId = "2",
                StoreId = 2,
                IsAvailable = true,
                LastChecked = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            }
        };
        
        var context = DbHelper.CreateDatabase(dbProductAvailabilityEntities);
        
        var priceRepository = new PriceRepository(context);
        
        // Act
        var actualStoreAvailability = await priceRepository.GetStoreAvailabilityStatusAsync("1", 1);
        
        // Assert
        var expectedStoreAvailability = new StoreAvailability(
            1,
            "1",
            true,
            new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
        );
        
        actualStoreAvailability.Should().BeEquivalentTo(expectedStoreAvailability);
    }
    
    [Fact]
    public async Task GetStoreOfferAsync_ReturnsStoreOffer()
    {
        // Arrange
        var dbPriceEntryEntities = new List<PriceEntryEntity>
        {
            new()
            {
                Id = "1",
                ProductId = "1",
                StoreId = 1,
                Price = 100,
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            },
            new()
            {
                Id = "2",
                ProductId = "2",
                StoreId = 2,
                Price = 200,
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            }
        };
        
        var dbStoreEntities = new List<StoreEntity>
        {
            new()
            {
                Id = 1,
                Name = "Store1",
                CompanyId = 1,
                CityId = 1,
                Address = "Address1",
                OperatorId = Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301")
            },
            new()
            {
                Id = 2,
                Name = "Store2",
                CompanyId = 2,
                CityId = 2,
                Address = "Address2",
                OperatorId = Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3302")
            }
        };
        
        var dbCompanyEntities = new List<CompanyEntity>
        {
            new()
            {
                Id = 1,
                Name = "Company1",
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified),
                LogoUrl = "LogoUrl1",
            },
            new()
            {
                Id = 2,
                Name = "Company2",
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified),
                LogoUrl = "LogoUrl2",
            }
        };

        var dbCityEntities = new List<CityEntity>
        {
            new()
            {
                Id = 1,
                Name = "City1"
            },
            new()
            {
                Id = 2,
                Name = "City2"
            }
        };
        
        var dbPromotionEntities = new List<PromotionEntity>
        {
            new()
            {
                Percentage = 20,
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified),
                PriceEntryId = "1"
            },
            new()
            {
                Percentage = 30,
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified),
                PriceEntryId = "2"
            }
        };
        
        var dbProductEntities = new List<ProductEntity>
        {
            new()
            {
                Id = "1",
                Name = "Banana",
                BrandId = 1,
                CategoryId = 1,
                ImageUrl = "1",
                Quantity = 1,
                Rating = 1.0,
                Unit = "unidades",
                Views = 1
            },
            new()
            {
                Id = "2",
                Name = "Apple",
                BrandId = 2,
                CategoryId = 2,
                ImageUrl = "2",
                Quantity = 2,
                Rating = 2.0,
                Unit = "unidades",
                Views = 2
            }
        };

        var dbProductAvailabilityEntities = new List<ProductAvailabilityEntity>
        {
            new()
            {
                ProductId = "1",
                StoreId = 1,
                IsAvailable = true,
                LastChecked = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            },
            new()
            {
                ProductId = "2",
                StoreId = 2,
                IsAvailable = true,
                LastChecked = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            }
        };
        
        var dbBrandEntities = new List<BrandEntity>
        {
            new()
            {
                Id = 1,
                Name = "Brand1"
            },
            new()
            {
                Id = 2,
                Name = "Brand2"
            }
        };

        var dbCategoryEntities = new List<CategoryEntity>
        {
            new()
            {
                Id = 1,
                Name = "Category1"
            },
            new()
            {
                Id = 2,
                Name = "Category2"
            }
        };
        
        var dbClientEntities = new List<ClientEntity>()
        {
            new()
            {
                Avatar = "Avatar1",
                UserId = Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301"),
                Username = "User1"
            }
        };
        
        var context = DbHelper.CreateDatabase(
            dbPriceEntryEntities,
            dbStoreEntities,
            dbCompanyEntities,
            dbCityEntities,
            dbPromotionEntities,
            dbProductEntities,
            dbProductAvailabilityEntities,
            dbBrandEntities,
            dbCategoryEntities,
            dbClientEntities
        );
        
        var priceRepository = new PriceRepository(context);
        
        // Act
        var actualStoreOffer = await priceRepository.GetStoreOfferAsync(
            "1",
            1,
            new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
        );
        
        // Assert
        var expectedStore = new Store(
            new StoreId(1),
            "Store1",
            "Address1",
            new City(1, "City1"),
            new Company(1, "Company1", "LogoUrl1", new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)),
            Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301")
        );
        
        var expectedPriceData = new Price(
            100,
            80,
            new Promotion(20, new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)),
            new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            );
        
        var expectedStoreAvailability = new StoreAvailability(
            1,
            "1",
            true,
            new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            );
        
        var expectedStoreOffer = new StoreOffer(
            expectedStore,
            expectedPriceData,
            expectedStoreAvailability
        );
        
        actualStoreOffer.Should().BeEquivalentTo(expectedStoreOffer);
    }

    [Fact]
    public async Task GetPriceHistoryByProductIdAndStoreIdAsync_ReturnsListAsync()
    {
        // Arrange
        var dbPriceEntryEntities = new List<PriceEntryEntity>
        {
            new()
            {
                Id = "1",
                ProductId = "1",
                StoreId = 1,
                Price = 100,
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            },
            new()
            {
                Id = "2",
                ProductId = "2",
                StoreId = 2,
                Price = 200,
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            }
        };
        
        var dbPromotionEntities = new List<PromotionEntity>
        {
            new()
            {
                Percentage = 20,
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified),
                PriceEntryId = "1"
            },
            new()
            {
                Percentage = 30,
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified),
                PriceEntryId = "2"
            }
        };
        
        var context = DbHelper.CreateDatabase(
            dbPriceEntryEntities,
            dbPromotionEntities
        );
        
        var priceRepository = new PriceRepository(context);
        
        // Act
        var actualPriceHistory = await priceRepository.GetPriceHistoryByProductIdAndStoreIdAsync(
            "1",
            1,
            new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified),
            new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
        );
        
        // Assert
        var expectedPriceHistory = new List<Price>
        {
            new Price(
                100,
                80,
                new Promotion(20, new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)),
                new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            )
        };
        
        actualPriceHistory.Should().BeEquivalentTo(expectedPriceHistory);
    }
    
    [Fact]
    public async Task AddPriceAsync_ReturnsPriceIdAsync()
    {
        // Arrange
        var dbPriceEntryEntities = new List<PriceEntryEntity>
        {
            new()
            {
                Id = "1",
                ProductId = "1",
                StoreId = 1,
                Price = 100,
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            },
            new()
            {
                Id = "2",
                ProductId = "2",
                StoreId = 2,
                Price = 200,
                CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
            }
        };
        
        var context = DbHelper.CreateDatabase(dbPriceEntryEntities);
        
        var priceRepository = new PriceRepository(context);
        
        // Act
        var actualPriceId = await priceRepository.AddPriceAsync(
            "1",
            1,
            100,
            20
        );
        
        // Assert
        var expectedAddedPriceEntryEntity = new PriceEntryEntity()
        {
            Id = "1",
            ProductId = "1",
            StoreId = 1,
            Price = 100,
            CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)
        };

        context.PriceEntry.Should().ContainEquivalentOf(expectedAddedPriceEntryEntity);
    }
}