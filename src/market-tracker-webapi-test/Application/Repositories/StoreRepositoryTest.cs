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
            storeData.Should().BeEquivalentTo(expectedStore, options => options.Excluding(x => x.Id));
        }
        
        [Fact]
        public async Task GetStoreByIdAsync_WhenStoreDoesNotExist_ThrowsEntityNotFoundException()
        {
            // Arrange
            var context = CreateDatabase(new List<StoreEntity>(), new List<CompanyEntity>());
            var storeRepository = new StoreRepository(context);

            // Act
            Func<Task> action = async () => await storeRepository.GetStoreByIdAsync(1);
            
            // Assert
            await action.Should().ThrowAsync<EntityNotFoundException>()
                .WithMessage($"Store with Id 1 not found.");
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
                }
            };

            var context = CreateDatabase(storeMockEntities, companyMockEntities);
            var storeRepository = new StoreRepository(context);

            var storeData = new StoreData()
            {
                Id = 1,
                Address = "AddressA",
                City = "Porto",
                OpenTime = new DateTime(2021, 1, 1, 8, 0, 0, DateTimeKind.Unspecified),
                CloseTime = new DateTime(2021, 1, 1, 20, 0, 0, DateTimeKind.Unspecified),
                CompanyId = 1
            };

            // Act
            var actualStore = await storeRepository.UpdateStoreAsync(storeData);
            
            // Assert
            actualStore.Should().BeEquivalentTo(storeData);
            (await context.Store.FindAsync(storeData.Id)).Should().BeEquivalentTo(storeData);
        }

        [Fact]
        public async Task UpdateStoreAsync_WhenStoreDoesNotExist_ThrowsEntityNotFoundException()
        {
            // Arrange
            var context = CreateDatabase(new List<StoreEntity>(), new List<CompanyEntity>());
            var storeRepository = new StoreRepository(context);

            var storeData = new StoreData()
            {
                Id = 1,
                Address = "AddressA",
                City = "Porto",
                OpenTime = new DateTime(2021, 1, 1, 8, 0, 0, DateTimeKind.Unspecified),
                CloseTime = new DateTime(2021, 1, 1, 20, 0, 0, DateTimeKind.Unspecified),
                CompanyId = 1
            };

            // Act
            Func<Task> action = async () => await storeRepository.UpdateStoreAsync(storeData);
            
            // Assert
            await action.Should().ThrowAsync<EntityNotFoundException>()
                .WithMessage($"Store with Id {storeData.Id} not found.");
        }
        
        [Fact]
        public async Task UpdateStoreAsync_WhenCompanyDoesNotExist_ThrowsEntityNotFoundException()
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
                }
            };
            
            var context = CreateDatabase(storeMockEntities, companyMockEntities);
            var storeRepository = new StoreRepository(context);

            var storeData = new StoreData()
            {
                Id = 1,
                Address = "AddressA",
                City = "Porto",
                OpenTime = new DateTime(2021, 1, 1, 8, 0, 0, DateTimeKind.Unspecified),
                CloseTime = new DateTime(2021, 1, 1, 20, 0, 0, DateTimeKind.Unspecified),
                CompanyId = 2
            };

            // Act
            Func<Task> action = async () => await storeRepository.UpdateStoreAsync(storeData);
            
            // Assert
            await action.Should().ThrowAsync<EntityNotFoundException>()
                .WithMessage($"Company with Id {storeData.CompanyId} not found.");
        }
        
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
                }
            };

            var context = CreateDatabase(storeMockEntities, companyMockEntities);
            var storeRepository = new StoreRepository(context);

            // Act
            var actualStore = await storeRepository.DeleteStoreAsync(1);
            
            // Assert
            actualStore.Should().BeEquivalentTo(storeMockEntities[0]);
            (await context.Store.FindAsync(1)).Should().BeNull();
        }
        
        [Fact]
        public async Task DeleteStoreAsync_WhenStoreDoesNotExist_ThrowsEntityNotFoundException()
        {
            // Arrange
            var context = CreateDatabase(new List<StoreEntity>(), new List<CompanyEntity>());
            var storeRepository = new StoreRepository(context);

            // Act
            Func<Task> action = async () => await storeRepository.DeleteStoreAsync(1);
            
            // Assert
            await action.Should().ThrowAsync<EntityNotFoundException>()
                .WithMessage($"Store with Id 1 not found.");
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

