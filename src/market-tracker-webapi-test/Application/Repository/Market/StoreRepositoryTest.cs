using FluentAssertions;
using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables;
using Microsoft.EntityFrameworkCore;

/*
namespace market_tracker_webapi_test.Application.Repository
{
    public class StoreRepositoryTest
    {
        [Fact]
        public async Task GetStoresAsync_WhenStoresExist_ReturnsStoreData()
        {
            // Arrange
            var companyMockEntities = new List<CompanyEntity>
            {
                new() { Id = 1, Name = "Company1" }
            };

            var cityMockEntities = new List<CityEntity>
            {
                new() { Id = 1, Name = "Lisboa" },
                new() { Id = 2, Name = "Amadora" },
                new() { Id = 3, Name = "Oeiras" }
            };

            var storeMockEntities = new List<StoreEntity>
            {
                new()
                {
                    Id = 1,
                    Name = "Store1",
                    Address = "Address1",
                    CityId = 1,
                    CompanyId = 1
                },
                new()
                {
                    Id = 2,
                    Name = "Store2",
                    Address = "Address2",
                    CityId = 2,
                    CompanyId = 1
                },
                new()
                {
                    Id = 3,
                    Name = "Store3",
                    Address = "Address3",
                    CityId = 3,
                    CompanyId = 1
                }
            };

            var expectedStores = new List<Store>
            {
                new Store()
                {
                    Name = "Store1",
                    Address = "Address1",
                    CityId = 1,
                    CompanyId = 1
                },
                new Store()
                {
                    Name = "Store2",
                    Address = "Address2",
                    CityId = 2,
                    CompanyId = 1
                },
                new Store()
                {
                    Name = "Store3",
                    Address = "Address3",
                    CityId = 3,
                    CompanyId = 1
                }
            };

            var context = CreateDatabase(storeMockEntities, companyMockEntities, cityMockEntities);
            var storeRepository = new StoreRepository(context);

            // Act
            var storeData = await storeRepository.GetStoresAsync();

            // Assert
            storeData
                .Should()
                .BeEquivalentTo(expectedStores, options => options.Excluding(x => x.Id));
        }

        [Fact]
        public async Task GetStoresAsync_WhenStoresDoNotExist_ReturnsEmptyList()
        {
            // Arrange
            var context = CreateDatabase(
                new List<StoreEntity>(),
                new List<CompanyEntity>(),
                new List<CityEntity>()
            );

            var storeRepository = new StoreRepository(context);

            // Act
            var storeData = await storeRepository.GetStoresAsync();

            // Assert
            storeData.Should().BeEmpty();
        }

        [Fact]
        public async Task GetStoreByIdAsync_WhenStoreExists_ReturnsStoreData()
        {
            // Arrange
            var companyMockEntities = new List<CompanyEntity>
            {
                new() { Id = 1, Name = "Company1" }
            };

            var cityMockEntities = new List<CityEntity>
            {
                new() { Id = 1, Name = "Lisboa" },
                new() { Id = 2, Name = "Amadora" },
                new() { Id = 3, Name = "Oeiras" }
            };

            var storeMockEntities = new List<StoreEntity>
            {
                new()
                {
                    Id = 1,
                    Name = "Store1",
                    Address = "Address1",
                    CityId = 1,
                    CompanyId = 1
                },
                new()
                {
                    Id = 2,
                    Name = "Store2",
                    Address = "Address2",
                    CityId = 2,
                    CompanyId = 1
                },
                new()
                {
                    Id = 3,
                    Name = "Store3",
                    Address = "Address3",
                    CityId = 3,
                    CompanyId = 1
                }
            };

            var expectedStore = new Store()
            {
                Name = "Store1",
                Address = "Address1",
                CityId = 1,
                CompanyId = 1
            };

            var context = CreateDatabase(storeMockEntities, companyMockEntities, cityMockEntities);
            var storeRepository = new StoreRepository(context);

            // Act
            var storeData = await storeRepository.GetStoreByIdAsync(1);

            // Assert
            storeData
                .Should()
                .BeEquivalentTo(expectedStore, options => options.Excluding(x => x.Id));
        }

        [Fact]
        public async Task GetStoreByIdAsync_WhenStoreDoesNotExist_ReturnNull()
        {
            // Arrange
            var context = CreateDatabase(
                new List<StoreEntity>(),
                new List<CompanyEntity>(),
                new List<CityEntity>()
            );

            var storeRepository = new StoreRepository(context);

            // Act
            var storeData = await storeRepository.GetStoreByIdAsync(1);

            // Assert
            storeData.Should().BeNull();
        }

        [Fact]
        public async Task GetStoreByNameAsync_WhenStoreExists_ReturnsStoreData()
        {
            // Arrange
            var companyMockEntities = new List<CompanyEntity>
            {
                new() { Id = 1, Name = "Company1" }
            };

            var cityMockEntities = new List<CityEntity>
            {
                new() { Id = 1, Name = "Lisboa" },
                new() { Id = 2, Name = "Amadora" },
                new() { Id = 3, Name = "Oeiras" }
            };

            var storeMockEntities = new List<StoreEntity>
            {
                new()
                {
                    Id = 1,
                    Name = "Store1",
                    Address = "Address1",
                    CityId = 1,
                    CompanyId = 1
                },
                new()
                {
                    Id = 2,
                    Name = "Store2",
                    Address = "Address2",
                    CityId = 2,
                    CompanyId = 1
                },
                new()
                {
                    Id = 3,
                    Name = "Store3",
                    Address = "Address3",
                    CityId = 3,
                    CompanyId = 1
                }
            };

            var expectedStore = new Store()
            {
                Name = "Store1",
                Address = "Address1",
                CityId = 1,
                CompanyId = 1
            };

            var context = CreateDatabase(storeMockEntities, companyMockEntities, cityMockEntities);
            var storeRepository = new StoreRepository(context);

            // Act
            var storeData = await storeRepository.GetStoreByNameAsync("Store1");

            // Assert
            storeData
                .Should()
                .BeEquivalentTo(expectedStore, options => options.Excluding(x => x.Id));
        }

        [Fact]
        public async Task GetStoreByNameAsync_WhenStoreDoesNotExist_ReturnNull()
        {
            // Arrange
            var context = CreateDatabase(
                new List<StoreEntity>(),
                new List<CompanyEntity>(),
                new List<CityEntity>()
            );

            var storeRepository = new StoreRepository(context);

            // Act
            var storeData = await storeRepository.GetStoreByNameAsync("Store1");

            // Assert
            storeData.Should().BeNull();
        }

        [Fact]
        public async Task GetStoreByAddressAsync_WhenStoreExists_ReturnsStoreData()
        {
            // Arrange
            var companyMockEntities = new List<CompanyEntity>
            {
                new() { Id = 1, Name = "Company1" }
            };

            var cityMockEntities = new List<CityEntity>
            {
                new() { Id = 1, Name = "Lisboa" },
                new() { Id = 2, Name = "Amadora" },
                new() { Id = 3, Name = "Oeiras" }
            };

            var storeMockEntities = new List<StoreEntity>
            {
                new()
                {
                    Id = 1,
                    Name = "Store1",
                    Address = "Address1",
                    CityId = 1,
                    CompanyId = 1
                },
                new()
                {
                    Id = 2,
                    Name = "Store2",
                    Address = "Address2",
                    CityId = 2,
                    CompanyId = 1
                },
                new()
                {
                    Id = 3,
                    Name = "Store3",
                    Address = "Address3",
                    CityId = 3,
                    CompanyId = 1
                }
            };

            var expectedStore = new Store()
            {
                Name = "Store1",
                Address = "Address1",
                CityId = 1,
                CompanyId = 1
            };

            var context = CreateDatabase(storeMockEntities, companyMockEntities, cityMockEntities);
            var storeRepository = new StoreRepository(context);

            // Act
            var storeData = await storeRepository.GetStoreByAddressAsync("Address1");

            // Assert
            storeData
                .Should()
                .BeEquivalentTo(expectedStore, options => options.Excluding(x => x.Id));
        }

        [Fact]
        public async Task GetStoreByAddressAsync_WhenStoreDoesNotExist_ReturnNull()
        {
            // Arrange
            var context = CreateDatabase(
                new List<StoreEntity>(),
                new List<CompanyEntity>(),
                new List<CityEntity>()
            );

            var storeRepository = new StoreRepository(context);

            // Act
            var storeData = await storeRepository.GetStoreByAddressAsync("Address1");

            // Assert
            storeData.Should().BeNull();
        }

        [Fact]
        public async Task GetStoreFromCompanyAsync_WhenStoreExists_ReturnsStoreData()
        {
            // Arrange
            var companyMockEntities = new List<CompanyEntity>
            {
                new() { Id = 1, Name = "Company1" }
            };

            var cityMockEntities = new List<CityEntity>
            {
                new() { Id = 1, Name = "Lisboa" },
                new() { Id = 2, Name = "Amadora" },
                new() { Id = 3, Name = "Oeiras" }
            };

            var storeMockEntities = new List<StoreEntity>
            {
                new()
                {
                    Id = 1,
                    Name = "Store1",
                    Address = "Address1",
                    CityId = 1,
                    CompanyId = 1
                },
                new()
                {
                    Id = 2,
                    Name = "Store2",
                    Address = "Address2",
                    CityId = 2,
                    CompanyId = 1
                },
                new()
                {
                    Id = 3,
                    Name = "Store3",
                    Address = "Address3",
                    CityId = 3,
                    CompanyId = 1
                }
            };

            var expectedStores = new List<Store>
            {
                new Store()
                {
                    Name = "Store1",
                    Address = "Address1",
                    CityId = 1,
                    CompanyId = 1
                },
                new Store()
                {
                    Name = "Store2",
                    Address = "Address2",
                    CityId = 2,
                    CompanyId = 1
                },
                new Store()
                {
                    Name = "Store3",
                    Address = "Address3",
                    CityId = 3,
                    CompanyId = 1
                }
            };

            var context = CreateDatabase(storeMockEntities, companyMockEntities, cityMockEntities);
            var storeRepository = new StoreRepository(context);

            // Act
            var storeData = await storeRepository.GetStoresFromCompanyAsync(1);

            // Assert
            storeData
                .Should()
                .BeEquivalentTo(expectedStores, options => options.Excluding(x => x.Id));
        }

        [Fact]
        public async Task GetStoreFromCompanyAsync_WhenStoreDoesNotExist_ReturnsEmptyList()
        {
            // Arrange
            var context = CreateDatabase(
                new List<StoreEntity>(),
                new List<CompanyEntity>(),
                new List<CityEntity>()
            );

            var storeRepository = new StoreRepository(context);

            // Act
            var storeData = await storeRepository.GetStoresFromCompanyAsync(1);

            // Assert
            storeData.Should().BeEmpty();
        }

        [Fact]
        public async Task GetStoresByCityNameAsync_WhenStoresExist_ReturnsStoreData()
        {
            // Arrange
            var companyMockEntities = new List<CompanyEntity>
            {
                new() { Id = 1, Name = "Company1" }
            };

            var cityMockEntities = new List<CityEntity>
            {
                new() { Id = 1, Name = "Lisboa" },
                new() { Id = 2, Name = "Amadora" },
                new() { Id = 3, Name = "Oeiras" }
            };

            var storeMockEntities = new List<StoreEntity>
            {
                new()
                {
                    Id = 1,
                    Name = "Store1",
                    Address = "Address1",
                    CityId = 1,
                    CompanyId = 1
                },
                new()
                {
                    Id = 2,
                    Name = "Store2",
                    Address = "Address2",
                    CityId = 2,
                    CompanyId = 1
                },
                new()
                {
                    Id = 3,
                    Name = "Store3",
                    Address = "Address3",
                    CityId = 3,
                    CompanyId = 1
                }
            };

            var expectedStores = new List<Store>
            {
                new Store()
                {
                    Name = "Store1",
                    Address = "Address1",
                    CityId = 1,
                    CompanyId = 1
                }
            };

            var context = CreateDatabase(storeMockEntities, companyMockEntities, cityMockEntities);
            var storeRepository = new StoreRepository(context);

            // Act
            var storeData = await storeRepository.GetStoresByCityNameAsync("Lisboa");

            // Assert
            storeData
                .Should()
                .BeEquivalentTo(expectedStores, options => options.Excluding(x => x.Id));
        }

        [Fact]
        public async Task GetStoresByCityNameAsync_WhenStoresDoNotExist_ReturnsEmptyList()
        {
            // Arrange
            var context = CreateDatabase(
                new List<StoreEntity>(),
                new List<CompanyEntity>(),
                new List<CityEntity>()
            );

            var storeRepository = new StoreRepository(context);

            // Act
            var storeData = await storeRepository.GetStoresByCityNameAsync("Lisboa");

            // Assert
            storeData.Should().BeEmpty();
        }

        [Fact]
        public async Task GetStoreByCityNameAsync_WhenCityDoesNotExist_ReturnsEmptyList()
        {
            // Arrange
            var context = CreateDatabase(
                new List<StoreEntity>(),
                new List<CompanyEntity>(),
                new List<CityEntity>()
            );

            var storeRepository = new StoreRepository(context);

            // Act
            var storeData = await storeRepository.GetStoresByCityNameAsync("Lisboa");

            // Assert
            storeData.Should().BeEmpty();
        }

        [Fact]
        public async Task AddStoreAsync_WhenCompanyExists_ReturnsStoreId()
        {
            // Arrange
            var companyMockEntities = new List<CompanyEntity>
            {
                new() { Id = 1, Name = "Company1" }
            };

            var cityMockEntities = new List<CityEntity>
            {
                new() { Id = 1, Name = "Lisboa" },
                new() { Id = 2, Name = "Amadora" },
                new() { Id = 3, Name = "Oeiras" }
            };

            var storeMockEntities = new List<StoreEntity>
            {
                new()
                {
                    Id = 1,
                    Name = "Store1",
                    Address = "Address1",
                    CityId = 1,
                    CompanyId = 1
                },
                new()
                {
                    Id = 2,
                    Name = "Store2",
                    Address = "Address2",
                    CityId = 2,
                    CompanyId = 1
                },
                new()
                {
                    Id = 3,
                    Name = "Store3",
                    Address = "Address3",
                    CityId = 3,
                    CompanyId = 1
                }
            };

            var context = CreateDatabase(storeMockEntities, companyMockEntities, cityMockEntities);
            var storeRepository = new StoreRepository(context);

            var storeData = new Store()
            {
                Name = "Store4",
                Address = "Address4",
                CityId = 1,
                CompanyId = 1
            };

            // Act
            var storeId = await storeRepository.AddStoreAsync("Store4", "Address4", 1, 1);

            var storeEntity = new StoreEntity()
            {
                Name = storeData.Name,
                Address = storeData.Address,
                CityId = storeData.CityId,
                CompanyId = storeData.CompanyId
            };

            storeMockEntities.Add(storeEntity);

            // Assert
            context
                .Store.Should()
                .BeEquivalentTo(storeMockEntities, options => options.Excluding(x => x.Id));
            storeId.Should().Be(4);
        }

        [Fact]
        public async Task AddStoreAsync_WhenCityIsNull_ReturnsStoreData()
        {
            // Arrange
            var companyMockEntities = new List<CompanyEntity>
            {
                new() { Id = 1, Name = "Company1" }
            };

            var context = CreateDatabase(
                new List<StoreEntity>(),
                companyMockEntities,
                new List<CityEntity>()
            );

            var storeRepository = new StoreRepository(context);

            // Act
            var storeId = await storeRepository.AddStoreAsync("Store4", "Address4", 1, 1);

            // Assert
            storeId.Should().Be(1);
            context.Store.Should().ContainSingle();
        }

        [Fact]
        public async Task UpdateStoreAsync_WhenStoreAndCompanyExist_ReturnsUpdatedStoreData()
        {
            // Arrange
            var companyMockEntities = new List<CompanyEntity>
            {
                new() { Id = 1, Name = "Company1" }
            };

            var cityMockEntities = new List<CityEntity>
            {
                new() { Id = 1, Name = "Lisboa" },
                new() { Id = 2, Name = "Porto" },
            };

            var storeMockEntities = new List<StoreEntity>
            {
                new()
                {
                    Id = 1,
                    Name = "Store1",
                    Address = "Address1",
                    CityId = 1,
                    CompanyId = 1
                }
            };

            var context = CreateDatabase(storeMockEntities, companyMockEntities, cityMockEntities);
            var storeRepository = new StoreRepository(context);

            var storeData = new Store()
            {
                Id = 1,
                Name = "Store1",
                Address = "AddressA",
                CityId = 2,
                CompanyId = 1
            };

            // Act
            var actualStore = await storeRepository.UpdateStoreAsync(1, "AddressA", 2, 1);

            // Assert
            actualStore.Should().BeEquivalentTo(storeData);
            (await context.Store.FindAsync(storeData.Id)).Should().BeEquivalentTo(storeData, options => options.Excluding(x => x.IsOnline));
        }

        [Fact]
        public async Task UpdateStoreAsync_WhenStoreDoesNotExist_ReturnsNull()
        {
            // Arrange
            var companyMockEntities = new List<CompanyEntity>
            {
                new() { Id = 1, Name = "Company1" }
            };

            var cityMockEntities = new List<CityEntity>
            {
                new() { Id = 1, Name = "Lisboa" }
            };

            var context = CreateDatabase(
                new List<StoreEntity>(),
                companyMockEntities,
                cityMockEntities
            );
            var storeRepository = new StoreRepository(context);

            // Act
            var actualStore = await storeRepository.UpdateStoreAsync(1, "AddressA", 1, 1);

            // Assert
            actualStore.Should().BeNull();
        }

        [Fact]
        public async Task DeleteStoreAsync_WhenStoreExists_ReturnsDeletedStoreData()
        {
            // Arrange
            var companyMockEntities = new List<CompanyEntity>
            {
                new() { Id = 1, Name = "Company1" }
            };

            var cityMockEntities = new List<CityEntity>
            {
                new() { Id = 1, Name = "Lisboa" }
            };

            var storeMockEntities = new List<StoreEntity>
            {
                new()
                {
                    Id = 1,
                    Name = "Store1",
                    Address = "Address1",
                    CityId = 1,
                    CompanyId = 1
                }
            };

            var context = CreateDatabase(storeMockEntities, companyMockEntities, cityMockEntities);
            var storeRepository = new StoreRepository(context);

            // Act
            var actualStore = await storeRepository.DeleteStoreAsync(1);

            // Assert
            actualStore.Should().BeEquivalentTo(storeMockEntities[0]);
            (await context.Store.FindAsync(1)).Should().BeNull();
        }

        [Fact]
        public async Task DeleteStoreAsync_WhenStoreDoesNotExist_ReturnsNull()
        {
            // Arrange
            var context = CreateDatabase(
                new List<StoreEntity>(),
                new List<CompanyEntity>(),
                new List<CityEntity>()
            );
            var storeRepository = new StoreRepository(context);

            // Act
            var actualStore = await storeRepository.DeleteStoreAsync(1);

            // Assert
            actualStore.Should().BeNull();
        }

        private static MarketTrackerDataContext CreateDatabase(
            IEnumerable<StoreEntity> storeEntities,
            IEnumerable<CompanyEntity> companyEntities,
            IEnumerable<CityEntity> cityEntities
        )
        {
            DbContextOptions<MarketTrackerDataContext> options =
                new DbContextOptionsBuilder<MarketTrackerDataContext>()
                    .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                    .Options;

            var databaseContext = new MarketTrackerDataContext(options);
            databaseContext.Company.AddRange(companyEntities);
            databaseContext.City.AddRange(cityEntities);
            databaseContext.Store.AddRange(storeEntities);
            databaseContext.SaveChanges();
            databaseContext.Database.EnsureCreated();
            return databaseContext;
        }
    }
}
*/