
using FluentAssertions;
using market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Shop;
using market_tracker_webapi.Application.Repository.Market.Store;
using market_tracker_webapi.Infrastructure.PostgreSQLTables.Market;
using market_tracker_webapi.Infrastructure.PostgreSQLTables.Market.Store;

namespace market_tracker_webapi_test.Application.Repository.Market;

public class StoreRepositoryTest
{
    [Fact]
    public async Task GetStoreByIdAsync_WhenStoreExists_ReturnsStore()
    {
        // Arrange
        var storeEntities = new List<StoreEntity>
        {
            new StoreEntity
            {
                Id = 1, Name = "storeName", Address = "storeAddress", CityId = 1, CompanyId = 1,
                OperatorId = Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301")
            },
            new StoreEntity
            {
                Id = 2, Name = "storeName2", Address = "storeAddress2", CityId = 2, CompanyId = 2,
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


        var context = DbHelper.CreateDatabase(
            storeEntities,
            dbCompanyEntities,
            dbCityEntities);

        var storeRepository = new StoreRepository(context);

        // Act
        var actualStore = await storeRepository.GetStoreByIdAsync(1);

        // Assert
        var expectedStore = new Store(
            1,
            "storeName",
            "storeAddress",
            new City(1, "City1"),
            new Company(1, "Company1", "LogoUrl1", new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)),
            Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301"));

        actualStore.Should().BeEquivalentTo(expectedStore);
    }

    [Fact]
    public async Task GetStoreByIdAsync_WhenStoreDoesNotExist_ReturnsNull()
    {
        // Arrange
        var storeEntities = new List<StoreEntity>
        {
            new StoreEntity
            {
                Id = 1, Name = "storeName", Address = "storeAddress", CityId = 1, CompanyId = 1,
                OperatorId = Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301")
            },
            new StoreEntity
            {
                Id = 2, Name = "storeName2", Address = "storeAddress2", CityId = 2, CompanyId = 2,
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


        var context = DbHelper.CreateDatabase(
            storeEntities,
            dbCompanyEntities,
            dbCityEntities);

        var storeRepository = new StoreRepository(context);

        // Act
        var actualStore = await storeRepository.GetStoreByIdAsync(3);

        // Assert
        actualStore.Should().BeNull();
    }

    // [Fact]
    // public async Task GetStoreByNameAsync_WhenStoreExists_ReturnsStore()
    // {
    //     // Arrange
    //     var storeEntities = new List<StoreEntity>
    //     {
    //         new StoreEntity
    //         {
    //             Id = 1, Name = "storeName", Address = "storeAddress", CityId = 1, CompanyId = 1,
    //             OperatorId = Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301")
    //         },
    //         new StoreEntity
    //         {
    //             Id = 2, Name = "storeName2", Address = "storeAddress2", CityId = 2, CompanyId = 2,
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
    //
    //     var context = DbHelper.CreateDatabase(
    //         storeEntities,
    //         dbCompanyEntities,
    //         dbCityEntities);
    //
    //     var storeRepository = new StoreRepository(context);
    //
    //     // Act
    //     var actualStore = await storeRepository.GetStoreByNameAsync("storeName");
    //
    //     // Assert
    //     var expectedStore = new Store(
    //         1,
    //         "storeName",
    //         "storeAddress",
    //         new City(1, "City1"),
    //         new Company(1, "Company1", "LogoUrl1", new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)),
    //         Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301"));
    //
    //     actualStore.Should().BeEquivalentTo(expectedStore);
    // }

    [Fact]
    public async Task GetStoreByOperatorIdAsync_WhenStoreExists_ReturnsStore()
    {
        // Arrange
        var storeEntities = new List<StoreEntity>
        {
            new StoreEntity
            {
                Id = 1, Name = "storeName", Address = "storeAddress", CityId = 1, CompanyId = 1,
                OperatorId = Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301")
            },
            new StoreEntity
            {
                Id = 2, Name = "storeName2", Address = "storeAddress2", CityId = 2, CompanyId = 2,
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


        var context = DbHelper.CreateDatabase(
            storeEntities,
            dbCompanyEntities,
            dbCityEntities);

        var storeRepository = new StoreRepository(context);

        // Act
        var actualStore =
            await storeRepository.GetStoreByOperatorIdAsync(Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301"));

        // Assert
        var expectedStore = new Store(
            1,
            "storeName",
            "storeAddress",
            new City(1, "City1"),
            new Company(1, "Company1", "LogoUrl1", new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified)),
            Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301"));

        actualStore.Should().BeEquivalentTo(expectedStore);
    }

    [Fact]
    public async Task GetStoreByOperatorIdAsync_WhenStoreDoesNotExist_ReturnsNull()
    {
        // Arrange
        var storeEntities = new List<StoreEntity>
        {
            new StoreEntity
            {
                Id = 1, Name = "storeName", Address = "storeAddress", CityId = 1, CompanyId = 1,
                OperatorId = Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301")
            },
            new StoreEntity
            {
                Id = 2, Name = "storeName2", Address = "storeAddress2", CityId = 2, CompanyId = 2,
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

        var context = DbHelper.CreateDatabase(
            storeEntities,
            dbCompanyEntities,
            dbCityEntities);

        var storeRepository = new StoreRepository(context);

        // Act
        var actualStore =
            await storeRepository.GetStoreByOperatorIdAsync(Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3303"));

        // Assert
        actualStore.Should().BeNull();
    }

    [Fact]
    public async Task AddStoreAsync_WhenStoreDoesNotExist_AddsStore()
    {
        // Arrange
        var storeEntities = new List<StoreEntity>
        {
            new StoreEntity
            {
                Id = 1, Name = "storeName", Address = "storeAddress", CityId = 1, CompanyId = 1,
                OperatorId = Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301")
            },
            new StoreEntity
            {
                Id = 2, Name = "storeName2", Address = "storeAddress2", CityId = 2, CompanyId = 2,
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

        var context = DbHelper.CreateDatabase(
            storeEntities,
            dbCompanyEntities,
            dbCityEntities);

        var storeRepository = new StoreRepository(context);
        
        // Act
        var actualStoreId = await storeRepository.AddStoreAsync(
            "storeName3",
            "storeAddress3",
            3,
            3,
            Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3303"));
        
        // Assert
        actualStoreId.Should().BeEquivalentTo(new StoreId(3));
    }
    
    [Fact]
    public async Task UpdateStoreAsync_WhenStoreExists_UpdatesStore()
    {
        // Arrange
        var storeEntities = new List<StoreEntity>
        {
            new StoreEntity
            {
                Id = 1, Name = "storeName", Address = "storeAddress", CityId = 1, CompanyId = 1,
                OperatorId = Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301")
            },
            new StoreEntity
            {
                Id = 2, Name = "storeName2", Address = "storeAddress2", CityId = 2, CompanyId = 2,
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

        var context = DbHelper.CreateDatabase(
            storeEntities,
            dbCompanyEntities,
            dbCityEntities);

        var storeRepository = new StoreRepository(context);
        
        // Act
        var actualStoreItem = await storeRepository.UpdateStoreAsync(
            1,
            "storeAddress3",
            3,
            3);
        
        // Assert
        var expectedStoreItem = new StoreItem(
            1,
            "storeName",
            "storeAddress3",
            3,
            3,
            Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301"));
        
        actualStoreItem.Should().BeEquivalentTo(expectedStoreItem);
        
    }
    
    [Fact]
    public async Task UpdateStoreAsync_WhenStoreDoesNotExist_ReturnsNull()
    {
        // Arrange
        var storeEntities = new List<StoreEntity>
        {
            new StoreEntity
            {
                Id = 1, Name = "storeName", Address = "storeAddress", CityId = 1, CompanyId = 1,
                OperatorId = Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301")
            },
            new StoreEntity
            {
                Id = 2, Name = "storeName2", Address = "storeAddress2", CityId = 2, CompanyId = 2,
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

        var context = DbHelper.CreateDatabase(
            storeEntities,
            dbCompanyEntities,
            dbCityEntities);

        var storeRepository = new StoreRepository(context);
        
        // Act
        var actualStoreItem = await storeRepository.UpdateStoreAsync(
            3,
            "storeAddress3",
            3,
            3);
        
        // Assert
        actualStoreItem.Should().BeNull();
        
    }

    [Fact]
    public async Task DeleteStoreAsync_WhenStoreExists_DeletesStore()
    {
        // Arrange
        var storeEntities = new List<StoreEntity>
        {
            new StoreEntity
            {
                Id = 1, Name = "storeName", Address = "storeAddress", CityId = 1, CompanyId = 1,
                OperatorId = Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301")
            },
            new StoreEntity
            {
                Id = 2, Name = "storeName2", Address = "storeAddress2", CityId = 2, CompanyId = 2,
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

        var context = DbHelper.CreateDatabase(
            storeEntities,
            dbCompanyEntities,
            dbCityEntities);

        var storeRepository = new StoreRepository(context);

        // Act
        var actualStoreItem = await storeRepository.DeleteStoreAsync(1);

        // Assert
        var expectedStoreItem = new StoreItem(
            1,
            "storeName",
            "storeAddress",
            1,
            1,
            Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301"));
        
        actualStoreItem.Should().BeEquivalentTo(expectedStoreItem);
    }
}
