using System.Collections;
using FluentAssertions;
using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Repository.Operations.List;
using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi_test.Application.Repository;

public class ListRepositoryTest
{
    [Fact]
    public async Task GetListsAsync_ShouldReturnListOfProducts()
    {
        // Arrange
        var listEntities = new List<ListEntity>
        {
            new ListEntity
            {
                Id = 1,
                ClientId = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11"),
                Name = "List 1",
                ArchivedAt = null,
                CreatedAt = new DateTime(2024, 1, 1, 1, 1, 1, DateTimeKind.Unspecified)
            },
            new ListEntity
            {
                Id = 2,
                ClientId = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11"),
                Name = "List 2",
                ArchivedAt = null,
                CreatedAt = new DateTime(2024, 1, 2, 1, 1, 1, DateTimeKind.Unspecified)
            },
            new ListEntity
            {
                Id = 3,
                ClientId = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11"),
                Name = "List 3",
                ArchivedAt = null,
                CreatedAt = new DateTime(2024, 1, 3, 1, 1, 1, DateTimeKind.Unspecified)
            }
        };

        var databaseContext = CreateDatabase(listEntities);
        var listRepository = new ListRepository(databaseContext);

        // Act
        var result = await listRepository.GetListsAsync(
            Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11"),
            null,
            null,
            new DateTime(2024, 1, 1, 1, 1, 1, DateTimeKind.Unspecified));

        // Assert
        result.Should().BeEquivalentTo(new List<ListOfProducts>()
        {
            new()
            {
                Id = 1,
                ClientId = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11"),
                ListName = "List 1",
                ArchivedAt = null,
                CreatedAt = new DateTime(2024, 1, 1, 1, 1, 1, DateTimeKind.Unspecified)
            }
        });
    }
    
    [Fact]
    public async Task GetListByIdAsync_ShouldReturnListOfProducts()
    {
        // Arrange
        var listEntities = new List<ListEntity>
        {
            new ListEntity
            {
                Id = 1,
                ClientId = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11"),
                Name = "List 1",
                ArchivedAt = null,
                CreatedAt = new DateTime(2024, 1, 1, 1, 1, 1, DateTimeKind.Unspecified)
            },
            new ListEntity
            {
                Id = 2,
                ClientId = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11"),
                Name = "List 2",
                ArchivedAt = null,
                CreatedAt = new DateTime(2024, 1, 2, 1, 1, 1, DateTimeKind.Unspecified)
            },
            new ListEntity
            {
                Id = 3,
                ClientId = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11"),
                Name = "List 3",
                ArchivedAt = null,
                CreatedAt = new DateTime(2024, 1, 3, 1, 1, 1, DateTimeKind.Unspecified)
            }
        };

        var databaseContext = CreateDatabase(listEntities);
        var listRepository = new ListRepository(databaseContext);

        // Act
        var result = await listRepository.GetListByIdAsync(1);

        // Assert
        result.Should().BeEquivalentTo(new ListOfProducts()
        {
            Id = 1,
            ClientId = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11"),
            ListName = "List 1",
            ArchivedAt = null,
            CreatedAt = new DateTime(2024, 1, 1, 1, 1, 1, DateTimeKind.Unspecified)
        });
    }
    
    [Fact]
    public async Task AddListAsync_ShouldReturnId()
    {
        // Arrange
        var listEntities = new List<ListEntity>
        {
            new ListEntity
            {
                Id = 1,
                ClientId = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11"),
                Name = "List 1",
                ArchivedAt = null,
                CreatedAt = new DateTime(2024, 1, 1, 1, 1, 1, DateTimeKind.Unspecified)
            },
            new ListEntity
            {
                Id = 2,
                ClientId = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11"),
                Name = "List 2",
                ArchivedAt = null,
                CreatedAt = new DateTime(2024, 1, 2, 1, 1, 1, DateTimeKind.Unspecified)
            },
            new ListEntity
            {
                Id = 3,
                ClientId = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11"),
                Name = "List 3",
                ArchivedAt = null,
                CreatedAt = new DateTime(2024, 1, 3, 1, 1, 1, DateTimeKind.Unspecified)
            }
        };

        var databaseContext = CreateDatabase(listEntities);
        var listRepository = new ListRepository(databaseContext);

        // Act
        var result = await listRepository.AddListAsync(
            Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11"),
            "List 4");

        // Assert
        result.Should().Be(4);
        (await databaseContext.List.FindAsync(result)).Should().BeEquivalentTo(new ListEntity()
        {
            Id = 4,
            ClientId = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11"),
            Name = "List 4",
            ArchivedAt = null
        }, options => options.Excluding(listEntity => listEntity.CreatedAt));
    }
    
    [Fact]
    public async Task UpdateListAsync_ShouldReturnListOfProducts()
    {
        // Arrange
        var listEntities = new List<ListEntity>
        {
            new ListEntity
            {
                Id = 1,
                ClientId = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11"),
                Name = "List 1",
                ArchivedAt = null,
                CreatedAt = new DateTime(2024, 1, 1, 1, 1, 1, DateTimeKind.Unspecified)
            },
            new ListEntity
            {
                Id = 2,
                ClientId = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11"),
                Name = "List 2",
                ArchivedAt = null,
                CreatedAt = new DateTime(2024, 1, 2, 1, 1, 1, DateTimeKind.Unspecified)
            },
            new ListEntity
            {
                Id = 3,
                ClientId = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11"),
                Name = "List 3",
                ArchivedAt = null,
                CreatedAt = new DateTime(2024, 1, 3, 1, 1, 1, DateTimeKind.Unspecified)
            }
        };

        var databaseContext = CreateDatabase(listEntities);
        var listRepository = new ListRepository(databaseContext);

        // Act
        var result = await listRepository.UpdateListAsync(1, "List 4", new DateTime(2024, 1, 4, 1, 1, 1, DateTimeKind.Unspecified));

        // Assert
        result.Should().BeEquivalentTo(new ListOfProducts()
        {
            Id = 1,
            ClientId = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11"),
            ListName = "List 4",
            CreatedAt = new DateTime(2024, 1, 1, 1, 1, 1, DateTimeKind.Unspecified),
            ArchivedAt = new DateTime(2024, 1, 4, 1, 1, 1, DateTimeKind.Unspecified)
        });
    }
    
    [Fact]
    public async Task DeleteListAsync_ShouldReturnListOfProducts()
    {
        // Arrange
        var listEntities = new List<ListEntity>
        {
            new ListEntity
            {
                Id = 1,
                ClientId = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11"),
                Name = "List 1",
                ArchivedAt = null,
                CreatedAt = new DateTime(2024, 1, 1, 1, 1, 1, DateTimeKind.Unspecified)
            },
            new ListEntity
            {
                Id = 2,
                ClientId = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11"),
                Name = "List 2",
                ArchivedAt = null,
                CreatedAt = new DateTime(2024, 1, 2, 1, 1, 1, DateTimeKind.Unspecified)
            },
            new ListEntity
            {
                Id = 3,
                ClientId = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11"),
                Name = "List 3",
                ArchivedAt = null,
                CreatedAt = new DateTime(2024, 1, 3, 1, 1, 1, DateTimeKind.Unspecified)
            }
        };

        var databaseContext = CreateDatabase(listEntities);
        var listRepository = new ListRepository(databaseContext);

        // Act
        var result = await listRepository.DeleteListAsync(1);

        // Assert
        result.Should().BeEquivalentTo(new ListOfProducts()
        {
            Id = 1,
            ClientId = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11"),
            ListName = "List 1",
            ArchivedAt = null,
            CreatedAt = new DateTime(2024, 1, 1, 1, 1, 1, DateTimeKind.Unspecified)
        });
    }
    
    private static MarketTrackerDataContext CreateDatabase(IEnumerable<ListEntity> listEntities)
    {
        DbContextOptions<MarketTrackerDataContext> options =
            new DbContextOptionsBuilder<MarketTrackerDataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

        var databaseContext = new MarketTrackerDataContext(options);
        databaseContext.List.AddRange(listEntities);
        databaseContext.SaveChanges();
        databaseContext.Database.EnsureCreated();
        return databaseContext;
    }
}