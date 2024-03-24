using FluentAssertions;
using market_tracker_webapi.Application.Models.City;
using market_tracker_webapi.Application.Repositories.City;
using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi_test.Application.Repositories;

public class CityRepositoryTest
{
    [Fact]
    public async Task GetCityByIdAsync_WithExistingCity_ReturnsCityData()
    {
        // Arrange
        var cityEntities = new List<CityEntity>
        {
            new ()
            {
                Id = 1, 
                Name = "City 1"
            }
        };

        var context = CreateDatabase(cityEntities);
        var cityRepository = new CityRepository(context);
        
        var expectedCityData = new CityData
        {
            Id = 1,
            Name = "City 1"
        };
        
        // Act
        var cityData = await cityRepository.GetCityByIdAsync(1);
        
        // Assert
        expectedCityData.Should().BeEquivalentTo(cityData);
    }
    
    [Fact]
    public async Task GetCityByIdAsync_WithNonExistingCity_ReturnsNull()
    {
        // Arrange
        var cityEntities = new List<CityEntity>
        {
            new ()
            {
                Id = 1, 
                Name = "City 1"
            }
        };

        var context = CreateDatabase(cityEntities);
        var cityRepository = new CityRepository(context);
        
        // Act
        var cityData = await cityRepository.GetCityByIdAsync(2);
        
        // Assert
        cityData.Should().BeNull();
    }
    
    [Fact]
    public async Task AddCityAsync_WithCityData_ReturnsCityId()
    {
        // Arrange
        var cityEntities = new List<CityEntity>
        {
            new ()
            {
                Id = 1, 
                Name = "City 1"
            }
        };

        var context = CreateDatabase(cityEntities);
        var cityRepository = new CityRepository(context);
        
        var cityData = new CityAddInputData
        {
            Name = "City 2"
        };
        
        // Act
        var cityId = await cityRepository.AddCityAsync(cityData);
        
        // Assert
        cityId.Should().Be(2);
    }
    
    [Fact]
    public async Task UpdateCityAsync_WithExistingCity_ReturnsUpdatedCityData()
    {
        // Arrange
        var cityEntities = new List<CityEntity>
        {
            new ()
            {
                Id = 1, 
                Name = "City 1"
            }
        };

        var context = CreateDatabase(cityEntities);
        var cityRepository = new CityRepository(context);
        
        var updatedCityData = new CityUpdateInputData()
        {
            Id = 1,
            Name = "City 2"
        };
        
        // Act
        var cityData = await cityRepository.UpdateCityAsync(updatedCityData);
        
        // Assert
        updatedCityData.Should().BeEquivalentTo(cityData);
    }
    
    [Fact]
    public async Task UpdateCityAsync_WithNonExistingCity_ReturnsNull()
    {
        // Arrange
        var cityEntities = new List<CityEntity>
        {
            new ()
            {
                Id = 1, 
                Name = "City 1"
            }
        };

        var context = CreateDatabase(cityEntities);
        var cityRepository = new CityRepository(context);
        
        var updatedCityData = new CityUpdateInputData()
        {
            Id = 2,
            Name = "City 2"
        };
        
        // Act
        var cityData = await cityRepository.UpdateCityAsync(updatedCityData);
        
        // Assert
        cityData.Should().BeNull();
    }
    
    [Fact]
    public async Task DeleteCityAsync_WithExistingCity_ReturnsDeletedCityData()
    {
        // Arrange
        var cityEntities = new List<CityEntity>
        {
            new ()
            {
                Id = 1, 
                Name = "City 1"
            }
        };

        var context = CreateDatabase(cityEntities);
        var cityRepository = new CityRepository(context);
        
        // Act
        var cityData = await cityRepository.DeleteCityAsync(1);
        
        // Assert
        cityData.Should().NotBeNull();
    }
    
    [Fact]
    public async Task DeleteCityAsync_WithNonExistingCity_ReturnsNull()
    {
        // Arrange
        var cityEntities = new List<CityEntity>
        {
            new ()
            {
                Id = 1, 
                Name = "City 1"
            }
        };

        var context = CreateDatabase(cityEntities);
        var cityRepository = new CityRepository(context);
        
        // Act
        var cityData = await cityRepository.DeleteCityAsync(2);
        
        // Assert
        cityData.Should().BeNull();
    }
    
    private static MarketTrackerDataContext CreateDatabase(IEnumerable<CityEntity> cityEntities)
    {
        DbContextOptions<MarketTrackerDataContext> options = new DbContextOptionsBuilder<MarketTrackerDataContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var databaseContext = new MarketTrackerDataContext(options);
        databaseContext.City.AddRange(cityEntities);
        databaseContext.SaveChanges();
        databaseContext.Database.EnsureCreated();
        return databaseContext;
    }
}