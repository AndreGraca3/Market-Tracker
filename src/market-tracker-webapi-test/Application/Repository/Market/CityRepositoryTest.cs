namespace market_tracker_webapi_test.Application.Repository;

/*public class CityRepositoryTest
{
    [Fact]
    public async Task GetCitiesAsync_WithExistingCities_ReturnsCityData()
    {
        // Arrange
        var cityEntities = new List<CityEntity>
        {
            new() { Id = 1, Name = "City 1" },
            new() { Id = 2, Name = "City 2" }
        };

        var context = CreateDatabase(cityEntities);
        var cityRepository = new CityRepository(context);

        var expectedCityData = new List<City>
        {
            new() { Id = 1, Name = "City 1" },
            new() { Id = 2, Name = "City 2" }
        };

        // Act
        var cityData = await cityRepository.GetCitiesAsync();

        // Assert
        expectedCityData.Should().BeEquivalentTo(cityData);
    }

    [Fact]
    public async Task GetCityByIdAsync_WithExistingCity_ReturnsCityData()
    {
        // Arrange
        var cityEntities = new List<CityEntity>
        {
            new() { Id = 1, Name = "City 1" }
        };

        var context = CreateDatabase(cityEntities);
        var cityRepository = new CityRepository(context);

        var expectedCityData = new City { Id = 1, Name = "City 1" };

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
            new() { Id = 1, Name = "City 1" }
        };

        var context = CreateDatabase(cityEntities);
        var cityRepository = new CityRepository(context);

        // Act
        var cityData = await cityRepository.GetCityByIdAsync(2);

        // Assert
        cityData.Should().BeNull();
    }

    [Fact]
    public async Task GetCityByNameAsync_WithExistingCity_ReturnsCityData()
    {
        // Arrange
        var cityEntities = new List<CityEntity>
        {
            new() { Id = 1, Name = "City 1" }
        };

        var context = CreateDatabase(cityEntities);
        var cityRepository = new CityRepository(context);

        var expectedCityData = new City { Id = 1, Name = "City 1" };

        // Act
        var cityData = await cityRepository.GetCityByNameAsync("City 1");

        // Assert
        expectedCityData.Should().BeEquivalentTo(cityData);
    }

    [Fact]
    public async Task GetCityByNameAsync_WithNonExistingCity_ReturnsNull()
    {
        // Arrange
        var cityEntities = new List<CityEntity>
        {
            new() { Id = 1, Name = "City 1" }
        };

        var context = CreateDatabase(cityEntities);
        var cityRepository = new CityRepository(context);

        // Act
        var cityData = await cityRepository.GetCityByNameAsync("City 2");

        // Assert
        cityData.Should().BeNull();
    }

    [Fact]
    public async Task AddCityAsync_WithCityData_ReturnsCityId()
    {
        // Arrange
        var cityEntities = new List<CityEntity>
        {
            new() { Id = 1, Name = "City 1" }
        };

        var context = CreateDatabase(cityEntities);
        var cityRepository = new CityRepository(context);

        var cityData = new City { Id = 2, Name = "City 2" };

        // Act
        var cityId = await cityRepository.AddCityAsync(cityData.Name);

        // Assert
        cityData.Id.Should().Be(cityId);
    }

    [Fact]
    public async Task UpdateCityAsync_WithExistingCity_ReturnsUpdatedCityData()
    {
        // Arrange
        var cityEntities = new List<CityEntity>
        {
            new() { Id = 1, Name = "City 1" }
        };

        var context = CreateDatabase(cityEntities);
        var cityRepository = new CityRepository(context);

        var updatedCityData = new City { Id = 1, Name = "City 2" };

        // Act
        var cityData = await cityRepository.UpdateCityAsync(
            updatedCityData.Id,
            updatedCityData.Name
        );

        // Assert
        updatedCityData.Should().BeEquivalentTo(cityData);
    }

    [Fact]
    public async Task UpdateCityAsync_WithNonExistingCity_ReturnsNull()
    {
        // Arrange
        var cityEntities = new List<CityEntity>
        {
            new() { Id = 1, Name = "City 1" }
        };

        var context = CreateDatabase(cityEntities);
        var cityRepository = new CityRepository(context);

        var updatedCityData = new City { Id = 2, Name = "City 2" };

        // Act
        var cityData = await cityRepository.UpdateCityAsync(
            updatedCityData.Id,
            updatedCityData.Name
        );

        // Assert
        cityData.Should().BeNull();
    }

    [Fact]
    public async Task DeleteCityAsync_WithExistingCity_ReturnsDeletedCityData()
    {
        // Arrange
        var cityEntities = new List<CityEntity>
        {
            new() { Id = 1, Name = "City 1" }
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
            new() { Id = 1, Name = "City 1" }
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
        DbContextOptions<MarketTrackerDataContext> options =
            new DbContextOptionsBuilder<MarketTrackerDataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

        var databaseContext = new MarketTrackerDataContext(options);
        databaseContext.City.AddRange(cityEntities);
        databaseContext.SaveChanges();
        databaseContext.Database.EnsureCreated();
        return databaseContext;
    }
}*/
