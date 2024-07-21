using FluentAssertions;
using market_tracker_webapi.Application.Domain.Filters.List;
using market_tracker_webapi.Application.Domain.Schemas.List;
using market_tracker_webapi.Application.Domain.Schemas.Market.Inventory;
using market_tracker_webapi.Application.Domain.Schemas.Market.Inventory.Product;
using market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Sales;
using market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Shop;
using market_tracker_webapi.Application.Http.Controllers.List;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.Schemas.List.ListEntry;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.ListEntry;
using market_tracker_webapi.Application.Service.Operations.List;
using market_tracker_webapi.Application.Service.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace market_tracker_webapi_test.Application.Controllers;


public class ListEntryControllerTest
{
    private readonly Mock<IListEntryService> listEntryServiceMock;
    private readonly ListEntryController listEntryController;
    
    public ListEntryControllerTest()
    {
        listEntryServiceMock = new Mock<IListEntryService>();
        listEntryController = new ListEntryController(listEntryServiceMock.Object);
    }
    
    // [Fact]
    // public async Task GetListEntries_ReturnsOk()
    // {
    //     // Arrange
    //     var shoppingListEntryResult = new ShoppingListEntriesResult
    //     {
    //         Entries = It.IsAny<IEnumerable<ListEntryOffer>>(),
    //         TotalPrice = 1,
    //         TotalProducts = 1
    //     };
    //     
    //     listEntryServiceMock
    //         .Setup(x => x.GetListEntriesAsync(
    //             It.IsAny<string>(),
    //             It.IsAny<Guid>(),
    //             It.IsAny<ShoppingListAlternativeType>(),
    //             It.IsAny<IList<int>>(),
    //             It.IsAny<IList<int>>(),
    //             It.IsAny<IList<int>>()
    //             ))
    //         .ReturnsAsync(shoppingListEntryResult);
    //
    //     // Act
    //     var result = await listEntryController.GetListEntriesAsync(
    //         It.IsAny<string>(),
    //         It.IsAny<ShoppingListAlternativeType>(),
    //         It.IsAny<AlternativeListFiltersInputModel>()
    //     );
    //
    //     // Assert
    //     result.Value.Should().BeEquivalentTo(shoppingListEntryResult.ToOutputModel());
    // }
}
