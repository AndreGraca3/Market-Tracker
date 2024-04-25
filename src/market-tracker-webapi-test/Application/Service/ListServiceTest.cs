using FluentAssertions;
using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Repository.Dto.List;
using market_tracker_webapi.Application.Repository.Dto.Price;
using market_tracker_webapi.Application.Repository.Dto.Product;
using market_tracker_webapi.Application.Repository.Dto.Store;
using market_tracker_webapi.Application.Repository.Operations.List;
using market_tracker_webapi.Application.Repository.Operations.Prices;
using market_tracker_webapi.Application.Repository.Operations.Product;
using market_tracker_webapi.Application.Repository.Operations.User;
using market_tracker_webapi.Application.Service.Errors.List;
using market_tracker_webapi.Application.Service.Errors.User;
using market_tracker_webapi.Application.Service.Operations.List;
using Moq;

namespace market_tracker_webapi_test.Application.Service;

public class ListServiceTest
{
    /*private readonly Mock<IListRepository> _listRepositoryMock; 
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IPriceRepository> _priceRepositoryMock;
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<IListEntryRepository> _listEntryRepositoryMock;
    
    private readonly ListService _listService;
    
    public ListServiceTest()
    {
        _listRepositoryMock = new Mock<IListRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _priceRepositoryMock = new Mock<IPriceRepository>();
        _productRepositoryMock = new Mock<IProductRepository>();
        _listEntryRepositoryMock = new Mock<IListEntryRepository>();
        
        _listService = new ListService(
            _listRepositoryMock.Object,
            _userRepositoryMock.Object,
            _priceRepositoryMock.Object,
            _productRepositoryMock.Object,
            _listEntryRepositoryMock.Object,
            new MockedTransactionManager()
        );
    }
    
    [Fact]
    public async Task GetListsAsync_ReturnsCollectionOutputModel()
    {
        // Arrange
        var lists = new List<ListOfProducts>
        {
            new()
            {
                Id = 1,
                ClientId = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11"),
                ListName = "List 1",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Unspecified)
            },
            new()
            {
                Id = 2,
                ClientId = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11"),
                ListName = "List 2",
                CreatedAt = new DateTime(2024, 1, 2, 0, 0, 0, DateTimeKind.Unspecified)
            }
        };
        
        _listRepositoryMock
            .Setup(x => x.GetListsAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>()))
            .ReturnsAsync(lists);
        
        // Act
        var result = await _listService.GetListsAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>());
        
        // Assert
        result.Value.Should().BeEquivalentTo(new CollectionOutputModel(lists));
    }
    
    [Fact]
    public async Task GetListByIdAsync_ReturnsListProduct()
    {
        // Arrange
        var expectListProduct = new ListProduct
        {
            Id = 1,
            Name = "List 1",
            ArchivedAt = null,
            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Unspecified),
            Products = new List<ListEntryDetails>()
            {
                new()
                {
                    Quantity = 1,
                    StorePrice = new StorePrice(
                        It.IsAny<StoreInfo>(), 
                        new PriceInfo(10, It.IsAny<Promotion>(), It.IsAny<DateTime>())),
                    IsAvailable = false,
                    ProductItem = new ProductItem()
                    {
                        ProductId = "product1",
                        Name = "product1"
                    }
                }
            },
            TotalPrice = 10,
            TotalProducts = 1
        };
        
        var listEntries = new List<ListEntry>
        {
            new()
            {
                ListId = 1,
                ProductId = "product1",
                Quantity = 1,
                StoreId = 1
            }
        };
        
        var listOfProducts = new ListOfProducts
        {
            Id = 1,
            ClientId = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11"),
            ListName = "List 1",
            ArchivedAt = null,
            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Unspecified)
        };
        
        _listRepositoryMock
            .Setup(x => x.GetListByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(listOfProducts);
        
        _listEntryRepositoryMock
            .Setup(x => x.GetListEntriesAsync(It.IsAny<int?>(), It.IsAny<string?>(), It.IsAny<int?>(), It.IsAny<int?>()))
            .ReturnsAsync(listEntries);

        _productRepositoryMock
            .Setup(x => x.GetProductByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(new ProductDetails(
                It.IsAny<string>(), "product1", It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<double>(),It.IsAny<Brand>(), It.IsAny<Category>()));
        
        _priceRepositoryMock
            .Setup(x => x.GetStorePriceByProductIdAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<DateTime>()))
            .ReturnsAsync(new StorePrice(It.IsAny<StoreInfo>(), new PriceInfo(10, It.IsAny<Promotion>(), It.IsAny<DateTime>())));

        _priceRepositoryMock
            .Setup(x => x.GetStoresAvailabilityAsync(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(It.IsAny<IEnumerable<StoreAvailability>>());
        
        // Act
        var result = await _listService.GetListByIdAsync(It.IsAny<int>());
        
        // Assert
        result.Value.Should().BeEquivalentTo(expectListProduct);
    }
    
    [Fact]
    public async Task GetListByIdAsync_ReturnsListByIdNotFound()
    {
        // Arrange
        _listRepositoryMock
            .Setup(x => x.GetListByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((ListOfProducts)null!);
        
        // Act
        var result = await _listService.GetListByIdAsync(It.IsAny<int>());
        
        // Assert
        result.Error.Should().BeEquivalentTo(new ListFetchingError.ListByIdNotFound(It.IsAny<int>()));
    }
    
    [Fact]
    public async Task AddListAsync_ReturnsIntIdOutputModel()
    {
        // Arrange
        var list = new ListOfProducts
        {
            Id = 1,
            ClientId = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11"),
            ListName = "List 1",
            ArchivedAt = null,
            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Unspecified)
        };

        _userRepositoryMock
            .Setup(x => x.GetUserByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new User(
                It.IsAny<Guid>(), 
                It.IsAny<string>(), 
                It.IsAny<string>(), 
                It.IsAny<string>(), 
                It.IsAny<string>(), 
                It.IsAny<DateTime>()
                ));
            
        _listRepositoryMock
            .Setup(x => x.AddListAsync(It.IsAny<Guid>(), It.IsAny<string>()))
            .ReturnsAsync(list.Id);
        
        // Act
        var result = await _listService.AddListAsync(It.IsAny<Guid>(), It.IsAny<string>());
        
        // Assert
        result.Value.Should().BeEquivalentTo(new IntIdOutputModel(list.Id));
    }
    
    [Fact]
    public async Task AddListAsync_ReturnsUserByIdNotFound()
    {
        // Arrange
        _userRepositoryMock
            .Setup(x => x.GetUserByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((User)null!);
        
        // Act
        var result = await _listService.AddListAsync(It.IsAny<Guid>(), It.IsAny<string>());
        
        // Assert
        result.Error.Should().BeEquivalentTo(new UserFetchingError.UserByIdNotFound(It.IsAny<Guid>()));
    }
    
    [Fact]
    public async Task AddListAsync_ReturnsListNameAlreadyExists()
    {
        // Arrange
        var list = new ListOfProducts
        {
            Id = 1,
            ClientId = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11"),
            ListName = "List 1",
            ArchivedAt = null,
            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Unspecified)
        };
        
        _userRepositoryMock
            .Setup(x => x.GetUserByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new User(
                It.IsAny<Guid>(), 
                It.IsAny<string>(), 
                It.IsAny<string>(), 
                It.IsAny<string>(), 
                It.IsAny<string>(), 
                It.IsAny<DateTime>()
            ));
        
        _listRepositoryMock
            .Setup(x => x.GetListsAsync(
                It.IsAny<Guid>(), 
                It.IsAny<string?>(), 
                It.IsAny<DateTime?>(), 
                It.IsAny<DateTime?>()))
            .ReturnsAsync(new List<ListOfProducts> { list });
        
        // Act
        var result = await _listService.AddListAsync(It.IsAny<Guid>(), It.IsAny<string>());
        
        // Assert
        result.Error.Should().BeEquivalentTo(new ListCreationError.ListNameAlreadyExists(It.IsAny<Guid>(), It.IsAny<string>()));
    }
    
    [Fact]
    public async Task UpdateListAsync_ReturnsListOfProducts()
    {
        // Arrange
        var list = new ListOfProducts
        {
            Id = 1,
            ClientId = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11"),
            ListName = "List 1",
            ArchivedAt = null,
            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Unspecified)
        };
        
        _listRepositoryMock
            .Setup(x => x.GetListByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(list);
        
        _listRepositoryMock
            .Setup(x => x.UpdateListAsync(It.IsAny<int>(), It.IsAny<string?>(), It.IsAny<DateTime?>()))
            .ReturnsAsync(list);
        
        // Act
        var result = await _listService.UpdateListAsync(It.IsAny<int>(), list.ClientId, It.IsAny<string?>(), null);
        
        // Assert
        result.Value.Should().BeEquivalentTo(list);
    }
    
    [Fact]
    public async Task UpdateListAsync_ReturnsListByIdNotFound()
    {
        // Arrange
        _listRepositoryMock
            .Setup(x => x.GetListByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((ListOfProducts)null!);
        
        // Act
        var result = await _listService.UpdateListAsync(It.IsAny<int>(), It.IsAny<Guid>(), It.IsAny<string?>(), null);
        
        // Assert
        result.Error.Should().BeEquivalentTo(new ListFetchingError.ListByIdNotFound(It.IsAny<int>()));
    }
    
    [Fact]
    public async Task UpdateListAsync_ReturnsUserDoNotOwnList()
    {
        // Arrange
        var list = new ListOfProducts
        {
            Id = 1,
            ClientId = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a12"),
            ListName = "List 1",
            ArchivedAt = null,
            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Unspecified)
        };
        
        _listRepositoryMock
            .Setup(x => x.GetListByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(list);
        
        // Act
        var result = await _listService.UpdateListAsync(1, Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11"), It.IsAny<string?>(), null);
        
        // Assert
        result.Error.Should().BeEquivalentTo(new UserPermissionsError.UserDoNotOwnList(Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11"), list.Id));
    }

    [Fact]
    public async Task UpdateListAsync_ReturnsListIsArchived()
    {
        // Arrange
        var list = new ListOfProducts
        {
            Id = 1,
            ClientId = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11"),
            ListName = "List 1",
            ArchivedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Unspecified),
            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Unspecified)
        };
        
        _listRepositoryMock
            .Setup(x => x.GetListByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(list);
        
        // Act
        var result = await _listService.UpdateListAsync(1, list.ClientId, It.IsAny<string?>(), null);
        
        // Assert
        result.Error.Should().BeEquivalentTo(new ListUpdateError.ListIsArchived(list.Id));
    }
    
    [Fact]
    public async Task DeleteListAsync_ReturnsListOfProducts()
    {
        // Arrange
        var list = new ListOfProducts
        {
            Id = 1,
            ClientId = Guid.Parse("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11"),
            ListName = "List 1",
            ArchivedAt = null,
            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Unspecified)
        };
        
        _listRepositoryMock
            .Setup(x => x.GetListByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(list);
        
        _listRepositoryMock
            .Setup(x => x.DeleteListAsync(It.IsAny<int>()))
            .ReturnsAsync(list);
        
        // Act
        var result = await _listService.DeleteListAsync(It.IsAny<int>());
        
        // Assert
        result.Value.Should().BeEquivalentTo(list);
    }
    
    [Fact]
    public async Task DeleteListAsync_ReturnsListByIdNotFound()
    {
        // Arrange
        _listRepositoryMock
            .Setup(x => x.GetListByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((ListOfProducts)null!);
        
        // Act
        var result = await _listService.DeleteListAsync(It.IsAny<int>());
        
        // Assert
        result.Error.Should().BeEquivalentTo(new ListFetchingError.ListByIdNotFound(It.IsAny<int>()));
    }*/
}