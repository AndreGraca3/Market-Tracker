using FluentAssertions;
using market_tracker_webapi.Application.Domain.Models.Market.Retail.Shop;
using market_tracker_webapi.Application.Http.Models.Identifiers;
using market_tracker_webapi.Application.Repository.Market.City;
using market_tracker_webapi.Application.Service.Errors.City;
using market_tracker_webapi.Application.Service.Operations.Market.City;
using Moq;

namespace market_tracker_webapi_test.Application.Service;

public class CityServiceTest
{
    private readonly Mock<ICityRepository> _cityRepositoryMock;

    private readonly CityService _cityService;

    public CityServiceTest()
    {
        _cityRepositoryMock = new Mock<ICityRepository>();

        _cityService = new CityService(_cityRepositoryMock.Object, new MockedTransactionManager());
    }

    [Fact]
    public async Task GetCitiesAsync_ShouldReturnCitiesCollectionOutputModel()
    {
        // Arrange
        var cities = new List<City>
        {
            new(1, "City 1"),
            new(2, "City 2")
        };

        _cityRepositoryMock.Setup(c => c.GetCitiesAsync()).ReturnsAsync(cities);

        // Act
        var result = await _cityService.GetCitiesAsync();

        // Assert
        result.Value.Should().BeEquivalentTo(cities);
    }

    [Fact]
    public async Task GetCityByIdAsync_WhenCityExists_ShouldReturnCity()
    {
        // Arrange
        var city = new City(1, "City 1");

        _cityRepositoryMock.Setup(c => c.GetCityByIdAsync(It.IsAny<int>())).ReturnsAsync(city);

        // Act
        var result = await _cityService.GetCityByIdAsync(1);

        // Assert
        result.Value.Should().BeEquivalentTo(city);
    }

    [Fact]
    public async Task GetCityByIdAsync_WhenCityDoesNotExist_ShouldReturnCityByIdNotFound()
    {
        // Arrange
        _cityRepositoryMock
            .Setup(c => c.GetCityByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((City?)null);

        // Act
        var result = await _cityService.GetCityByIdAsync(1);

        // Assert
        result.Error.Should().BeEquivalentTo(new CityFetchingError.CityByIdNotFound(1));
    }

    [Fact]
    public async Task GetCityByNameAsync_WhenCityExists_ShouldReturnCity()
    {
        // Arrange
        var city = new City(1, "City 1");

        _cityRepositoryMock.Setup(c => c.GetCityByNameAsync(It.IsAny<string>())).ReturnsAsync(city);

        // Act
        var result = await _cityService.GetCityByNameAsync("City 1");

        // Assert
        result.Value.Should().BeEquivalentTo(city);
    }

    [Fact]
    public async Task GetCityByNameAsync_WhenCityDoesNotExist_ShouldReturnCityByNameNotFound()
    {
        // Arrange
        _cityRepositoryMock
            .Setup(c => c.GetCityByNameAsync(It.IsAny<string>()))
            .ReturnsAsync((City?)null);

        // Act
        var result = await _cityService.GetCityByNameAsync("City 1");

        // Assert
        result.Error.Should().BeEquivalentTo(new CityFetchingError.CityByNameNotFound("City 1"));
    }

    [Fact]
    public async Task AddCityAsync_WhenCityNameDoesNotExist_ShouldReturnCityId()
    {
        // Arrange
        _cityRepositoryMock
            .Setup(c => c.GetCityByNameAsync(It.IsAny<string>()))
            .ReturnsAsync((City?)null);

        // Act
        var result = await _cityService.AddCityAsync("City 1");

        // Assert
        result.Value.Should().BeEquivalentTo(new IntIdOutputModel(0));
    }

    [Fact]
    public async Task AddCityAsync_WhenCityNameExists_ShouldReturnCityNameAlreadyExists()
    {
        // Arrange
        var city = new City(1, "City 1");

        _cityRepositoryMock.Setup(c => c.GetCityByNameAsync(It.IsAny<string>())).ReturnsAsync(city);

        // Act
        var result = await _cityService.AddCityAsync("City 1");

        // Assert
        result
            .Error.Should()
            .BeEquivalentTo(new CityCreationError.CityNameAlreadyExists("City 1"));
    }

    [Fact]
    public async Task UpdateCityAsync_WhenCityExists_AndNameIsValid_ShouldReturnCityId()
    {
        // Arrange
        var city = new City(1, "City 1");

        _cityRepositoryMock
            .Setup(c => c.GetCityByNameAsync(It.IsAny<string>()))
            .ReturnsAsync((City)null!);

        _cityRepositoryMock
            .Setup(c => c.UpdateCityAsync(It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync(city);

        // Act
        var result = await _cityService.UpdateCityAsync(1, "City 1");

        // Assert
        result.Value.Should().BeEquivalentTo(new IntIdOutputModel(1));
    }

    [Fact]
    public async Task UpdateCityAsync_WhenCityDoesNotExist_ShouldReturnCityByIdNotFound()
    {
        // Arrange
        _cityRepositoryMock
            .Setup(c => c.GetCityByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((City?)null);

        // Act
        var result = await _cityService.UpdateCityAsync(1, "City 1");

        // Assert
        result.Error.Should().BeEquivalentTo(new CityFetchingError.CityByIdNotFound(1));
    }

    [Fact]
    public async Task UpdateCityAsync_WhenCityNameExists_ShouldReturnCityNameAlreadyExists()
    {
        // Arrange
        var city = new City(1, "City 1");

        _cityRepositoryMock.Setup(c => c.GetCityByNameAsync(It.IsAny<string>())).ReturnsAsync(city);

        // Act
        var result = await _cityService.UpdateCityAsync(1, "City 1");

        // Assert
        result
            .Error.Should()
            .BeEquivalentTo(new CityCreationError.CityNameAlreadyExists("City 1"));
    }

    [Fact]
    public async Task DeleteCityAsync_WhenCityExists_ShouldReturnCityId()
    {
        // Arrange
        var city = new City(1, "City 1");

        _cityRepositoryMock.Setup(c => c.GetCityByIdAsync(It.IsAny<int>())).ReturnsAsync(city);

        // Act
        var result = await _cityService.DeleteCityAsync(1);

        // Assert
        result.Value.Should().BeEquivalentTo(new IntIdOutputModel(1));
    }

    [Fact]
    public async Task DeleteCityAsync_WhenCityDoesNotExist_ShouldReturnCityByIdNotFound()
    {
        // Arrange
        _cityRepositoryMock
            .Setup(c => c.GetCityByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((City?)null);

        // Act
        var result = await _cityService.DeleteCityAsync(1);

        // Assert
        result.Error.Should().BeEquivalentTo(new CityFetchingError.CityByIdNotFound(1));
    }
}