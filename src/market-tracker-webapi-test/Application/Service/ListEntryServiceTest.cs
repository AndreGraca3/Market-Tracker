using FluentAssertions;
using market_tracker_webapi.Application.Domain.Filters.List;
using market_tracker_webapi.Application.Domain.Schemas.List;
using market_tracker_webapi.Application.Domain.Schemas.Market.Inventory.Product;
using market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Sales;
using market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Sales.Pricing;
using market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Shop;
using market_tracker_webapi.Application.Repository.List;
using market_tracker_webapi.Application.Repository.List.ListEntry;
using market_tracker_webapi.Application.Repository.Market.Inventory.Product;
using market_tracker_webapi.Application.Repository.Market.Price;
using market_tracker_webapi.Application.Repository.Market.Store;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.List;
using market_tracker_webapi.Application.Service.Errors.ListEntry;
using market_tracker_webapi.Application.Service.Errors.Product;
using market_tracker_webapi.Application.Service.Errors.Store;
using market_tracker_webapi.Application.Service.Operations.List;
using Moq;
using DateTime = System.DateTime;

namespace market_tracker_webapi_test.Application.Service;

public class ListEntryServiceTest
{
    private readonly Mock<IListRepository> _listRepositoryMock;
    private readonly Mock<IPriceRepository> _priceRepositoryMock;
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<IListEntryRepository> _listEntryRepositoryMock;
    private readonly Mock<IStoreRepository> _storeRepositoryMock;

    private readonly ListEntryService _listEntryService;

    public ListEntryServiceTest()
    {
        _listRepositoryMock = new Mock<IListRepository>();
        _priceRepositoryMock = new Mock<IPriceRepository>();
        _productRepositoryMock = new Mock<IProductRepository>();
        _storeRepositoryMock = new Mock<IStoreRepository>();
        _listEntryRepositoryMock = new Mock<IListEntryRepository>();

        _listEntryService = new ListEntryService(
            _listRepositoryMock.Object,
            _listEntryRepositoryMock.Object,
            _priceRepositoryMock.Object,
            _productRepositoryMock.Object,
            _storeRepositoryMock.Object,
            new MockedTransactionManager());
    }

    [Fact]
    public async Task GetListEntriesAsync_ReturnsListEntries()
    {
        // Arrange
        const int pricePerProduct = 10;
        
        _listRepositoryMock.Setup(x => x.GetListByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(MockedData.DummyLists[0]);

        _listEntryRepositoryMock.Setup(x => x.GetListEntriesAsync(It.IsAny<string>(), null))
            .ReturnsAsync(MockedData.DummyListEntries);

        var dummyOffer = new StoreOffer(MockedData.DummyStores[0], new Price(pricePerProduct, null, DateTime.Now),
            new StoreAvailability(MockedData.DummyStores[0].Id.Value, MockedData.DummyProducts[0].Id.Value, true, DateTime.Now));

        _priceRepositoryMock.Setup(x =>
                x.GetStoreOfferAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<DateTime>()))
            .ReturnsAsync(dummyOffer);

        // Act
        var result =
            await _listEntryService.GetListEntriesAsync(
                MockedData.DummyListEntries[0].Id.Value, MockedData.DummyClients[0].Id.Value, null, null, null, null);

        // Assert
        foreach (var listEntryOffer in result.Entries)
        {
            listEntryOffer.ProductOffer.StoreOffer.Should().BeEquivalentTo(dummyOffer);
        }
        result.TotalPrice.Should().Be(pricePerProduct * MockedData.DummyListEntries.Count);
        result.TotalProducts.Should().Be(MockedData.DummyListEntries.Count);
    }

    [Fact]
    public async Task GetCheapestListEntriesAsync_ReturnsCheapestListEntries()
    {
        // Arrange
        _listRepositoryMock.Setup(x => x.GetListByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(MockedData.DummyLists[0]);

        _listEntryRepositoryMock.Setup(x => x.GetListEntriesAsync(It.IsAny<string>(), null))
            .ReturnsAsync(MockedData.DummyListEntries);

        var cheapestOffer = new StoreOffer(MockedData.DummyStores[0], new Price(10, null, DateTime.Now),
            new StoreAvailability(MockedData.DummyStores[0].Id.Value, MockedData.DummyProducts[0].Id.Value, true, DateTime.Now));

        _priceRepositoryMock.Setup(x =>
                x.GetCheapestStoreOfferAvailableByProductIdAsync(It.IsAny<string>(), null, null, null))
            .ReturnsAsync(cheapestOffer);

        // Act
        var result =
            await _listEntryService.GetListEntriesAsync(MockedData.DummyListEntries[0].Id.Value, MockedData.DummyClients[0].Id.Value,
                ShoppingListAlternativeType.Cheapest, null, null, null);

        // Assert
        foreach (var listEntryOffer in result.Entries)
        {
            listEntryOffer.ProductOffer.StoreOffer?.PriceData.Should().BeEquivalentTo(cheapestOffer.PriceData);
        }
    }

    [Fact]
    public async Task AddListEntryAsync_ReturnsListEntryId()
    {
        // Arrange
        _listRepositoryMock.Setup(x => x.GetListByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(MockedData.DummyLists[0]);

        _productRepositoryMock.Setup(x => x.GetProductByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(MockedData.DummyProducts[0]);

        _storeRepositoryMock.Setup(x => x.GetStoreByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(MockedData.DummyStores[0]);

        _priceRepositoryMock.Setup(x => x.GetStoreAvailabilityStatusAsync(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(
                new StoreAvailability(
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    true,
                    It.IsAny<DateTime>()
                ));

        _listEntryRepositoryMock.Setup(x =>
                x.AddListEntryAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(MockedData.DummyListEntries[0].Id);

        // Act
        var result =
            await _listEntryService.AddListEntryAsync(
                MockedData.DummyListEntries[0].Id.Value, MockedData.DummyClients[0].Id.Value, MockedData.DummyProducts[0].Id.Value, MockedData.DummyStores[0].Id.Value, 1);

        // Assert
        result.Should().BeEquivalentTo(MockedData.DummyListEntries[0].Id);
    }

    [Fact]
    public async Task AddListEntryAsync_ReturnsClientDoesNotBelongToList()
    {
        // Arrange
        _listRepositoryMock.Setup(x => x.GetListByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(MockedData.DummyLists[2]);

        _productRepositoryMock.Setup(x => x.GetProductByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(MockedData.DummyProducts[0]);

        _storeRepositoryMock.Setup(x => x.GetStoreByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(MockedData.DummyStores[0]);

        _priceRepositoryMock.Setup(x => x.GetStoreAvailabilityStatusAsync(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(
                new StoreAvailability(
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    true,
                    It.IsAny<DateTime>()
                ));

        // Act
        var result = await Assert.ThrowsAsync<MarketTrackerServiceException>(async () =>
            await _listEntryService.AddListEntryAsync(MockedData.DummyListEntries[0].Id.Value, MockedData.DummyClients[0].Id.Value,
                MockedData.DummyProducts[0].Id.Value, MockedData.DummyStores[0].Id.Value, 1));

        // Assert
        result.ServiceError
            .Should()
            .BeEquivalentTo(new ListFetchingError.ClientDoesNotBelongToList(MockedData.DummyClients[0].Id.Value, MockedData.DummyListEntries[0].Id.Value));
    }

    [Fact]
    public async Task AddListEntryAsync_ReturnsProductAlreadyInList()
    {
        // Arrange
        _listRepositoryMock.Setup(x => x.GetListByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(MockedData.DummyLists[0]);

        _productRepositoryMock.Setup(x => x.GetProductByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(MockedData.DummyProducts[0]);

        _storeRepositoryMock.Setup(x => x.GetStoreByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(MockedData.DummyStores[0]);

        _priceRepositoryMock.Setup(x => x.GetStoreAvailabilityStatusAsync(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(
                new StoreAvailability(
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    true,
                    It.IsAny<DateTime>()
                ));

        _listEntryRepositoryMock.Setup(x => x.GetListEntryByProductIdAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(MockedData.DummyListEntries[0]);

        // Act
        var result = await Assert.ThrowsAsync<MarketTrackerServiceException>(async () =>
            await _listEntryService.AddListEntryAsync(MockedData.DummyListEntries[0].Id.Value, MockedData.DummyClients[0].Id.Value,
                MockedData.DummyProducts[0].Id.Value, MockedData.DummyStores[0].Id.Value, 1));

        // Assert
        result.ServiceError
            .Should()
            .BeEquivalentTo(new ListEntryCreationError.ProductAlreadyInList(MockedData.DummyProducts[0].Id.Value, MockedData.DummyListEntries[0].Id.Value));
    }

    [Fact]
    public async Task AddListEntryAsync_ReturnsListEntryInvalidQuantity()
    {
        // Arrange
        const int invalidQuantity = 0;

        // Act
        var result = await Assert.ThrowsAsync<MarketTrackerServiceException>(async () =>
            await _listEntryService.AddListEntryAsync(MockedData.DummyListEntries[0].Id.Value, MockedData.DummyClients[0].Id.Value,
                MockedData.DummyProducts[0].Id.Value, MockedData.DummyStores[0].Id.Value, invalidQuantity));

        // Assert
        result.ServiceError
            .Should()
            .BeEquivalentTo(new ListEntryCreationError.ListEntryQuantityInvalid(invalidQuantity));
    }

    [Fact]
    public async Task AddListEntryAsync_ReturnsListByIdNotFound()
    {
        // Arrange
        _listRepositoryMock.Setup(x => x.GetListByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((ShoppingList)null!);

        // Act
        var result = await Assert.ThrowsAsync<MarketTrackerServiceException>(async () =>
            await _listEntryService.AddListEntryAsync(MockedData.DummyListEntries[0].Id.Value, MockedData.DummyClients[0].Id.Value,
                MockedData.DummyProducts[0].Id.Value, MockedData.DummyStores[0].Id.Value, 1));

        // Assert
        result.ServiceError
            .Should()
            .BeEquivalentTo(new ListFetchingError.ListByIdNotFound(MockedData.DummyListEntries[0].Id.Value));
    }

    [Fact]
    public async Task AddListEntryAsync_ReturnsListIsArchived()
    {
        // Arrange
        _listRepositoryMock.Setup(x => x.GetListByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(MockedData.DummyLists[1]);

        // Act
        var result = await Assert.ThrowsAsync<MarketTrackerServiceException>(async () =>
            await _listEntryService.AddListEntryAsync(MockedData.DummyListEntries[1].Id.Value, MockedData.DummyClients[0].Id.Value,
                MockedData.DummyProducts[0].Id.Value, MockedData.DummyStores[0].Id.Value, 1));

        // Assert
        result.ServiceError.Should().BeEquivalentTo(new ListUpdateError.ListIsArchived(MockedData.DummyListEntries[1].Id.Value));
    }

    [Fact]
    public async Task AddListEntryAsync_ReturnsProductByIdNotFound()
    {
        // Arrange
        _listRepositoryMock.Setup(x => x.GetListByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(MockedData.DummyLists[0]);

        _productRepositoryMock.Setup(x => x.GetProductByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((Product)null!);

        // Act
        var result = await Assert.ThrowsAsync<MarketTrackerServiceException>(async () =>
            await _listEntryService.AddListEntryAsync(MockedData.DummyListEntries[0].Id.Value, MockedData.DummyClients[0].Id.Value,
                MockedData.DummyProducts[0].Id.Value, MockedData.DummyStores[0].Id.Value, 1));

        // Assert
        result.ServiceError
            .Should()
            .BeEquivalentTo(new ProductFetchingError.ProductByIdNotFound(MockedData.DummyProducts[0].Id.Value));
    }

    [Fact]
    public async Task AddListEntryAsync_ReturnsStoreByIdNotFound()
    {
        // Arrange
        _listRepositoryMock.Setup(x => x.GetListByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(MockedData.DummyLists[0]);

        _productRepositoryMock.Setup(x => x.GetProductByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(MockedData.DummyProducts[0]);

        _storeRepositoryMock.Setup(x => x.GetStoreByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Store)null!);

        // Act
        var result = await Assert.ThrowsAsync<MarketTrackerServiceException>(async () =>
            await _listEntryService.AddListEntryAsync(MockedData.DummyListEntries[0].Id.Value, MockedData.DummyClients[0].Id.Value,
                MockedData.DummyProducts[0].Id.Value, MockedData.DummyStores[0].Id.Value, 1));

        // Assert
        result.ServiceError
            .Should()
            .BeEquivalentTo(new StoreFetchingError.StoreByIdNotFound(MockedData.DummyStores[0].Id.Value));
    }

    [Fact]
    public async Task AddListEntryAsync_ReturnsUnavailableProductInStore()
    {
        // Arrange
        _listRepositoryMock.Setup(x => x.GetListByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(MockedData.DummyLists[0]);

        _productRepositoryMock.Setup(x => x.GetProductByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(MockedData.DummyProducts[0]);

        _storeRepositoryMock.Setup(x => x.GetStoreByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(MockedData.DummyStores[0]);

        _priceRepositoryMock.Setup(x => x.GetStoreAvailabilityStatusAsync(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(new StoreAvailability(MockedData.DummyStores[0].Id.Value, MockedData.DummyProducts[0].Id.Value, false, DateTime.Now));

        // Act
        var result = await Assert.ThrowsAsync<MarketTrackerServiceException>(async () =>
            await _listEntryService.AddListEntryAsync(MockedData.DummyListEntries[0].Id.Value, MockedData.DummyClients[0].Id.Value,
                MockedData.DummyProducts[0].Id.Value, MockedData.DummyStores[0].Id.Value, 1));

        // Assert
        result.ServiceError
            .Should()
            .BeEquivalentTo(new ProductFetchingError.OutOfStockInStore(MockedData.DummyProducts[0].Id.Value, MockedData.DummyStores[0].Id.Value));
    }

    [Fact]
    public async Task UpdateListEntryAsync_ReturnsListEntry()
    {
        // Arrange
        _listRepositoryMock.Setup(x => x.GetListByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(MockedData.DummyLists[0]);

        _listEntryRepositoryMock.Setup(x => x.GetListEntryByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(MockedData.DummyListEntries[0]);

        _priceRepositoryMock.Setup(x => x.GetStoreAvailabilityStatusAsync(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(
                new StoreAvailability(
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    true,
                    It.IsAny<DateTime>()
                ));

        _storeRepositoryMock.Setup(x => x.GetStoreByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(MockedData.DummyStores[0]);

        _productRepositoryMock.Setup(x => x.GetProductByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(MockedData.DummyProducts[0]);

        _listEntryRepositoryMock.Setup(x =>
                x.UpdateListEntryByIdAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int?>()))
            .ReturnsAsync(MockedData.DummyListEntries[0]);

        // Act
        var result =
            await _listEntryService.UpdateListEntryAsync(
                MockedData.DummyListEntries[0].Id.Value, MockedData.DummyClients[0].Id.Value, MockedData.DummyProducts[0].Id.Value, 1, 1);

        // Assert
        result.Should().BeEquivalentTo(MockedData.DummyListEntries[0]);
    }

    [Fact]
    public async Task UpdateListEntryAsync_ReturnsListEntryByIdNotFound()
    {
        // Arrange
        _listRepositoryMock.Setup(x => x.GetListByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(MockedData.DummyLists[0]);

        _listEntryRepositoryMock.Setup(x => x.GetListEntryByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((ListEntry)null!);

        // Act
        var result = await Assert.ThrowsAsync<MarketTrackerServiceException>(async () =>
            await _listEntryService.UpdateListEntryAsync(MockedData.DummyListEntries[0].Id.Value, MockedData.DummyClients[0].Id.Value,
                MockedData.DummyProducts[0].Id.Value, 1, 1));

        // Assert
        result.ServiceError
            .Should()
            .BeEquivalentTo(new ListEntryFetchingError.ListEntryByIdNotFound(MockedData.DummyListEntries[0].Id.Value));
    }

    [Fact]
    public async Task UpdateListEntryAsync_ReturnsProductNotFoundInStore()
    {
        // Arrange
        _listRepositoryMock.Setup(x => x.GetListByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(MockedData.DummyLists[0]);

        _listEntryRepositoryMock.Setup(x => x.GetListEntryByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(MockedData.DummyListEntries[0]);

        _productRepositoryMock.Setup(x => x.GetProductByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(MockedData.DummyProducts[0]);

        _storeRepositoryMock.Setup(x => x.GetStoreByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(MockedData.DummyStores[0]);

        _priceRepositoryMock.Setup(x => x.GetStoreAvailabilityStatusAsync(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync((StoreAvailability)null!);

        // Act
        var result = await Assert.ThrowsAsync<MarketTrackerServiceException>(async () =>
            await _listEntryService.UpdateListEntryAsync(MockedData.DummyListEntries[0].Id.Value, MockedData.DummyClients[0].Id.Value,
                MockedData.DummyProducts[0].Id.Value, 1, 1));

        // Assert
        result.ServiceError
            .Should()
            .BeEquivalentTo(new ProductFetchingError.ProductNotFoundInStore(MockedData.DummyProducts[0].Id.Value, MockedData.DummyStores[0].Id.Value));
    }

    [Fact]
    public async Task UpdateListEntryAsync_ReturnsProductOutOfStockInStore()
    {
        // Arrange
        _listRepositoryMock.Setup(x => x.GetListByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(MockedData.DummyLists[0]);

        _listEntryRepositoryMock.Setup(x => x.GetListEntryByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(MockedData.DummyListEntries[0]);

        _productRepositoryMock.Setup(x => x.GetProductByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(MockedData.DummyProducts[0]);

        _storeRepositoryMock.Setup(x => x.GetStoreByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(MockedData.DummyStores[0]);

        _priceRepositoryMock.Setup(x => x.GetStoreAvailabilityStatusAsync(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(new StoreAvailability(MockedData.DummyStores[0].Id.Value, MockedData.DummyProducts[0].Id.Value, false, DateTime.Now));

        // Act
        var result = await Assert.ThrowsAsync<MarketTrackerServiceException>(async () =>
            await _listEntryService.UpdateListEntryAsync(MockedData.DummyListEntries[0].Id.Value, MockedData.DummyClients[0].Id.Value,
                MockedData.DummyProducts[0].Id.Value, 1, 1));

        // Assert
        result.ServiceError
            .Should()
            .BeEquivalentTo(new ProductFetchingError.OutOfStockInStore(MockedData.DummyProducts[0].Id.Value, MockedData.DummyStores[0].Id.Value));
    }

    [Fact]
    public async Task UpdateListEntryAsync_ReturnsListEntryQuantityInvalid()
    {
        // Arrange
        const int invalidQuantity = 0;

        _listRepositoryMock.Setup(x => x.GetListByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(MockedData.DummyLists[0]);

        _listEntryRepositoryMock.Setup(x => x.GetListEntryByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(MockedData.DummyListEntries[0]);

        _priceRepositoryMock.Setup(x => x.GetStoreAvailabilityStatusAsync(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(
                new StoreAvailability(
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    true,
                    It.IsAny<DateTime>()
                ));

        // Act
        var result = await Assert.ThrowsAsync<MarketTrackerServiceException>(async () =>
            await _listEntryService.UpdateListEntryAsync(MockedData.DummyListEntries[0].Id.Value, MockedData.DummyClients[0].Id.Value,
                MockedData.DummyProducts[0].Id.Value, 1, invalidQuantity));

        // Assert
        result.ServiceError
            .Should()
            .BeEquivalentTo(new ListEntryCreationError.ListEntryQuantityInvalid(invalidQuantity));
    }

    [Fact]
    public async Task DeleteListEntryAsync_ReturnsListEntry()
    {
        // Arrange
        _listRepositoryMock.Setup(x => x.GetListByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(MockedData.DummyLists[0]);

        _listEntryRepositoryMock.Setup(x => x.DeleteListEntryByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(MockedData.DummyListEntries[0]);

        // Act
        var result =
            await _listEntryService.DeleteListEntryAsync(MockedData.DummyListEntries[0].Id.Value, MockedData.DummyClients[0].Id.Value,
                MockedData.DummyListEntries[0].Id.Value);

        // Assert
        result.Should().BeEquivalentTo(MockedData.DummyListEntries[0].Id);
    }

    [Fact]
    public async Task DeleteListEntryAsync_ReturnsListEntryByIdNotFound()
    {
        // Arrange
        _listRepositoryMock.Setup(x => x.GetListByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(MockedData.DummyLists[0]);

        _listEntryRepositoryMock.Setup(x => x.GetListEntryByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((ListEntry)null!);

        // Act
        var result = await Assert.ThrowsAsync<MarketTrackerServiceException>(async () =>
            await _listEntryService.DeleteListEntryAsync(MockedData.DummyListEntries[0].Id.Value, MockedData.DummyClients[0].Id.Value,
                MockedData.DummyListEntries[0].Id.Value));

        // Assert
        result.ServiceError
            .Should()
            .BeEquivalentTo(new ListEntryFetchingError.ListEntryByIdNotFound(MockedData.DummyListEntries[0].Id.Value));
    }
}