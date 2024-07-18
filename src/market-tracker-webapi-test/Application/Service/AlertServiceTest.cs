using FluentAssertions;
using market_tracker_webapi.Application.Domain.Schemas.Account;
using market_tracker_webapi.Application.Domain.Schemas.Account.Auth;
using market_tracker_webapi.Application.Domain.Schemas.Market.Inventory;
using market_tracker_webapi.Application.Domain.Schemas.Market.Inventory.Product;
using market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Sales.Pricing;
using market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Shop;
using market_tracker_webapi.Application.Repository.Account.Users.Client;
using market_tracker_webapi.Application.Repository.Market.Alert;
using market_tracker_webapi.Application.Repository.Market.Inventory.Product;
using market_tracker_webapi.Application.Repository.Market.Price;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Operations.Market.Alert;
using Moq;

namespace market_tracker_webapi_test.Application.Service;

public class AlertServiceTest
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<IPriceRepository> _priceRepositoryMock;
    private readonly Mock<IPriceAlertRepository> _priceAlertRepositoryMock;
    private readonly Mock<IClientDeviceRepository> _clientDeviceRepositoryMock;
    
    private readonly AlertService _alertService;
    
    public AlertServiceTest()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _priceRepositoryMock = new Mock<IPriceRepository>();
        _priceAlertRepositoryMock = new Mock<IPriceAlertRepository>();
        _clientDeviceRepositoryMock = new Mock<IClientDeviceRepository>();
        
        _alertService = new AlertService(
            new MockedTransactionManager(),
            _productRepositoryMock.Object,
            _priceRepositoryMock.Object,
            _priceAlertRepositoryMock.Object,
            _clientDeviceRepositoryMock.Object
        );
    }
    
    [Fact]
    public async Task GetPriceAlertsByClientIdAsync_ShouldReturnPriceAlerts()
    {
        // Arrange
        var pricesAlerts = new List<PriceAlert>
        {
            new PriceAlert(
                "1", 
                Guid.Parse("00000000-0000-0000-0000-000000000001"), 
                "1", 
                1, 
                1, 
                new DateTime(2024, 01, 01, 0 ,0 ,0, DateTimeKind.Unspecified)
                ),
            new PriceAlert(
                "2", 
                Guid.Parse("00000000-0000-0000-0000-000000000002"), 
                "2", 
                2, 
                2, 
                new DateTime(2024, 01, 01, 0 ,0 ,0, DateTimeKind.Unspecified)
            ),
        };
        
        _priceAlertRepositoryMock
            .Setup(x => x.GetPriceAlertsAsync(It.IsAny<Guid>(), It.IsAny<string?>(), It.IsAny<int?>(), It.IsAny<int?>()))
            .ReturnsAsync(pricesAlerts);
        
        // Act
        var result = await _alertService.GetPriceAlertsByClientIdAsync(
            Guid.Parse("00000000-0000-0000-0000-000000000001"), 
            "1", 
            1
        );
        
        // Assert
        result.Should().BeEquivalentTo(pricesAlerts);
    }

    [Fact]
    public async Task AddPriceAlertAsync_ShouldReturnPriceAlertId()
    {
        // Arrange
        var product = new Product(
            "1",
            "Product 1",
            "ImageUrl 1",
            1,
            ProductUnit.Units,
            1,
            1,
            new Brand(1, "Brand 1"),
            new Category(1, "Category 1")
            );
        
        var storeAvailability = new StoreAvailability(
            1,
            "1",
            true,
            new DateTime(2024, 01, 01, 0 ,0 ,0, DateTimeKind.Unspecified)
            );
        
        var priceAlert = new PriceAlert(
            "1",
            Guid.Parse("00000000-0000-0000-0000-000000000001"),
            "1",
            1,
            1,
            new DateTime(2024, 01, 01, 0 ,0 ,0, DateTimeKind.Unspecified)
            );

        var deviceTokens = new List<DeviceToken>()
        {
            new(
                Guid.Parse("00000000-0000-0000-0000-000000000001"),
                "Device 1",
                "Token 1"
                )
        };
        
        var priceAlertId = new PriceAlertId("1");
        
        _productRepositoryMock
            .Setup(x => x.GetProductByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(product);
        
        _priceRepositoryMock
            .Setup(x => x.GetStoreAvailabilityStatusAsync(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(storeAvailability);
        
        _clientDeviceRepositoryMock
            .Setup(x => x.GetDeviceTokensByClientIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(deviceTokens);
        
        _priceAlertRepositoryMock
            .Setup(x => x.AddPriceAlertAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(priceAlertId);
        
        // Act
        var result = await _alertService.AddPriceAlertAsync(
            Guid.Parse("00000000-0000-0000-0000-000000000001"),
            "1",
            1,
            1
        );
        
        // Assert
        result.Should().BeEquivalentTo(priceAlertId);
    }
    
    [Fact]
    public async Task AddPriceAlertAsync_ShouldThrowProductFetchingErrorProductByIdNotFound()
    {
        // Arrange
        _productRepositoryMock
            .Setup(x => x.GetProductByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((Product?)null);
        
        // Act
        Func<Task> act = async () => await _alertService.AddPriceAlertAsync(
            Guid.Parse("00000000-0000-0000-0000-000000000001"),
            "1",
            1,
            1
        );
        
        // Assert
        await act.Should().ThrowAsync<MarketTrackerServiceException>();
    }
    
    [Fact]
    public async Task AddPriceAlertAsync_ShouldThrowProductFetchingErrorOutOfStockInStore()
    {
        // Arrange
        var product = new Product(
            "1",
            "Product 1",
            "ImageUrl 1",
            1,
            ProductUnit.Units,
            1,
            1,
            new Brand(1, "Brand 1"),
            new Category(1, "Category 1")
            );
        
        _productRepositoryMock
            .Setup(x => x.GetProductByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(product);
        
        _priceRepositoryMock
            .Setup(x => x.GetStoreAvailabilityStatusAsync(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync((StoreAvailability?)null);
        
        // Act
        Func<Task> act = async () => await _alertService.AddPriceAlertAsync(
            Guid.Parse("00000000-0000-0000-0000-000000000001"),
            "1",
            1,
            1
        );
        
        // Assert
        await act.Should().ThrowAsync<MarketTrackerServiceException>();
    }
    
    [Fact]
    public async Task AddPriceAlertAsync_ShouldThrowAlertCreationErrorProductAlreadyHasPriceAlertInStore()
    {
        // Arrange
        var product = new Product(
            "1",
            "Product 1",
            "ImageUrl 1",
            1,
            ProductUnit.Units,
            1,
            1,
            new Brand(1, "Brand 1"),
            new Category(1, "Category 1")
            );
        
        var storeAvailability = new StoreAvailability(
            1,
            "1",
            true,
            new DateTime(2024, 01, 01, 0 ,0 ,0, DateTimeKind.Unspecified)
            );
        
        var priceAlert = new PriceAlert(
            "1",
            Guid.Parse("00000000-0000-0000-0000-000000000001"),
            "1",
            1,
            1,
            new DateTime(2024, 01, 01, 0 ,0 ,0, DateTimeKind.Unspecified)
            );
        
        _productRepositoryMock
            .Setup(x => x.GetProductByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(product);
        
        _priceRepositoryMock
            .Setup(x => x.GetStoreAvailabilityStatusAsync(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(storeAvailability);
        
        _priceAlertRepositoryMock
            .Setup(x => x.GetPriceAlertAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(priceAlert);
        
        // Act
        Func<Task> act = async () => await _alertService.AddPriceAlertAsync(
            Guid.Parse("00000000-0000-0000-0000-000000000001"),
            "1",
            1,
            1
        );
        
        // Assert
        await act.Should().ThrowAsync<MarketTrackerServiceException>();
    }
    
    [Fact]
    public async Task AddPriceAlertAsync_ShouldThrowAlertCreationErrorNoDeviceTokensFound()
    {
        // Arrange
        var product = new Product(
            "1",
            "Product 1",
            "ImageUrl 1",
            1,
            ProductUnit.Units,
            1,
            1,
            new Brand(1, "Brand 1"),
            new Category(1, "Category 1")
            );
        
        var storeAvailability = new StoreAvailability(
            1,
            "1",
            true,
            new DateTime(2024, 01, 01, 0 ,0 ,0, DateTimeKind.Unspecified)
            );
        
        _productRepositoryMock
            .Setup(x => x.GetProductByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(product);
        
        _priceRepositoryMock
            .Setup(x => x.GetStoreAvailabilityStatusAsync(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(storeAvailability);
        
        _clientDeviceRepositoryMock
            .Setup(x => x.GetDeviceTokensByClientIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new List<DeviceToken>());
        
        // Act
        Func<Task> act = async () => await _alertService.AddPriceAlertAsync(
            Guid.Parse("00000000-0000-0000-0000-000000000001"),
            "1",
            1,
            1
        );
        
        // Assert
        await act.Should().ThrowAsync<MarketTrackerServiceException>();
    }
    
    [Fact]
    public async Task RemovePriceAlertAsync_ShouldReturnPriceAlert()
    {
        // Arrange
        var priceAlert = new PriceAlert(
            "1",
            Guid.Parse("00000000-0000-0000-0000-000000000001"),
            "1",
            1,
            1,
            new DateTime(2024, 01, 01, 0 ,0 ,0, DateTimeKind.Unspecified)
            );
        
        _priceAlertRepositoryMock
            .Setup(x => x.RemovePriceAlertByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(priceAlert);
        
        // Act
        var result = await _alertService.RemovePriceAlertAsync(
            Guid.Parse("00000000-0000-0000-0000-000000000001"),
            "1"
        );
        
        // Assert
        result.Should().BeEquivalentTo(priceAlert);
    }
    
    [Fact]
    public async Task RemovePriceAlertAsync_ShouldThrowAlertRemovalErrorPriceAlertNotFound()
    {
        // Arrange
        _priceAlertRepositoryMock
            .Setup(x => x.RemovePriceAlertByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((PriceAlert?)null);
        
        // Act
        Func<Task> act = async () => await _alertService.RemovePriceAlertAsync(
            Guid.Parse("00000000-0000-0000-0000-000000000001"),
            "1"
        );
        
        // Assert
        await act.Should().ThrowAsync<MarketTrackerServiceException>();
    }
    
    [Fact]
    public async Task RemovePriceAlertAsync_ShouldThrowAlertRemovalErrorClientDoesNotOwnAlert()
    {
        // Arrange
        var priceAlert = new PriceAlert(
            "1",
            Guid.Parse("00000000-0000-0000-0000-000000000002"),
            "1",
            1,
            1,
            new DateTime(2024, 01, 01, 0 ,0 ,0, DateTimeKind.Unspecified)
            );
        
        _priceAlertRepositoryMock
            .Setup(x => x.RemovePriceAlertByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(priceAlert);
        
        // Act
        Func<Task> act = async () => await _alertService.RemovePriceAlertAsync(
            Guid.Parse("00000000-0000-0000-0000-000000000001"),
            "1"
        );
        
        // Assert
        await act.Should().ThrowAsync<MarketTrackerServiceException>();
    }
}