using FluentAssertions;
using market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Shop;
using market_tracker_webapi.Application.Repository.Market.City;
using market_tracker_webapi.Infrastructure.PostgreSQLTables.Market;

namespace market_tracker_webapi_test.Application.Repository.Market;

public class CityRepositoryTest
{
    [Fact]
    public async Task GetCityByIdAsync_WhenCityExists_ReturnsCity()
    {
        // Arrange
        var cityEntities = new List<CityEntity>
        {
            new CityEntity { Id = 1, Name = "cityName" },
            new CityEntity { Id = 2, Name = "cityName2" }
        };
        
        var context = DbHelper.CreateDatabase(cityEntities);
        
        var cityRepository = new CityRepository(context);
        
        // Act
        var actualCity = await cityRepository.GetCityByIdAsync(1);
        
        // Assert
        var expectedCity = new City(1, "cityName");
        actualCity.Should().BeEquivalentTo(expectedCity);
    }
    
    [Fact]
    public async Task GetCityByIdAsync_WhenCityDoesNotExist_ReturnsNull()
    {
        // Arrange
        var cityEntities = new List<CityEntity>
        {
            new CityEntity { Id = 1, Name = "cityName" },
            new CityEntity { Id = 2, Name = "cityName2" }
        };
        
        var context = DbHelper.CreateDatabase(cityEntities);
        
        var cityRepository = new CityRepository(context);
        
        // Act
        var actualCity = await cityRepository.GetCityByIdAsync(3);
        
        // Assert
        actualCity.Should().BeNull();
    }
    
    [Fact]
    public async Task GetCityByNameAsync_WhenCityExists_ReturnsCity()
    {
        // Arrange
        var cityEntities = new List<CityEntity>
        {
            new CityEntity { Id = 1, Name = "cityName" },
            new CityEntity { Id = 2, Name = "cityName2" }
        };
        
        var context = DbHelper.CreateDatabase(cityEntities);
        
        var cityRepository = new CityRepository(context);
        
        // Act
        var actualCity = await cityRepository.GetCityByNameAsync("cityName");
        
        // Assert
        var expectedCity = new City(1, "cityName");
        actualCity.Should().BeEquivalentTo(expectedCity);
    }
    
    [Fact]
    public async Task GetCityByNameAsync_WhenCityDoesNotExist_ReturnsNull()
    {
        // Arrange
        var cityEntities = new List<CityEntity>
        {
            new CityEntity { Id = 1, Name = "cityName" },
            new CityEntity { Id = 2, Name = "cityName2" }
        };
        
        var context = DbHelper.CreateDatabase(cityEntities);
        
        var cityRepository = new CityRepository(context);
        
        // Act
        var actualCity = await cityRepository.GetCityByNameAsync("cityName3");
        
        // Assert
        actualCity.Should().BeNull();
    }
    
    [Fact]
    public async Task GetCitiesAsync_WhenCitiesExist_ReturnsCities()
    {
        // Arrange
        var cityEntities = new List<CityEntity>
        {
            new CityEntity { Id = 1, Name = "cityName" },
            new CityEntity { Id = 2, Name = "cityName2" }
        };
        
        var context = DbHelper.CreateDatabase(cityEntities);
        
        var cityRepository = new CityRepository(context);
        
        // Act
        var actualCities = await cityRepository.GetCitiesAsync();
        
        // Assert
        var expectedCities = new List<City>
        {
            new City(1, "cityName"),
            new City(2, "cityName2")
        };
        actualCities.Should().BeEquivalentTo(expectedCities);
    }
    
    [Fact]
    public async Task GetCitiesAsync_WhenNoCitiesExist_ReturnsEmptyList()
    {
        // Arrange
        var cityEntities = new List<CityEntity>();
        
        var context = DbHelper.CreateDatabase(cityEntities);
        
        var cityRepository = new CityRepository(context);
        
        // Act
        var actualCities = await cityRepository.GetCitiesAsync();
        
        // Assert
        actualCities.Should().BeEmpty();
    }
    
    [Fact]
    public async Task AddCityAsync_WhenCityDoesNotExist_AddsCity()
    {
        // Arrange
        var cityEntities = new List<CityEntity>
        {
            new CityEntity { Id = 1, Name = "cityName" },
            new CityEntity { Id = 2, Name = "cityName2" }
        };
        
        var context = DbHelper.CreateDatabase(cityEntities);
        
        var cityRepository = new CityRepository(context);
        
        var city = new City(3, "cityName3");
        
        // Act
        await cityRepository.AddCityAsync("cityName3");
        
        // Assert
        var actualCity = await cityRepository.GetCityByIdAsync(3);
        actualCity.Should().BeEquivalentTo(city);
    }
    
    [Fact]
    public async Task AddCityAsync_WhenCityExists_DoesNotAddCity()
    {
        // Arrange
        var cityEntities = new List<CityEntity>
        {
            new CityEntity { Id = 1, Name = "cityName" },
            new CityEntity { Id = 2, Name = "cityName2" }
        };
        
        var context = DbHelper.CreateDatabase(cityEntities);
        
        var cityRepository = new CityRepository(context);
        
        var city = new City(1, "cityName");
        
        // Act
        await cityRepository.AddCityAsync("cityName");
        
        // Assert
        var actualCity = await cityRepository.GetCityByIdAsync(1);
        actualCity.Should().BeEquivalentTo(city);
    }
    
    [Fact]
    public async Task UpdateCityAsync_WhenCityExists_UpdatesCity()
    {
        // Arrange
        var cityEntities = new List<CityEntity>
        {
            new CityEntity { Id = 1, Name = "cityName" },
            new CityEntity { Id = 2, Name = "cityName2" }
        };
        
        var context = DbHelper.CreateDatabase(cityEntities);
        
        var cityRepository = new CityRepository(context);
        
        var city = new City(1, "cityNameUpdated");
        
        // Act
        await cityRepository.UpdateCityAsync(1, "cityNameUpdated");
        
        // Assert
        var actualCity = await cityRepository.GetCityByIdAsync(1);
        actualCity.Should().BeEquivalentTo(city);
    }
    
    [Fact]
    public async Task UpdateCityAsync_WhenCityDoesNotExist_DoesNotUpdateCity()
    {
        // Arrange
        var cityEntities = new List<CityEntity>
        {
            new CityEntity { Id = 1, Name = "cityName" },
            new CityEntity { Id = 2, Name = "cityName2" }
        };
        
        var context = DbHelper.CreateDatabase(cityEntities);
        
        var cityRepository = new CityRepository(context);
        
        var city = new City(3, "cityName3");
        
        // Act
        await cityRepository.UpdateCityAsync(3, "cityName3");
        
        // Assert
        var actualCity = await cityRepository.GetCityByIdAsync(3);
        actualCity.Should().BeNull();
    }
    
    [Fact]
    public async Task DeleteCityAsync_WhenCityExists_DeletesCity()
    {
        // Arrange
        var cityEntities = new List<CityEntity>
        {
            new CityEntity { Id = 1, Name = "cityName" },
            new CityEntity { Id = 2, Name = "cityName2" }
        };
        
        var context = DbHelper.CreateDatabase(cityEntities);
        
        var cityRepository = new CityRepository(context);
        
        // Act
        await cityRepository.DeleteCityAsync(1);
        
        // Assert
        var actualCity = await cityRepository.GetCityByIdAsync(1);
        actualCity.Should().BeNull();
    }
    
    [Fact]
    public async Task DeleteCityAsync_WhenCityDoesNotExist_DoesNotDeleteCity()
    {
        // Arrange
        var cityEntities = new List<CityEntity>
        {
            new CityEntity { Id = 1, Name = "cityName" },
            new CityEntity { Id = 2, Name = "cityName2" }
        };
        
        var context = DbHelper.CreateDatabase(cityEntities);
        
        var cityRepository = new CityRepository(context);
        
        // Act
        await cityRepository.DeleteCityAsync(3);
        
        // Assert
        var actualCity = await cityRepository.GetCityByIdAsync(3);
        actualCity.Should().BeNull();
    }
}
