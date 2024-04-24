using FluentAssertions;
using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Http.Controllers.List;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.ListEntry;
using market_tracker_webapi.Application.Repository.Dto.List;
using market_tracker_webapi.Application.Repository.Dto.Store;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.List;
using market_tracker_webapi.Application.Service.Errors.ListEntry;
using market_tracker_webapi.Application.Service.Operations.List;
using market_tracker_webapi.Application.Utils;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace market_tracker_webapi_test.Controllers;

public class ListEntryControllerTest
{
    /*private readonly Mock<IListEntryService> listEntryServiceMock;
    private readonly ListEntryController listEntryController;
    
    public ListEntryControllerTest()
    {
        listEntryServiceMock = new Mock<IListEntryService>();
        listEntryController = new ListEntryController(listEntryServiceMock.Object);
    }
    
    [Fact]
    public async Task GetListEntryByIdAsync_WhenListEntryExists_ReturnsListEntryDetails()
    {
        // Arrange
        var listEntryDetails = new ListEntryDetails()
        {
            ProductItem = It.IsAny<ProductItem>(),
            StorePrice = It.IsAny<StorePrice>(),
            IsAvailable = true,
            Quantity = 1
        };
        listEntryServiceMock.Setup(service => service.GetListEntryByIdAsync(It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync(EitherExtensions.Success<IServiceError, ListEntryDetails>(listEntryDetails));
        
        // Act
        var result = await listEntryController.GetListEntryByIdAsync(It.IsAny<int>(), It.IsAny<string>());
        
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var model = Assert.IsType<ListEntryDetails>(okResult.Value);
        model.Should().BeEquivalentTo(listEntryDetails);
    }
    
    [Fact]
    public async Task AddListEntryAsync_WhenListEntryIsAdded_ReturnsIntIdOutputModel()
    {
        // Arrange
        var intIdOutputModel = new IntIdOutputModel(1);
    
        listEntryServiceMock.Setup(service => service.AddListEntryAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(EitherExtensions.Success<IServiceError, IntIdOutputModel>(intIdOutputModel));

        var inputModel = new CreationListEntryInputModel
        {
            ListId = 1,
            ProductId = "product1",
            StoreId = 1,
            Quantity = 1
        };
    
        // Act
        var result = await listEntryController.AddListEntryAsync(inputModel);
    
        // Assert
        var createdResult = Assert.IsType<CreatedResult>(result.Result);
        var value = Assert.IsType<Either<IServiceError, IntIdOutputModel>>(createdResult.Value);
        value.Value.Should().BeEquivalentTo(intIdOutputModel);
    }
    
    [Fact]
    public async Task UpdateListEntryAsync_WhenListEntryIsUpdated_ReturnsListEntry()
    {
        // Arrange
        var  updateListEntryInputModel = new UpdateListEntryInputModel
        {
            StoreId = 1,
            Quantity = 1
        };
        
        var listEntry = new ListEntry
        {
            ListId = 1,
            ProductId = "product1",
            StoreId = 1,
            Quantity = 1
        };
        
        listEntryServiceMock.Setup(service => service.UpdateListEntryAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>()))
            .ReturnsAsync(EitherExtensions.Success<IServiceError, ListEntry>(listEntry));
        
        // Act
        var result = await listEntryController.UpdateListEntryAsync(It.IsAny<int>(), It.IsAny<string>(), updateListEntryInputModel);
        
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var model = Assert.IsType<ListEntry>(okResult.Value);
        model.Should().BeEquivalentTo(listEntry);
    }
    
    [Fact]
    public async Task DeleteListEntryAsync_WhenListEntryIsDeleted_ReturnsNoContent()
    {
        // Arrange
        var listEntry = new ListEntry
        {
            ListId = 1,
            ProductId = "product1",
            StoreId = 1,
            Quantity = 1
        };
    
        listEntryServiceMock.Setup(service => service.DeleteListEntryAsync(It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync(EitherExtensions.Success<ListEntryFetchingError, ListEntry>(listEntry));
    
        // Act
        var result = await listEntryController.DeleteListEntryAsync(It.IsAny<int>(), It.IsAny<string>());
    
        // Assert
        Assert.IsType<NoContentResult>(result.Result);
    }*/
}