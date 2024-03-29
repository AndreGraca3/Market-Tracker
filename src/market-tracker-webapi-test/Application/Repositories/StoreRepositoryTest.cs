using FluentAssertions;
using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Exceptions;
using market_tracker_webapi.Application.Models;
using market_tracker_webapi.Application.Repositories.Store;
using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi_test.Application.Repositories
{
    public class StoreRepositoryTest
    {
        [Fact]
        public async Task GetStoresAsync_WhenStoresExist_ReturnsStoreData()
        {
            // Arrange
            var companyMockEntities = new List<CompanyEntity>
            {
                new ()
                {
                    Id = 1,
                    Name = "Company1"
                }
            };
            
            var cityMockEntities = new List<CityEntity>
            {
                new()
                {
                    Id = 1,
                    Name = "Lisboa"
                },
                new()
                {
                    Id = 2,
                    Name = "Amadora"
                },
                new()
                {
                    Id = 3,
                    Name = "Oeiras"
                }
            };
            
            var storeMockEntities = new List<StoreEntity>
            {
                new ()
                {
                    Id = 1, 
                    Address = "Address1",
                    CityId = 1,
                    CompanyId = 1
                },
                new ()
                {
                    Id = 2, 
                    Address = "Address2",
                    CityId = 2,
                    CompanyId = 1
                },
                new ()
                {
                    Id = 3, 
                    Address = "Address3",
                    CityId = 3,
                    CompanyId = 1
                }
            };

            var expectedStores = new List<StoreDomain>
            {
                new StoreDomain()
                {
                    Address = "Address1",
                    CityId = 1,
                    CompanyId = 1
                },
                new StoreDomain()
                {
                    Address = "Address2",
                    CityId = 2,
                    CompanyId = 1
                },
                new StoreDomain()
                {
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
            storeData.Should().BeEquivalentTo(expectedStores, options => options.Excluding(x => x.Id));
        }
        
        [Fact]
        public async Task GetStoresAsync_WhenStoresDoNotExist_ReturnsEmptyList()
        {
            // Arrange
            var context = CreateDatabase(
                new List<StoreEntity>(), 
                new List<CompanyEntity>(), 
                new List<CityEntity>());
            
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
                new ()
                {
                    Id = 1,
                    Name = "Company1"
                }
            };

            var cityMockEntities = new List<CityEntity>
            {
                new()
                {
                    Id = 1,
                    Name = "Lisboa"
                },
                new()
                {
                    Id = 2,
                    Name = "Amadora"
                },
                new()
                {
                    Id = 3,
                    Name = "Oeiras"
                }
            };
            
            var storeMockEntities = new List<StoreEntity>
            {
                new ()
                {
                    Id = 1, 
                    Address = "Address1",
                    CityId = 1,
                    CompanyId = 1
                },
                new ()
                {
                    Id = 2, 
                    Address = "Address2",
                    CityId = 2,
                    CompanyId = 1
                },
                new ()
                {
                    Id = 3, 
                    Address = "Address3",
                    CityId = 3,
                    CompanyId = 1
                }
            };

            var expectedStore = new StoreDomain()
            {
                Address = "Address1",
                CityId = 1,
                CompanyId = 1
            };
            
            var context = CreateDatabase(storeMockEntities, companyMockEntities, cityMockEntities);
            var storeRepository = new StoreRepository(context);

            // Act
            var storeData = await storeRepository.GetStoreByIdAsync(1);

            // Assert
            storeData.Should().BeEquivalentTo(expectedStore, options => options.Excluding(x => x.Id));
        }
        
        [Fact]
        public async Task GetStoreByIdAsync_WhenStoreDoesNotExist_ReturnNull()
        {
            // Arrange
            var context = CreateDatabase(
                new List<StoreEntity>(), 
                new List<CompanyEntity>(), 
                new List<CityEntity>());
            
            var storeRepository = new StoreRepository(context);
            
            // Act
            var storeData = await storeRepository.GetStoreByIdAsync(1);
            
            // Assert
            storeData.Should().BeNull();
        }
        
        [Fact]
        public async Task GetStoreByAddressAsync_WhenStoreExists_ReturnsStoreData()
        {
            // Arrange
            var companyMockEntities = new List<CompanyEntity>
            {
                new ()
                {
                    Id = 1,
                    Name = "Company1"
                }
            };

            var cityMockEntities = new List<CityEntity>
            {
                new()
                {
                    Id = 1,
                    Name = "Lisboa"
                },
                new()
                {
                    Id = 2,
                    Name = "Amadora"
                },
                new()
                {
                    Id = 3,
                    Name = "Oeiras"
                }
            };
            
            var storeMockEntities = new List<StoreEntity>
            {
                new ()
                {
                    Id = 1, 
                    Address = "Address1",
                    CityId = 1,
                    CompanyId = 1
                },
                new ()
                {
                    Id = 2, 
                    Address = "Address2",
                    CityId = 2,
                    CompanyId = 1
                },
                new ()
                {
                    Id = 3, 
                    Address = "Address3",
                    CityId = 3,
                    CompanyId = 1
                }
            };

            var expectedStore = new StoreDomain()
            {
                Address = "Address1",
                CityId = 1,
                CompanyId = 1
            };
            
            var context = CreateDatabase(storeMockEntities, companyMockEntities, cityMockEntities);
            var storeRepository = new StoreRepository(context);

            // Act
            var storeData = await storeRepository.GetStoreByAddressAsync("Address1");

            // Assert
            storeData.Should().BeEquivalentTo(expectedStore, options => options.Excluding(x => x.Id));
        }
        
        [Fact]
        public async Task GetStoreByAddressAsync_WhenStoreDoesNotExist_ReturnNull()
        {
            // Arrange
            var context = CreateDatabase(
                new List<StoreEntity>(), 
                new List<CompanyEntity>(), 
                new List<CityEntity>());
            
            var storeRepository = new StoreRepository(context);
            
            // Act
            var storeData = await storeRepository.GetStoreByAddressAsync("Address1");
            
            // Assert
            storeData.Should().BeNull();
        }
        
        [Fact]
        public async Task AddStoreAsync_WhenCompanyExists_ReturnsStoreId()
        {
            // Arrange
            var companyMockEntities = new List<CompanyEntity>
            {
                new ()
                {
                    Id = 1,
                    Name = "Company1"
                }
            };
            
            var cityMockEntities = new List<CityEntity>
            {
                new()
                {
                    Id = 1,
                    Name = "Lisboa"
                },
                new()
                {
                    Id = 2,
                    Name = "Amadora"
                },
                new()
                {
                    Id = 3,
                    Name = "Oeiras"
                }
            };
            
            var storeMockEntities = new List<StoreEntity>
            {
                new ()
                {
                    Id = 1, 
                    Address = "Address1",
                    CityId = 1,
                    CompanyId = 1
                },
                new ()
                {
                    Id = 2, 
                    Address = "Address2",
                    CityId = 2,
                    CompanyId = 1
                },
                new ()
                {
                    Id = 3, 
                    Address = "Address3",
                    CityId = 3,
                    CompanyId = 1
                }
            };

            var context = CreateDatabase(storeMockEntities, companyMockEntities, cityMockEntities);
            var storeRepository = new StoreRepository(context);

            var storeData = new StoreDomain()
            {
                Address = "Address4",
                CityId = 1,
                CompanyId = 1
            };
            
            // Act
            var storeId = await storeRepository.AddStoreAsync("Address4", 1, 1);
            
            var storeEntity = new StoreEntity()
            {
                Address = storeData.Address,
                CityId = storeData.CityId,
                CompanyId = storeData.CompanyId
            };
            
            storeMockEntities.Add(storeEntity);
            
            // Assert
            context.Store.Should().BeEquivalentTo(storeMockEntities, options => options.Excluding(x => x.Id));
            storeId.Should().Be(4);
        }

        // [Fact]
        // public async Task AddStoreAsync_WhenCompanyDoesNotExist_ReturnsNull()
        // {
        //     // Arrange
        //     var cityMockEntities = new List<CityEntity>
        //     {
        //         new()
        //         {
        //             Id = 1,
        //             Name = "Lisboa"
        //         }
        //     };
        //     
        //     var context = CreateDatabase(new List<StoreEntity>(), new List<CompanyEntity>(), cityMockEntities);
        //     var storeRepository = new StoreRepository(context);
        //     
        //     var storeData = new StoreDomain()
        //     {
        //         Address = "Address4",
        //         CityId = 1,
        //         CompanyId = 1
        //     };
        //     
        //     // Act
        //     var storeId = await storeRepository.AddStoreAsync("Address4", 1, 1);
        //     
        //     // Assert
        //     storeId.Should().BeNull();
        // }

        [Fact]
        public async Task AddStoreAsync_WhenCityIsNull_ReturnsStoreData()
        {
            // Arrange
            var companyMockEntities = new List<CompanyEntity>
            {
                new ()
                {
                    Id = 1,
                    Name = "Company1"
                }
            };
            
            var context = CreateDatabase(
                new List<StoreEntity>(), 
                companyMockEntities,
                new List<CityEntity>());
            
            var storeRepository = new StoreRepository(context);
            
            var storeData = new StoreDomain()
            {
                Address = "Address4",
                CityId = 1,
                CompanyId = 1
            };
            
            // Act
            var storeId = await storeRepository.AddStoreAsync("Address4", 1, 1);
            
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
                new()
                {
                    Id = 1,
                    Name = "Company1"
                }
            };
            
            var cityMockEntities = new List<CityEntity>
            {
                new()
                {
                    Id = 1,
                    Name = "Lisboa"
                },
                new()
                {
                    Id = 2,
                    Name = "Porto"
                },
            };

            var storeMockEntities = new List<StoreEntity>
            {
                new()
                {
                    Id = 1,
                    Address = "Address1",
                    CityId = 1,
                    CompanyId = 1
                }
            };

            var context = CreateDatabase(storeMockEntities, companyMockEntities, cityMockEntities);
            var storeRepository = new StoreRepository(context);

            var storeData = new StoreDomain()
            {
                Id = 1,
                Address = "AddressA",
                CityId = 2,
                CompanyId = 1
            };

            // Act
            var actualStore = await storeRepository.UpdateStoreAsync(1, "AddressA", 2, 1);
            
            // Assert
            actualStore.Should().BeEquivalentTo(storeData);
            (await context.Store.FindAsync(storeData.Id)).Should().BeEquivalentTo(storeData);
        }

        [Fact]
        public async Task UpdateStoreAsync_WhenStoreDoesNotExist_ReturnsNull()
        {
            // Arrange
            var companyMockEntities = new List<CompanyEntity>
            {
                new()
                {
                    Id = 1,
                    Name = "Company1"
                }
            };
            
            var cityMockEntities = new List<CityEntity>
            {
                new()
                {
                    Id = 1,
                    Name = "Lisboa"
                }
            };
            
            var context = CreateDatabase(new List<StoreEntity>(), companyMockEntities, cityMockEntities);
            var storeRepository = new StoreRepository(context);

            var storeData = new StoreDomain()
            {
                Id = 1,
                Address = "AddressA",
                CityId = 1,
                CompanyId = 1
            };
            
            // Act
            var actualStore = await storeRepository.UpdateStoreAsync(1, "AddressA", 1, 1);
            
            // Assert
            actualStore.Should().BeNull();
        }
        
        [Fact]
        public async Task UpdateStoreAsync_WhenCompanyDoesNotExist_ReturnsNull()
        {
            // Arrange
            var companyMockEntities = new List<CompanyEntity>
            {
                new()
                {
                    Id = 1,
                    Name = "Company1"
                }
            };
            
            var cityMockEntities = new List<CityEntity>
            {
                new()
                {
                    Id = 1,
                    Name = "Lisboa"
                },
                new()
                {
                    Id = 2,
                    Name = "Porto"
                },
            };

            var storeMockEntities = new List<StoreEntity>
            {
                new()
                {
                    Id = 1,
                    Address = "Address1",
                    CityId = 1,
                    CompanyId = 1
                }
            };
            
            var context = CreateDatabase(storeMockEntities, companyMockEntities, cityMockEntities);
            var storeRepository = new StoreRepository(context);

            var storeData = new StoreDomain()
            {
                Id = 1,
                Address = "AddressA",
                CityId = 2,
                CompanyId = 2
            };

            // Act
            var actualStore = await storeRepository.UpdateStoreAsync(1, "AddressA", 2, 2);
            
            // Assert
            actualStore.Should().BeNull();
        }

        // [Fact]
        // public async Task UpdateStoreAsync_WhenCityIsNull_ReturnsStoreData()
        // {
        //     var companyMockEntities = new List<CompanyEntity>
        //     {
        //         new()
        //         {
        //             Id = 1,
        //             Name = "Company1"
        //         }
        //     };
        //     
        //     var cityMockEntities = new List<CityEntity>
        //     {
        //         new()
        //         {
        //             Id = 1,
        //             Name = "Lisboa"
        //         }
        //     };
        //     
        //     var storeMockEntities = new List<StoreEntity>
        //     {
        //         new()
        //         {
        //             Id = 1,
        //             Address = "Address1",
        //             CityId = 1,
        //             CompanyId = 1
        //         }
        //     };
        //     
        //     var context = CreateDatabase(storeMockEntities, companyMockEntities, cityMockEntities);
        //     
        //     var storeRepository = new StoreRepository(context);
        //     
        //     var storeData = new StoreDomain()
        //     {
        //         Id = 1,
        //         Address = "AddressA",
        //         CityId = null,
        //         CompanyId = 1
        //     };
        //     
        //     var actualStore = await storeRepository.UpdateStoreAsync(1, "AddressA", null, 1);
        //     
        //     actualStore.Should().BeEquivalentTo(storeData);
        // }
        
        [Fact]
        public async Task DeleteStoreAsync_WhenStoreExists_ReturnsDeletedStoreData()
        {
            // Arrange
            var companyMockEntities = new List<CompanyEntity>
            {
                new()
                {
                    Id = 1,
                    Name = "Company1"
                }
            };
            
            var cityMockEntities = new List<CityEntity>
            {
                new()
                {
                    Id = 1,
                    Name = "Lisboa"
                }
            };

            var storeMockEntities = new List<StoreEntity>
            {
                new()
                {
                    Id = 1,
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
            var context = CreateDatabase(new List<StoreEntity>(), new List<CompanyEntity>(), new List<CityEntity>());
            var storeRepository = new StoreRepository(context);
            
            // Act
            var actualStore = await storeRepository.DeleteStoreAsync(1);
            
            // Assert
            actualStore.Should().BeNull();
        }
        
        private static MarketTrackerDataContext CreateDatabase(
            IEnumerable<StoreEntity> storeEntities,
            IEnumerable<CompanyEntity> companyEntities,
                IEnumerable<CityEntity> cityEntities)
        {
            DbContextOptions<MarketTrackerDataContext> options = new DbContextOptionsBuilder<MarketTrackerDataContext>()
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

