using FluentAssertions;
using market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Shop;
using market_tracker_webapi.Application.Http.Controllers.Market.Store;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.Schemas.Market.Retail.Store;
using market_tracker_webapi.Application.Service.Operations.Market.Store;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace market_tracker_webapi_test.Application.Controllers;


public class StoreControllerTest
{
    private readonly Mock<IStoreService> _storeServiceMock;
    private readonly StoreController _storeController;

    public StoreControllerTest()
    {
        _storeServiceMock = new Mock<IStoreService>();
        _storeController = new StoreController(_storeServiceMock.Object);
    }

    [Fact]
    public async Task GetStores_ReturnsOk()
    {
        // Arrange
        var stores = new List<Store>
        {
            new Store(
                new StoreId(1), 
                "Store 1", "Address 1", 
                new City(1, "City 1"), 
                new Company(1, "Company 1", "logoUrl1", new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Unspecified)), 
                Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301")),
        };
        
        _storeServiceMock
            .Setup(x => x.GetStoresAsync(It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<string?>()))
            .ReturnsAsync(stores);

        // Act
        var result = await _storeController.GetStoresAsync(It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<string?>());

        // Assert
        result.Value.Should().BeEquivalentTo(stores.Select(s => s.ToOutputModel()).ToCollectionOutputModel());
    }
    
    [Fact]
    public async Task GetStore_ReturnsOk()
    {
        // Arrange
        var store = new Store(
            new StoreId(1), 
            "Store 1", "Address 1", 
            new City(1, "City 1"), 
            new Company(1, "Company 1", "logoUrl1", new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Unspecified)), 
            Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301"));
        
        _storeServiceMock
            .Setup(x => x.GetStoreByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(store);

        // Act
        var result = await _storeController.GetStoreByIdAsync(It.IsAny<int>());

        // Assert
        result.Value.Should().BeEquivalentTo(store.ToOutputModel());
    }
    
    // [Fact]
    // public async Task UpdateStore_ReturnsOk()
    // {
    //     // Arrange
    //     var storeItem = new StoreItem(
    //         1, 
    //         "Store 1", 
    //         "Address 1", 
    //         1, 
    //         1,
    //         Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301"));
    //     
    //     _storeServiceMock
    //         .Setup(x => x.UpdateStoreAsync(
    //             It.IsAny<int>(), 
    //             It.IsAny<string>(), 
    //             It.IsAny<string>(), 
    //             It.IsAny<int>(), 
    //             It.IsAny<int>()))
    //         .ReturnsAsync(storeItem);
    //     
    //     // Act
    //     var result = await _storeController.UpdateStoreAsync(It.IsAny<int>(), It.IsAny<StoreUpdateInputModel>());
    //
    //     // Assert
    //     result.Result.Should().BeOfType<OkObjectResult>();
    // }
    
    [Fact]
    public async Task RemoveStore_ReturnsNoContent()
    {
        // Arrange
        _storeServiceMock
            .Setup(x => x.DeleteStoreAsync(It.IsAny<int>()))
            .ReturnsAsync(new StoreId(It.IsAny<int>()));
        
        // Act
        var result = await _storeController.DeleteStoreAsync(It.IsAny<int>());
        
        // Assert
        result.Should().BeOfType<NoContentResult>();
    }
}
