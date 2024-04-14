using FluentAssertions;
using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Repository.Dto.List;
using market_tracker_webapi.Application.Repository.Dto.Product;
using market_tracker_webapi.Application.Repository.Dto.Store;
using market_tracker_webapi.Application.Repository.Operations.List;
using market_tracker_webapi.Application.Repository.Operations.Prices;
using market_tracker_webapi.Application.Repository.Operations.Product;
using market_tracker_webapi.Application.Repository.Operations.Store;
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
    public async Task GetListEntryByIdAsync_ReturnsListEntryDetails()
    {
        // Arrange
        const int listId = 1;
        const string productId = "product1";
        
        var listEntry = new ListEntry
        {
            ListId = listId,
            ProductId = productId,
            StoreId = 1,
            Quantity = 1
        };
        
        var product = new ProductDetails(
            It.IsAny<string>(),
            "Product",
            It.IsAny<string>(),
            It.IsAny<int>(),
            It.IsAny<string>(),
            It.IsAny<int>(),
            It.IsAny<double>(),
            It.IsAny<Brand>(),
            It.IsAny<Category>());

        var listEntryDetails = new ListEntryDetails()
        {
            ProductItem = new ProductItem()
            {
                ProductId = "product1",
                Name = "Product"
            },
            Quantity = 1,
            StorePrice = It.IsAny<StorePrice>(),
            IsAvailable = false
        };
        
        _listEntryRepositoryMock.Setup(x => x.GetListEntryAsync(It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync(listEntry);
        
        _productRepositoryMock.Setup(x => x.GetProductByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(product);
        
        _priceRepositoryMock.Setup(x => x.GetStorePriceByProductIdAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<DateTime>()))
            .ReturnsAsync(It.IsAny<StorePrice>());
        
        _priceRepositoryMock.Setup(x => x.GetStoresAvailabilityAsync(It.IsAny<string>(), It.IsAny<int?>()))
            .ReturnsAsync(It.IsAny<IEnumerable<StoreAvailability>>());
        
        // Act
        var result = await _listEntryService.GetListEntryByIdAsync(listId, productId);
        
        // Assert
        result.Value.Should().BeEquivalentTo(listEntryDetails);
    }
    
    [Fact]
    public async Task GetListEntryByIdAsync_ReturnsListEntryNotFound()
    {
        // Arrange
        const int listId = 1;
        const string productId = "product1";
        
        _listEntryRepositoryMock.Setup(x => x.GetListEntryAsync(It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync((ListEntry)null!);
        
        // Act
        var result = await _listEntryService.GetListEntryByIdAsync(listId, productId);
        
        // Assert
        result.Error.Should().BeEquivalentTo(new ListEntryFetchingError.ListEntryByIdNotFound(listId, productId));
    }
    
    [Fact]
    public async Task AddListEntryAsync_ReturnsListEntryId()
    {
        // Arrange
        const int listId = 1;
        const string productId = "product1";
        const int storeId = 1;
        const int quantity = 1;
        
        _listRepositoryMock.Setup(x => x.GetListByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new ListOfProducts()
            {
                Id = It.IsAny<int>(),
                ListName = "List1",
                ArchivedAt = null,
                CreatedAt = It.IsAny<DateTime>(),
                ClientId = It.IsAny<Guid>()
            });
        
        _productRepositoryMock.Setup(x => x.GetProductByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(new ProductDetails(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<double>(),
                It.IsAny<Brand>(),
                It.IsAny<Category>()));
        
        _storeRepositoryMock.Setup(x => x.GetStoreByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new Store()
            {
                Id = It.IsAny<int>(),
                Address = It.IsAny<string>(),
                Name = It.IsAny<string>(),
                CityId = It.IsAny<int?>(),
                CompanyId = It.IsAny<int>()
            });
        
        _priceRepositoryMock.Setup(x => x.GetStoresAvailabilityAsync(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(new List<StoreAvailability>() { 
                new StoreAvailability(
                    It.IsAny<int>(), 
                    It.IsAny<string>(), 
                    It.IsAny<int>(),
                    It.IsAny<bool>(),
                    It.IsAny<DateTime>()
            )});
        
        _listEntryRepositoryMock.Setup(x => x.AddListEntryAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(1);
        
        // Act
        var result = await _listEntryService.AddListEntryAsync(listId, productId, storeId, quantity);
        
        // Assert
        result.Value.Should().BeEquivalentTo(new IntIdOutputModel(1));
    }
    
    [Fact]
    public async Task AddListEntryAsync_ReturnsListEntryQuantityInvalid()
    {
        // Arrange
        const int listId = 1;
        const string productId = "product1";
        const int storeId = 1;
        const int quantity = 0;
        
        // Act
        var result = await _listEntryService.AddListEntryAsync(listId, productId, storeId, quantity);
        
        // Assert
        result.Error.Should().BeEquivalentTo(new ListEntryCreationError.ListEntryQuantityInvalid(quantity));
    }
    
    [Fact]
    public async Task AddListEntryAsync_ReturnsListByIdNotFound()
    {
        // Arrange
        const int listId = 1;
        const string productId = "product1";
        const int storeId = 1;
        const int quantity = 1;
        
        _listRepositoryMock.Setup(x => x.GetListByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((ListOfProducts)null!);
        
        // Act
        var result = await _listEntryService.AddListEntryAsync(listId, productId, storeId, quantity);
        
        // Assert
        result.Error.Should().BeEquivalentTo(new ListFetchingError.ListByIdNotFound(listId));
    }
    
    [Fact]
    public async Task AddListEntryAsync_ReturnsListIsArchived()
    {
        // Arrange
        const int listId = 1;
        const string productId = "product1";
        const int storeId = 1;
        const int quantity = 1;
        
        _listRepositoryMock.Setup(x => x.GetListByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new ListOfProducts()
            {
                Id = It.IsAny<int>(),
                ListName = "List1",
                ArchivedAt = DateTime.Now,
                CreatedAt = It.IsAny<DateTime>(),
                ClientId = It.IsAny<Guid>()
            });
        
        // Act
        var result = await _listEntryService.AddListEntryAsync(listId, productId, storeId, quantity);
        
        // Assert
        result.Error.Should().BeEquivalentTo(new ListUpdateError.ListIsArchived(listId));
    }
    
    [Fact]
    public async Task AddListEntryAsync_ReturnsProductByIdNotFound()
    {
        // Arrange
        const int listId = 1;
        const string productId = "product1";
        const int storeId = 1;
        const int quantity = 1;
        
        _listRepositoryMock.Setup(x => x.GetListByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new ListOfProducts()
            {
                Id = It.IsAny<int>(),
                ListName = "List1",
                ArchivedAt = null,
                CreatedAt = It.IsAny<DateTime>(),
                ClientId = It.IsAny<Guid>()
            });
        
        _productRepositoryMock.Setup(x => x.GetProductByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((ProductDetails)null!);
        
        // Act
        var result = await _listEntryService.AddListEntryAsync(listId, productId, storeId, quantity);
        
        // Assert
        result.Error.Should().BeEquivalentTo(new ProductFetchingError.ProductByIdNotFound(productId));
    }
    
    [Fact]
    public async Task AddListEntryAsync_ReturnsStoreByIdNotFound()
    {
        // Arrange
        const int listId = 1;
        const string productId = "product1";
        const int storeId = 1;
        const int quantity = 1;
        
        _listRepositoryMock.Setup(x => x.GetListByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new ListOfProducts()
            {
                Id = It.IsAny<int>(),
                ListName = "List1",
                ArchivedAt = null,
                CreatedAt = It.IsAny<DateTime>(),
                ClientId = It.IsAny<Guid>()
            });
        
        _productRepositoryMock.Setup(x => x.GetProductByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(new ProductDetails(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<double>(),
                It.IsAny<Brand>(),
                It.IsAny<Category>()));
        
        _storeRepositoryMock.Setup(x => x.GetStoreByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Store)null!);
        
        // Act
        var result = await _listEntryService.AddListEntryAsync(listId, productId, storeId, quantity);
        
        // Assert
        result.Error.Should().BeEquivalentTo(new StoreFetchingError.StoreByIdNotFound(storeId));
    }
    
    [Fact]
    public async Task AddListEntryAsync_ReturnsUnavailableProductInStore()
    {
        // Arrange
        const int listId = 1;
        const string productId = "product1";
        const int storeId = 1;
        const int quantity = 1;
        
        _listRepositoryMock.Setup(x => x.GetListByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new ListOfProducts()
            {
                Id = It.IsAny<int>(),
                ListName = "List1",
                ArchivedAt = null,
                CreatedAt = It.IsAny<DateTime>(),
                ClientId = It.IsAny<Guid>()
            });
        
        _productRepositoryMock.Setup(x => x.GetProductByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(new ProductDetails(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<double>(),
                It.IsAny<Brand>(),
                It.IsAny<Category>()));
        
        _storeRepositoryMock.Setup(x => x.GetStoreByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new Store()
            {
                Id = It.IsAny<int>(),
                Address = It.IsAny<string>(),
                Name = It.IsAny<string>(),
                CityId = It.IsAny<int?>(),
                CompanyId = It.IsAny<int>()
            });
        
        _priceRepositoryMock.Setup(x => x.GetStoresAvailabilityAsync(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(new List<StoreAvailability>());
        
        // Act
        var result = await _listEntryService.AddListEntryAsync(listId, productId, storeId, quantity);
        
        // Assert
        result.Error.Should().BeEquivalentTo(new ProductFetchingError.UnavailableProductInStore(productId, storeId));
    }
    
    [Fact]
    public async Task UpdateListEntryAsync_ReturnsListEntry()
    {
        // Arrange
        const int listId = 1;
        const string productId = "product1";
        
        var listEntry = new ListEntry
        {
            ListId = listId,
            ProductId = productId,
            StoreId = 1,
            Quantity = 1
        };
        
        _listEntryRepositoryMock.Setup(x => x.GetListEntryAsync(It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync(listEntry);
        
        _priceRepositoryMock.Setup(x => x.GetStoresAvailabilityAsync(It.IsAny<string>(), It.IsAny<int?>()))
            .ReturnsAsync(new List<StoreAvailability>() { 
                new StoreAvailability(
                    It.IsAny<int>(), 
                    It.IsAny<string>(), 
                    It.IsAny<int>(),
                    It.IsAny<bool>(),
                    It.IsAny<DateTime>()
            )});
        
        _listEntryRepositoryMock.Setup(x => x.UpdateListEntryAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>()))
            .ReturnsAsync(listEntry);
        
        // Act
        var result = await _listEntryService.UpdateListEntryAsync(listId, productId, 1, 1);
        
        // Assert
        result.Value.Should().BeEquivalentTo(listEntry);
    }
    
    [Fact]
    public async Task UpdateListEntryAsync_ReturnsListEntryByIdNotFound()
    {
        // Arrange
        const int listId = 1;
        const string productId = "product1";
        
        _listEntryRepositoryMock.Setup(x => x.GetListEntryAsync(It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync((ListEntry)null!);
        
        // Act
        var result = await _listEntryService.UpdateListEntryAsync(listId, productId, 1, 1);
        
        // Assert
        result.Error.Should().BeEquivalentTo(new ListEntryFetchingError.ListEntryByIdNotFound(listId, productId));
    }
    
    [Fact]
    public async Task UpdateListEntryAsync_ReturnsUnavailableProductInStore()
    {
        // Arrange
        const int listId = 1;
        const string productId = "product1";
        
        var listEntry = new ListEntry
        {
            ListId = listId,
            ProductId = productId,
            StoreId = 1,
            Quantity = 1
        };
        
        _listEntryRepositoryMock.Setup(x => x.GetListEntryAsync(It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync(listEntry);
        
        _priceRepositoryMock.Setup(x => x.GetStoresAvailabilityAsync(It.IsAny<string>(), It.IsAny<int?>()))
            .ReturnsAsync(new List<StoreAvailability>());
        
        // Act
        var result = await _listEntryService.UpdateListEntryAsync(listId, productId, 1, 1);
        
        // Assert
        result.Error.Should().BeEquivalentTo(new ProductFetchingError.UnavailableProductInStore(productId, 1));
    }
    
    [Fact]
    public async Task UpdateListEntryAsync_ReturnsListEntryQuantityInvalid()
    {
        // Arrange
        const int listId = 1;
        const string productId = "product1";
        
        var listEntry = new ListEntry
        {
            ListId = listId,
            ProductId = productId,
            StoreId = 1,
            Quantity = 0
        };
        
        _listEntryRepositoryMock.Setup(x => x.GetListEntryAsync(It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync(listEntry);
        
        _priceRepositoryMock.Setup(x => x.GetStoresAvailabilityAsync(It.IsAny<string>(), It.IsAny<int?>()))
            .ReturnsAsync(new List<StoreAvailability>() { 
                new StoreAvailability(
                    It.IsAny<int>(), 
                    It.IsAny<string>(), 
                    It.IsAny<int>(),
                    It.IsAny<bool>(),
                    It.IsAny<DateTime>()
                )});
        
        // Act
        var result = await _listEntryService.UpdateListEntryAsync(listId, productId, 1, 0);
        
        // Assert
        result.Error.Should().BeEquivalentTo(new ListEntryCreationError.ListEntryQuantityInvalid(0));
    }
    
    [Fact]
    public async Task DeleteListEntryAsync_ReturnsListEntry()
    {
        // Arrange
        const int listId = 1;
        const string productId = "product1";
        
        var listEntry = new ListEntry
        {
            ListId = listId,
            ProductId = productId,
            StoreId = 1,
            Quantity = 1
        };
        
        _listEntryRepositoryMock.Setup(x => x.GetListEntryAsync(It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync(listEntry);
        
        _listEntryRepositoryMock.Setup(x => x.DeleteListEntryAsync(It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync(listEntry);
        
        // Act
        var result = await _listEntryService.DeleteListEntryAsync(listId, productId);
        
        // Assert
        result.Value.Should().BeEquivalentTo(listEntry);
    }
    
    [Fact]
    public async Task DeleteListEntryAsync_ReturnsListEntryByIdNotFound()
    {
        // Arrange
        const int listId = 1;
        const string productId = "product1";
        
        _listEntryRepositoryMock.Setup(x => x.GetListEntryAsync(It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync((ListEntry)null!);
        
        // Act
        var result = await _listEntryService.DeleteListEntryAsync(listId, productId);
        
        // Assert
        result.Error.Should().BeEquivalentTo(new ListEntryFetchingError.ListEntryByIdNotFound(listId, productId));
    }
}