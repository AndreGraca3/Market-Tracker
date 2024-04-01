using FluentAssertions;
using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Http.Controllers;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Problem;
using market_tracker_webapi.Application.Models;
using market_tracker_webapi.Application.Service.Errors.City;
using market_tracker_webapi.Application.Service.Operations.City;
using market_tracker_webapi.Application.Utils;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace market_tracker_webapi_test.Controllers;

public class CityControllerTest
{
    private readonly Mock<ICityService> _cityServiceMock;
    private readonly CityController _cityController;
    
    public CityControllerTest()
    {
        _cityServiceMock = new Mock<ICityService>();
        _cityController = new CityController(_cityServiceMock.Object);
    }
    
    [Fact]
    public async Task GetCitiesAsync_ShouldReturnOk()
    {
        // Arrange
        var cities = new List<CityDomain>
        {
            new(){Id = 1, Name = "City 1"},
            new(){Id = 2, Name = "City 1"}
        };
        _cityServiceMock
            .Setup(service => service.GetCitiesAsync())
            .ReturnsAsync(cities);
        
        // Act
        var result = await _cityController.GetCitiesAsync();
        
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var actualCities = Assert.IsType<List<CityDomain>>(okResult.Value);
        actualCities.Should().BeEquivalentTo(cities);
    }
    
    [Fact]
    public async Task GetCityByIdAsync_ShouldReturnOk()
    {
        // Arrange
        var city = new CityDomain {Id = 1, Name = "City 1"};
        _cityServiceMock
            .Setup(service => service.GetCityByIdAsync(1))
            .ReturnsAsync(EitherExtensions.Success<CityFetchingError, CityDomain>(city));
        
        // Act
        var result = await _cityController.GetCityByIdAsync(1);
        
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var actualCity = Assert.IsType<CityDomain>(okResult.Value);
        actualCity.Should().BeEquivalentTo(city);
    }
    
    [Fact]
    public async Task GetCityByIdAsync_ShouldReturnNotFound()
    {
        // Arrange
        _cityServiceMock
            .Setup(service => service.GetCityByIdAsync(1))
            .ReturnsAsync(EitherExtensions.Failure<CityFetchingError, CityDomain>(new CityFetchingError.CityByIdNotFound(It.IsAny<int>())));
        
        // Act
        var actual = await _cityController.GetCityByIdAsync(1);
        
        // Assert
        ObjectResult result = Assert.IsType<ObjectResult>(actual.Result);
        var problem = Assert.IsType<CityProblem.CityByIdNotFound>(result.Value);
        problem.Data.Should().BeEquivalentTo(new CityFetchingError.CityByIdNotFound(It.IsAny<int>()));
    }
    
    [Fact]
    public async Task AddCityAsync_ShouldReturnOk()
    {
        // Arrange
        var cityId = new IdOutputModel{ Id = 1 };
        _cityServiceMock
            .Setup(service => service.AddCityAsync("City 1"))
            .ReturnsAsync(EitherExtensions.Success<ICityError, IdOutputModel>(cityId));
        
        // Act
        var result = await _cityController.AddCityAsync(new CityCreationInputModel {CityName = "City 1"});
        
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var actualCity = Assert.IsType<IdOutputModel>(okResult.Value);
        actualCity.Should().BeEquivalentTo(cityId);
    }
    
    [Fact]
    public async Task AddCityAsync_ShouldReturnConflict()
    {
        // Arrange
        _cityServiceMock
            .Setup(service => service.AddCityAsync("City 1"))
            .ReturnsAsync(EitherExtensions.Failure<ICityError, IdOutputModel>(new CityCreationError.CityNameAlreadyExists("City 1")));
        
        // Act
        var actual = await _cityController.AddCityAsync(new CityCreationInputModel {CityName = "City 1"});
        
        // Assert
        ObjectResult result = Assert.IsType<ObjectResult>(actual.Result);
        var problem = Assert.IsType<CityProblem.CityNameAlreadyExists>(result.Value);
        problem.Data.Should().BeEquivalentTo(new CityCreationError.CityNameAlreadyExists("City 1"));
    }
    
    [Fact]
    public async Task UpdateCityAsync_ShouldReturnOk()
    {
        // Arrange
        var city = new CityDomain {Id = 1, Name = "City 1"};
        _cityServiceMock
            .Setup(service => service.UpdateCityAsync(1, "City 1"))
            .ReturnsAsync(EitherExtensions.Success<ICityError, CityDomain>(city));
        
        // Act
        var result = await _cityController.UpdateCityAsync(1, new CityUpdateInputModel {CityName = "City 1"});
        
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var actualCity = Assert.IsType<CityDomain>(okResult.Value);
        actualCity.Should().BeEquivalentTo(city);
    }
    
    [Fact]
    public async Task UpdateCityAsync_ShouldReturnNotFound()
    {
        // Arrange
        _cityServiceMock
            .Setup(service => service.UpdateCityAsync(1, "City 1"))
            .ReturnsAsync(EitherExtensions.Failure<ICityError, CityDomain>(new CityFetchingError.CityByIdNotFound(It.IsAny<int>())));
        
        // Act
        var actual = await _cityController.UpdateCityAsync(1, new CityUpdateInputModel {CityName = "City 1"});
        
        // Assert
        ObjectResult result = Assert.IsType<ObjectResult>(actual.Result);
        var problem = Assert.IsType<CityProblem.CityByIdNotFound>(result.Value);
        problem.Data.Should().BeEquivalentTo(new CityFetchingError.CityByIdNotFound(It.IsAny<int>()));
    }
    
    [Fact]
    public async Task UpdateCityAsync_ShouldReturnConflict()
    {
        // Arrange
        _cityServiceMock
            .Setup(service => service.UpdateCityAsync(1, "City 1"))
            .ReturnsAsync(EitherExtensions.Failure<ICityError, CityDomain>(new CityCreationError.CityNameAlreadyExists("City 1")));
        
        // Act
        var actual = await _cityController.UpdateCityAsync(1, new CityUpdateInputModel {CityName = "City 1"});
        
        // Assert
        ObjectResult result = Assert.IsType<ObjectResult>(actual.Result);
        var problem = Assert.IsType<CityProblem.CityNameAlreadyExists>(result.Value);
        problem.Data.Should().BeEquivalentTo(new CityCreationError.CityNameAlreadyExists("City 1"));
    }
    
    [Fact]
    public async Task DeleteCityAsync_ShouldReturnOk()
    {
        // Arrange
        var expectedId = new IdOutputModel { Id = 1 };
        
        _cityServiceMock
            .Setup(service => service.DeleteCityAsync(1))
            .ReturnsAsync(EitherExtensions.Success<CityFetchingError, IdOutputModel>(expectedId));
        
        // Act
        var result = await _cityController.DeleteCityAsync(1);
        
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var actual = Assert.IsType<IdOutputModel>(okResult.Value);
        actual.Should().BeEquivalentTo(expectedId);
    }
    
    [Fact]
    public async Task DeleteCityAsync_ShouldReturnNotFound()
    {
        // Arrange
        _cityServiceMock
            .Setup(service => service.DeleteCityAsync(1))
            .ReturnsAsync(EitherExtensions.Failure<CityFetchingError, IdOutputModel>(new CityFetchingError.CityByIdNotFound(It.IsAny<int>())));
        
        // Act
        var actual = await _cityController.DeleteCityAsync(1);
        
        // Assert
        ObjectResult result = Assert.IsType<ObjectResult>(actual.Result);
        var problem = Assert.IsType<CityProblem.CityByIdNotFound>(result.Value);
        problem.Data.Should().BeEquivalentTo(new CityFetchingError.CityByIdNotFound(It.IsAny<int>()));
    }
}