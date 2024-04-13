using FluentAssertions;
using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Repository.Dto.List;
using market_tracker_webapi.Application.Repository.Operations.List;
using market_tracker_webapi.Application.Repository.Operations.Prices;
using market_tracker_webapi.Application.Repository.Operations.Product;
using market_tracker_webapi.Application.Repository.Operations.User;
using market_tracker_webapi.Application.Service.Operations.List;
using Moq;

namespace market_tracker_webapi_test.Application.Service;

public class ListServiceTest
{
    private readonly Mock<IListRepository> _listRepositoryMock; 
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
        var list = new ListProduct
        {
            Id = 1,
            Name = "List 1",
            ArchivedAt = null,
            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Unspecified),
            Products = new List<ListEntryDetails>(),
            TotalPrice = 0,
            TotalProducts = 0
        };
        
        _listRepositoryMock
            .Setup(x => x.GetListByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(list);
        
        _listEntryRepositoryMock
            .Setup(x => x.GetListEntriesAsync(It.IsAny<int>()))
            .ReturnsAsync(new List<ListEntry>());
        
        // Act
        var result = await _listService.GetListByIdAsync(It.IsAny<int>());
        
        // Assert
        result.Value.Should().BeEquivalentTo(list);
    }
}