using FluentAssertions;
using market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Sales;
using market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Sales.Pricing;
using market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Shop;
using market_tracker_webapi.Application.Repository.Market.Price;
using market_tracker_webapi.Application.Service.Operations.Market.Inventory.Product;
using market_tracker_webapi.Application.Service.Results;
using Moq;

namespace market_tracker_webapi_test.Application.Service;

public class ProductPriceServiceTest
{
    private readonly Mock<IPriceRepository> _priceRepositoryMock;
    
    private readonly ProductPriceService _productPriceService;
    
    public ProductPriceServiceTest()
    {
        _priceRepositoryMock = new Mock<IPriceRepository>();
        
        _productPriceService = new ProductPriceService(_priceRepositoryMock.Object);
    }
    
    [Fact]
    public async Task GetProductPricesAsync_WhenCalled_ShouldReturnCompaniesPricesResult()
    {
        // Arrange
        var store = new Store(
            1, 
            "store1", 
            "address1", 
            new City(1, "city1"), 
            new Company(1, "company1", "logoUrl1", new DateTime(2024, 01, 01, 0 ,0 ,0, DateTimeKind.Unspecified)), 
            Guid.Parse("00000000-0000-0000-0000-000000000001")
            );

        var price = new Price(
            10, 
            new Promotion(
                10, 
                new DateTime(2024, 01, 01, 0 ,0 ,0, DateTimeKind.Unspecified)), 
            new DateTime(2024, 01, 01, 0 ,0 ,0, 
                DateTimeKind.Unspecified));
        
        var storeAvailability = new StoreAvailability(1, "1", true, new DateTime(2024, 01, 01, 0 ,0 ,0, DateTimeKind.Unspecified));
        
        var storeOffer = new StoreOffer(store, price, storeAvailability);
        
        _priceRepositoryMock
            .Setup(x => x.GetStoresAvailabilityAsync(It.IsAny<string>()))
            .ReturnsAsync(new List<StoreAvailability> { storeAvailability });
        
        _priceRepositoryMock
            .Setup(x => x.GetStoreOfferAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<DateTime>()
            ))
            .ReturnsAsync(storeOffer);
        
        // Act
        var result = await _productPriceService.GetProductPricesAsync("1");
        
        // Assert
        var companiesPricesResult = Assert.IsType<CompaniesPricesResult>(result);
        
        var expectedCompaniesPrices = new List<CompanyPrices>
        {
            new CompanyPrices(
                1,
                "company1",
                "logoUrl1",
                new List<StoreOffer> { storeOffer }
            )
        };
        
        var expectedCompaniesPricesResult = new CompaniesPricesResult(expectedCompaniesPrices, 9, 9);

        companiesPricesResult.Should().BeEquivalentTo(expectedCompaniesPricesResult);
    }
    
    [Fact]
    public async Task GetProductPricesAsync_WhenCalledWithNoStores_ShouldReturnCompaniesPricesResultWithNoCompanies()
    {
        // Arrange
        _priceRepositoryMock
            .Setup(x => x.GetStoresAvailabilityAsync(It.IsAny<string>()))
            .ReturnsAsync(new List<StoreAvailability>());
        
        // Act
        var result = await _productPriceService.GetProductPricesAsync("1");
        
        // Assert
        var companiesPricesResult = Assert.IsType<CompaniesPricesResult>(result);
        
        var expectedCompaniesPricesResult = new CompaniesPricesResult(new List<CompanyPrices>(), 0, 0);

        companiesPricesResult.Should().BeEquivalentTo(expectedCompaniesPricesResult);
    }
    
    [Fact]
    public async Task GetProductPricesAsync_WhenCalledWithNoStoreOffers_ShouldReturnCompaniesPricesResultWithNoCompanies()
    {
        // Arrange
        var storeAvailability = new StoreAvailability(1, "1", true, new DateTime(2024, 01, 01, 0 ,0 ,0, DateTimeKind.Unspecified));
        
        _priceRepositoryMock
            .Setup(x => x.GetStoresAvailabilityAsync(It.IsAny<string>()))
            .ReturnsAsync(new List<StoreAvailability> { storeAvailability });
        
        _priceRepositoryMock
            .Setup(x => x.GetStoreOfferAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<DateTime>()))
            .ReturnsAsync((StoreOffer)null);
        
        // Act
        var result = await _productPriceService.GetProductPricesAsync("1");
        
        // Assert
        var companiesPricesResult = Assert.IsType<CompaniesPricesResult>(result);
        
        var expectedCompaniesPricesResult = new CompaniesPricesResult(new List<CompanyPrices>(), 0, 0);

        companiesPricesResult.Should().BeEquivalentTo(expectedCompaniesPricesResult);
    }




}