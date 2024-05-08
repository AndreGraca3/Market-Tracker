using FluentAssertions;
using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Http;
using market_tracker_webapi.Application.Http.Controllers.List;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.List;
using market_tracker_webapi.Application.Http.Models.ListEntry;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.List;
using market_tracker_webapi.Application.Service.Operations.List;
using market_tracker_webapi.Application.Utils;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace market_tracker_webapi_test.Controllers;

/*
public class ListControllerTest
{
    private readonly Mock<IListService> listServiceMock;
    private readonly ListController listController;
    
    public ListControllerTest()
    {
        listServiceMock = new Mock<IListService>();
        listController = new ListController(listServiceMock.Object);
    }
    
    [Fact]
    public async Task GetListsAsync_ShouldReturnOkObjectResult_WhenServiceReturnsSuccess()
    {
        // Arrange
        var lists = new List<ShoppingList>()
        {
            new()
            {
                Id = 1,
                Name = "List 1",
                ClientId = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a10"),
                ArchivedAt = null,
                CreatedAt = new DateTime(2024, 1, 1, 1, 1, 1, DateTimeKind.Unspecified)
            },
            new()
            {
                Id = 2,
                Name = "List 2",
                ClientId = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a10"),
                ArchivedAt = null,
                CreatedAt = new DateTime(2024, 1, 1, 1, 1, 1, DateTimeKind.Unspecified)
            }
        };
        
        listServiceMock
            .Setup(service => service.GetListsAsync(
                It.IsAny<Guid>(),
                It.IsAny<string>(),
                It.IsAny<DateTime?>(),
                It.IsAny<DateTime?>()))
            .ReturnsAsync(EitherExtensions.Success<IServiceError, CollectionOutputModel>(new CollectionOutputModel(lists)));
        
        // Act
        var result = await listController.GetListsAsync(
            Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a10"),
            null,
            null,
            null);
        
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var actualListCollection = Assert.IsType<CollectionOutputModel>(okResult.Value);
        actualListCollection.Should().BeEquivalentTo(new CollectionOutputModel(lists));
        actualListCollection.Items.Should().BeEquivalentTo(lists);
    }
    
    [Fact]
    public async Task GetListByIdAsync_ShouldReturnOkObjectResult_WhenServiceReturnsSuccess()
    {
        // Arrange
        var list = new ShoppingListOutputModel()
        {
            Id = 1,
            Name = "List 1",
            ArchivedAt = null,
            CreatedAt = new DateTime(2024, 1, 1, 1, 1, 1, DateTimeKind.Unspecified),
            TotalPrice = 10,
            TotalProducts = 1,
            Products = new List<ListEntryDetails>()
            {
                new()
                {
                    ProductItem = It.IsAny<ProductItem>(),
                    StorePrice = It.IsAny<StorePrice>(),
                    IsAvailable = true,
                    Quantity = 1
                }
            }
        };
        
        listServiceMock
            .Setup(service => service.GetListByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(EitherExtensions.Success<ListFetchingError, ShoppingListOutputModel>(list));
        
        // Act
        var result = await listController.GetListByIdAsync(1);
        
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var actualList = Assert.IsType<ShoppingListOutputModel>(okResult.Value);
        actualList.Should().BeEquivalentTo(list);
    }
    
    [Fact]
    public async Task AddListAsync_ShouldReturnCreatedResult_WhenServiceReturnsSuccess()
    {
        // Arrange
        var listId = 1;
        var outputModel = new IntIdOutputModel(listId);
    
        listServiceMock
            .Setup(service => service.AddListAsync(
                It.IsAny<Guid>(),
                It.IsAny<string>()))
            .ReturnsAsync(EitherExtensions.Success<IServiceError, IntIdOutputModel>(outputModel));
    
        // Act
        var result = await listController.AddListAsync(new ListCreationInputModel()
        {
            ListName = "List 1",
            ClientId = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a10")
        });
    
        // Assert
        var createdResult = Assert.IsType<CreatedResult>(result.Result);
        var value = Assert.IsType<Either<IServiceError, IntIdOutputModel>>(createdResult.Value);
        value.Value.Should().BeEquivalentTo(new IntIdOutputModel(1));
    }
    
    [Fact]
    public async Task UpdateListAsync_ShouldReturnOkObjectResult_WhenServiceReturnsSuccess()
    {
        // Arrange
        var list = new ShoppingList()
        {
            Id = 1,
            Name = "List 1",
            ClientId = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a10"),
            ArchivedAt = null,
            CreatedAt = new DateTime(2024, 1, 1, 1, 1, 1, DateTimeKind.Unspecified)
        };
        
        listServiceMock
            .Setup(service => service.UpdateListAsync(
                It.IsAny<int>(),
                It.IsAny<Guid>(),
                It.IsAny<string>(),
                It.IsAny<DateTime?>()))
            .ReturnsAsync(EitherExtensions.Success<IServiceError, ShoppingList>(list));
        
        // Act
        var result = await listController.UpdateListAsync(1, Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a10"), new UpdateListInputModel()
        {
            ListName = "List 1",
            IsArchived = null
        });
        
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var actualList = Assert.IsType<ShoppingList>(okResult.Value);
        actualList.Should().BeEquivalentTo(list);
    }
    
    [Fact]
    public async Task DeleteListAsync_ShouldReturnNoContentResult_WhenServiceReturnsSuccess()
    {
        // Arrange
        var list = new ShoppingList()
        {
            Id = 1,
            Name = "List 1",
            ClientId = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a10"),
            ArchivedAt = null,
            CreatedAt = new DateTime(2024, 1, 1, 1, 1, 1, DateTimeKind.Unspecified)
        };

        listServiceMock
            .Setup(service => service.DeleteListAsync(It.IsAny<int>()))
            .ReturnsAsync(EitherExtensions.Success<ListFetchingError, ShoppingList>(list));

        // Act
        var result = await listController.DeleteListAsync(list.Id);

        // Assert
        Assert.IsType<NoContentResult>(result.Result);
    }
}
*/