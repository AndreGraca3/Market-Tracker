using FluentAssertions;
using market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Shop;
using market_tracker_webapi.Application.Http.Controllers.Market;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.Schemas.Market.Retail.City;
using market_tracker_webapi.Application.Http.Problems;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.City;
using market_tracker_webapi.Application.Service.Operations.Market.City;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace market_tracker_webapi_test.Application.Controllers;


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
    public async Task GetCitiesAsync_ReturnsCities()
    {
        // Arrange
        var cities = new List<City>()
        {
            new(1, "City 1"),
        };
        _cityServiceMock
            .Setup(service => service.GetCitiesAsync())
            .ReturnsAsync(cities);
        
        // Act
        var result = await _cityController.GetCitiesAsync();
        
        // Assert
        var actionResult = Assert.IsType<ActionResult<CollectionOutputModel<CityOutputModel>>>(result);
        var collectionOutputModel = Assert.IsType<CollectionOutputModel<CityOutputModel>>(actionResult.Value);
        
        collectionOutputModel.Should().BeEquivalentTo(new CollectionOutputModel<CityOutputModel>(cities.Select(c => c.ToOutputModel())));
    }
    
    [Fact]
    public async Task GetCityByIdAsync_ReturnsCity()
    {
        // Arrange
        var city = new City(1, "City 1");
        _cityServiceMock
            .Setup(service => service.GetCityByIdAsync(1))
            .ReturnsAsync(city);
        
        // Act
        var result = await _cityController.GetCityByIdAsync(1);
        
        // Assert
        var actionResult = Assert.IsType<ActionResult<CityOutputModel>>(result);
        var cityOutputModel = Assert.IsType<CityOutputModel>(actionResult.Value);
        
        cityOutputModel.Should().BeEquivalentTo(city.ToOutputModel());
    }
    
    // [Fact]
    // public async Task AddCityAsync_ReturnsCityId()
    // {
    //     // Arrange
    //     var cityId = new CityId(1);
    //     _cityServiceMock
    //         .Setup(service => service.AddCityAsync("City 1"))
    //         .ReturnsAsync(cityId);
    //     
    //     // Act
    //     var result = await _cityController.AddCityAsync(new CityCreationInputModel("City 1"));
    //     
    //     // Assert
    //     var actionResult = Assert.IsType<ActionResult<CityId>>(result);
    //     var cityIdResult = Assert.IsType<CityId>(actionResult.Value);
    //     
    //     cityIdResult.Should().BeEquivalentTo(cityId);
    // }
    
    [Fact]
    public async Task UpdateCityAsync_ReturnsUpdatedCity()
    {
        // Arrange
        var city = new City(1, "City 2");
        _cityServiceMock
            .Setup(service => service.UpdateCityAsync(1, "City 2"))
            .ReturnsAsync(city);
        
        // Act
        var result = await _cityController.UpdateCityAsync(1, new CityUpdateInputModel("City 2"));
        
        // Assert
        var actionResult = Assert.IsType<ActionResult<CityOutputModel>>(result);
        var cityOutputModel = Assert.IsType<CityOutputModel>(actionResult.Value);
        
        cityOutputModel.Should().BeEquivalentTo(city.ToOutputModel());
    }
    
    [Fact]
    public async Task DeleteCityAsync_ReturnsNoContent()
    {
        // Arrange
        _cityServiceMock
            .Setup(service => service.DeleteCityAsync(1))
            .ReturnsAsync(new CityId(1));
        
        // Act
        var result = await _cityController.DeleteCityAsync(1);
        
        // Assert
        var actionResult = Assert.IsType<NoContentResult>(result);
    }
}

