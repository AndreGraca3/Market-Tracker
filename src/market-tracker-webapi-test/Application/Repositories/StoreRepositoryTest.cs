using FluentAssertions;
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
            
            var storeMockEntities = new List<StoreEntity>
            {
                new ()
                {
                    Id = 1, 
                    Address = "Address1",
                    City = "Lisboa",
                    OpenTime = new DateTime(2021, 1, 1, 8, 0, 0, DateTimeKind.Unspecified),
                    CloseTime = new DateTime(2021, 1, 1, 20, 0, 0, DateTimeKind.Unspecified),
                    CompanyId = 1
                },
                new ()
                {
                    Id = 2, 
                    Address = "Address2",
                    City = "Amadora",
                    OpenTime = new DateTime(2021, 1, 1, 8, 0, 0, DateTimeKind.Unspecified),
                    CloseTime = new DateTime(2021, 1, 1, 20, 0, 0, DateTimeKind.Unspecified),
                    CompanyId = 1
                },
                new ()
                {
                    Id = 3, 
                    Address = "Address3",
                    City = "Oeiras",
                    OpenTime = new DateTime(2021, 1, 1, 8, 0, 0, DateTimeKind.Unspecified),
                    CloseTime = new DateTime(2021, 1, 1, 20, 0, 0, DateTimeKind.Unspecified),
                    CompanyId = 1
                }
            };

            var expectedStore = new StoreData()
            {
                Address = "Address1",
                City = "Lisboa",
                OpenTime = new DateTime(2021, 1, 1, 8, 0, 0, DateTimeKind.Unspecified),
                CloseTime = new DateTime(2021, 1, 1, 20, 0, 0, DateTimeKind.Unspecified),
                CompanyId = 1
            };
            
            var context = CreateDatabase(storeMockEntities, companyMockEntities);
            var storeRepository = new StoreRepository(context);

            // Act
            var storeData = await storeRepository.GetStoreByIdAsync(1);

            // Assert
            storeData.Should().BeEquivalentTo(expectedStore);
        }
        
        [Fact]
        public async Task GetStoreByIdAsync_WhenStoreDoesNotExist_ReturnsNull()
        {
            // Arrange
            var context = CreateDatabase(new List<StoreEntity>(), new List<CompanyEntity>());
            var storeRepository = new StoreRepository(context);

            // Act
            var storeData = await storeRepository.GetStoreByIdAsync(99);

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
            
            var storeMockEntities = new List<StoreEntity>
            {
                new ()
                {
                    Id = 1, 
                    Address = "Address1",
                    City = "Lisboa",
                    OpenTime = new DateTime(2021, 1, 1, 8, 0, 0, DateTimeKind.Unspecified),
                    CloseTime = new DateTime(2021, 1, 1, 20, 0, 0, DateTimeKind.Unspecified),
                    CompanyId = 1
                },
                new ()
                {
                    Id = 2, 
                    Address = "Address2",
                    City = "Amadora",
                    OpenTime = new DateTime(2021, 1, 1, 8, 0, 0, DateTimeKind.Unspecified),
                    CloseTime = new DateTime(2021, 1, 1, 20, 0, 0, DateTimeKind.Unspecified),
                    CompanyId = 1
                },
                new ()
                {
                    Id = 3, 
                    Address = "Address3",
                    City = "Oeiras",
                    OpenTime = new DateTime(2021, 1, 1, 8, 0, 0, DateTimeKind.Unspecified),
                    CloseTime = new DateTime(2021, 1, 1, 20, 0, 0, DateTimeKind.Unspecified),
                    CompanyId = 1
                }
            };

            var context = CreateDatabase(storeMockEntities, companyMockEntities);
            var storeRepository = new StoreRepository(context);

            var storeData = new StoreData()
            {
                Address = "Address4",
                City = "Lisboa",
                OpenTime = new DateTime(2021, 1, 1, 8, 0, 0, DateTimeKind.Unspecified),
                CloseTime = new DateTime(2021, 1, 1, 20, 0, 0, DateTimeKind.Unspecified),
                CompanyId = 1
            };
            
            // Act
            var storeId = await storeRepository.AddStoreAsync(storeData);
            
            var storeEntity = new StoreEntity()
            {
                Address = storeData.Address,
                City = storeData.City,
                OpenTime = storeData.OpenTime,
                CloseTime = storeData.CloseTime,
                CompanyId = storeData.CompanyId
            };
            
            storeMockEntities.Add(storeEntity);
            
            // Assert
            context.Store.Should().BeEquivalentTo(storeMockEntities, options => options.Excluding(x => x.Id));
            storeId.Should().Be(4);
        }

        [Fact]
        public async Task AddStoreAsync_WhenCompanyDoesNotExist_ThrowsEntityNotFoundException()
        {
            // Arrange
            var context = CreateDatabase(new List<StoreEntity>(), new List<CompanyEntity>());
            var storeRepository = new StoreRepository(context);

            var storeData = new StoreData()
            {
                Address = "Address4",
                City = "Lisboa",
                OpenTime = new DateTime(2021, 1, 1, 8, 0, 0, DateTimeKind.Unspecified),
                CloseTime = new DateTime(2021, 1, 1, 20, 0, 0, DateTimeKind.Unspecified),
                CompanyId = 1
            };
            
            // Act
            Func<Task> action = async () => await storeRepository.AddStoreAsync(storeData);
            
            // Assert
            await action.Should().ThrowAsync<EntityNotFoundException>()
                .WithMessage($"Company with Id {storeData.CompanyId} not found.");
        }

        [Fact]
        public async Task AddStoreAsync_WhenAddressAlreadyExists_ReturnsEntityCreationException()
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

            var storeMockEntities = new List<StoreEntity>
            {
                new()
                {
                    Id = 1,
                    Address = "Address1",
                    City = "Lisboa",
                    OpenTime = new DateTime(2021, 1, 1, 8, 0, 0, DateTimeKind.Unspecified),
                    CloseTime = new DateTime(2021, 1, 1, 20, 0, 0, DateTimeKind.Unspecified),
                    CompanyId = 1
                },
                new()
                {
                    Id = 2,
                    Address = "Address2",
                    City = "Amadora",
                    OpenTime = new DateTime(2021, 1, 1, 8, 0, 0, DateTimeKind.Unspecified),
                    CloseTime = new DateTime(2021, 1, 1, 20, 0, 0, DateTimeKind.Unspecified),
                    CompanyId = 1
                },
                new()
                {
                    Id = 3,
                    Address = "Address3",
                    City = "Oeiras",
                    OpenTime = new DateTime(2021, 1, 1, 8, 0, 0, DateTimeKind.Unspecified),
                    CloseTime = new DateTime(2021, 1, 1, 20, 0, 0, DateTimeKind.Unspecified),
                    CompanyId = 1
                }
            };

            var context = CreateDatabase(storeMockEntities, companyMockEntities);
            var storeRepository = new StoreRepository(context);

            var storeData = new StoreData()
            {
                Address = "Address1",
                City = "Lisboa",
                OpenTime = new DateTime(2021, 1, 1, 8, 0, 0, DateTimeKind.Unspecified),
                CloseTime = new DateTime(2021, 1, 1, 20, 0, 0, DateTimeKind.Unspecified),
                CompanyId = 1
            };

            // Act
            Func<Task> action = async () => await storeRepository.AddStoreAsync(storeData);

            // Assert
            await action.Should().ThrowAsync<EntityCreationException>()
                .WithMessage("Duplicate values were found in the table MarketTracker.store");
        }


        private static MarketTrackerDataContext CreateDatabase(IEnumerable<StoreEntity> storeEntities, IEnumerable<CompanyEntity> companyEntities)
        {
            DbContextOptions<MarketTrackerDataContext> options = new DbContextOptionsBuilder<MarketTrackerDataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var databaseContext = new MarketTrackerDataContext(options);
            databaseContext.Company.AddRange(companyEntities);
            databaseContext.Store.AddRange(storeEntities);
            databaseContext.SaveChanges();
            databaseContext.Database.EnsureCreated();
            return databaseContext;
        }
    }
}

