using System.Collections;
using FluentAssertions;
using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi_test.Application.Repository;

/*
public class ListEntryRepositoryTest
{
    [Fact]
    public async Task GetListEntriesAsync_ShouldReturnShoppingList()
    {
        // Arrange
        var context = CreateDatabase();
        
        var listEntryRepository = new ListEntryRepository(context);
        
        // Act
        var result = await listEntryRepository.GetListEntriesAsync(1);
        
        // Assert
        result.Should().BeEquivalentTo(new List<ListEntryEntity>()
        {
            new()
            {
                ListId = 1,
                StoreId = 1,
                ProductId = "product1",
                Quantity = 1
            },
            new()
            {
                ListId = 1,
                StoreId = 1,
                ProductId = "product2",
                Quantity = 2
            }
        });
    }
    
    [Fact]
    public async Task GetListEntriesByListIdAsync_ShouldReturnListEntry()
    {
        // Arrange
        var context = CreateDatabase();
        
        var listEntryRepository = new ListEntryRepository(context);
        
        // Act
        var result = await listEntryRepository.GetListEntryAsync(1, "product1");
        
        // Assert
        result.Should().BeEquivalentTo(new ListEntry()
        {
            ListId = 1,
            StoreId = 1,
            ProductId = "product1",
            Quantity = 1
        });
    }
    
    [Fact]
    public async Task AddListEntryAsync_ShouldAddListEntry()
    {
        // Arrange
        var context = CreateDatabase();
        
        var listEntryRepository = new ListEntryRepository(context);
        
        // Act
        await listEntryRepository.AddListEntryAsync(1, "product3", 1, 3);
        
        // Assert
        var result = await context.ListEntry.ToListAsync();
        result.Should().BeEquivalentTo(new List<ListEntryEntity>()
        {
            new()
            {
                ListId = 1,
                StoreId = 1,
                ProductId = "product1",
                Quantity = 1
            },
            new()
            {
                ListId = 1,
                StoreId = 1,
                ProductId = "product2",
                Quantity = 2
            },
            new()
            {
                ListId = 1,
                StoreId = 1,
                ProductId = "product3",
                Quantity = 3
            }
        });
    }
    
    [Fact]
    public async Task UpdateListEntryAsync_ShouldUpdateListEntry()
    {
        // Arrange
        var context = CreateDatabase();
        
        var listEntryRepository = new ListEntryRepository(context);
        
        // Act
        await listEntryRepository.UpdateListEntryAsync(1, "product1", 1, 3);
        
        // Assert
        var result = await context.ListEntry.ToListAsync();
        result.Should().BeEquivalentTo(new List<ListEntryEntity>()
        {
            new()
            {
                ListId = 1,
                StoreId = 1,
                ProductId = "product1",
                Quantity = 3
            },
            new()
            {
                ListId = 1,
                StoreId = 1,
                ProductId = "product2",
                Quantity = 2
            }
        });
    }
    
    [Fact]
    public async Task DeleteListEntryAsync_ShouldDeleteListEntry()
    {
        // Arrange
        var context = CreateDatabase();
        
        var listEntryRepository = new ListEntryRepository(context);
        
        // Act
        await listEntryRepository.DeleteListEntryAsync(1, "product1");
        
        // Assert
        var result = await context.ListEntry.ToListAsync();
        result.Should().BeEquivalentTo(new List<ListEntryEntity>()
        {
            new()
            {
                ListId = 1,
                StoreId = 1,
                ProductId = "product2",
                Quantity = 2
            }
        });
    }

    private static MarketTrackerDataContext CreateDatabase()
    {
        var listEntities = new List<ListEntity>
        {
            new ListEntity
            {
                Id = 1,
                ClientId = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11"),
                Name = "List 1",
                ArchivedAt = null,
                CreatedAt = new DateTime(2024, 1, 1, 1, 1, 1, DateTimeKind.Unspecified)
            }
        };
        
        var storeEntities = new List<StoreEntity>
        {
            new StoreEntity
            {
                Id = 1,
                Name = "Store 1",
                Address = "Address 1",
                CompanyId = 1,
                CityId = 1
            }
        };
        
        var companyEntities = new List<CompanyEntity>
        {
            new CompanyEntity
            {
                Id = 1,
                Name = "Company 1"
            }
        };
        
        var cityEntities = new List<CityEntity>
        {
            new CityEntity
            {
                Id = 1,
                Name = "City 1"
            }
        };
        
        var listEntryEntities = new List<ListEntryEntity>
        {
            new ListEntryEntity
            {
                ListId = 1,
                StoreId = 1,
                ProductId = "product1",
                Quantity = 1
            },
            new ListEntryEntity
            {
                ListId = 1,
                StoreId = 1,
                ProductId = "product2",
                Quantity = 2
            }
        };
        
        DbContextOptions<MarketTrackerDataContext> options =
            new DbContextOptionsBuilder<MarketTrackerDataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

        var databaseContext = new MarketTrackerDataContext(options);
        databaseContext.List.AddRange(listEntities);
        databaseContext.ListEntry.AddRange(listEntryEntities);
        databaseContext.Store.AddRange(storeEntities);
        databaseContext.Company.AddRange(companyEntities);
        databaseContext.City.AddRange(cityEntities);
        databaseContext.SaveChanges();
        databaseContext.Database.EnsureCreated();
        return databaseContext;
    }
}
*/